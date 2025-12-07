using System;
using TowerDefence.Core;
using TowerDefence.Game.AI;
using TowerDefence.Game.AI.Navigation;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Events;
using TowerDefence.Game.Round.States;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Spawning;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
using TowerDefence.Infrastructure.Factory;
using Unity.Cinemachine;
using UnityEngine;

namespace TowerDefence.Game.Round.Rules
{
    public abstract class BaseRoundManager : MonoBehaviour, IRoundManager, ITickable
    {
        [Header("Scene References")]
        [SerializeField] protected CinemachineCamera cinemachineCamera;
        [SerializeField] protected UiControlsInputSource uiControls;
        [SerializeField] protected SpawnPointComponent[] spawnPoints;
        [Header("Prefabs and Settings")]
        [SerializeField] protected TeamSettings teamSettings;
        [SerializeField] protected RaceSettings raceSettings;
        [SerializeField] protected WeaponSettings weaponSettings;
        [SerializeField] private BotsSettings botsSettings;
        [SerializeField] private Player playerPrefab;

        [Header("Rules")]
        [SerializeField, Min(1)] private int maxPlayers = 4;
        [SerializeField, Min(0f)] private float warmupDuration = 1f;
        [SerializeField, Min(0f)] protected float waitForGameOverDuration = 3f;

        protected readonly RoundStateMachine stateMachine = new ();
        private IRoundState _warmupState;

        private IEventBus _eventBus;
        private ITickDispatcher _tickDispatcher;
        private IAIManager _aiManager;
        private PlayerBuilder _playerBuilder;
        private IPlayerRegistry _playerRegistry;
        private IFactory<IWaypointGenerator> _waypointGeneratorFactory;
        private BotStatesFactory _botStatesFactory;
        private BotFactory _botFactory;

        public Player RealPlayer => _playerRegistry.Players[0];
        private PlayerBuilder PlayerBuilder => _playerBuilder ??= new PlayerBuilder(playerPrefab);

        private void Awake()
        {
            Init();
            ServiceLocator.Instance.Register<IRoundManager>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance?.Unregister<IRoundManager>();
            _tickDispatcher.Unsubscribe(Tick);
            stateMachine.SetState(null);
        }

        public void Init()
        {
            DoComposition();

            _eventBus = Services.Get<IEventBus>();
            _aiManager = Services.Get<IAIManager>();
            _tickDispatcher = Services.Get<ITickDispatcher>();
            _tickDispatcher.Subscribe(Tick);
            _warmupState = new WarmupState(this, warmupDuration);
        }

        public void Tick(float deltaTime)
        {
            stateMachine.Tick(deltaTime);
            if (stateMachine.CurrentState == null || stateMachine.CurrentState.Intention == RoundStateType.None) return;
            HandleIntention(stateMachine.CurrentState.Intention, stateMachine.CurrentState.Payload);
        }

        public void StartRound()
        {
            HandleIntention(RoundStateType.Warmup, null);
        }

        public void SetCameraTarget(Transform target)
        {
            cinemachineCamera.Target = new CameraTarget
            {
                TrackingTarget = target,
                LookAtTarget = target
            };
        }

        // TODO: Move to SceneCompositionRoot
        private void DoComposition()
        {
            _playerRegistry = new PlayerRegistry();
            _waypointGeneratorFactory = new SquarePlaneRandomPositionGeneratorFactory(
                Vector2.one * -25f,
                Vector2.one * 25f,
                0f);

            _botStatesFactory = new BotStatesFactory(botsSettings, _waypointGeneratorFactory.Create());
            _botFactory = new BotFactory(_botStatesFactory);
        }

        private void HandleIntention(RoundStateType intention, object payload)
        {
            switch (intention)
            {
                case RoundStateType.Warmup:
                    SpawnAllPlayers();
                    stateMachine.SetState(_warmupState);
                    break;
                case RoundStateType.Match:
                    SetPlayerInputActive(true);
                    stateMachine.SetState(GetMatchState());
                    _eventBus.Publish(new MatchStartEvent());
                    break;
                case RoundStateType.RoundResults:
                    SetPlayerInputActive(false);
                    _eventBus.Publish(new MatchEndEvent());
                    var roundResultsState = GetRoundResultsState((RoundResults)payload);
                    stateMachine.SetState(roundResultsState);
                    break;
                case RoundStateType.PostRound:
                    DespawnAllPlayers();
                    stateMachine.SetState(null);
                    var globalStateMachine = Services.Get<IStateMachine>();
                    globalStateMachine.SetState(new GameOverState());
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(intention), intention, null);
            }
        }

        private void SpawnAllPlayers()
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                var spawnPoint = spawnPoints[i % spawnPoints.Length];
                int teamIndex = i % teamSettings.Teams.Length;
                bool isRealPlayer = i == 0;
                var player = SpawnPlayer(spawnPoint, teamIndex, isRealPlayer);
                player.name = $"Player{i}_{(isRealPlayer ? "real" : "bot")}";
            }
        }

        // TODO: move to PlayerSpawner or something like this
        private Player SpawnPlayer(SpawnPointComponent spawnPoint, int teamIndex, bool isRealPlayer)
        {
            var spawnTransform = spawnPoint.transform;
            var race = raceSettings.GetRandom();
            var weaponInstance = Instantiate(weaponSettings.GetRandomPrefab());

            var player = PlayerBuilder
                .CreateNewPlayer()
                .InPosition(spawnTransform.position)
                .WithRotation(spawnTransform.rotation)
                .WithRace(race)
                .WithWeapon(weaponInstance)
                .WithTeam(teamSettings, teamIndex)
                .Build();

            _playerRegistry.RegisterPlayer(player);

            if (isRealPlayer)
            {
                player.SetInputSource(uiControls);
            }
            else
            {
                var bot = _botFactory.CreateBot(player);
                _aiManager.RegisterBot(bot);
            }

            return player;
        }

        private void DespawnAllPlayers()
        {
            _playerRegistry.DisposeAllPlayers();
            _aiManager.DisposeAllBots();
        }

        private void SetPlayerInputActive(bool active)
        {
            foreach (var player in _playerRegistry.Players)
            {
                player.SetInputEnabled(active);
            }

            uiControls.gameObject.SetActive(active);
        }

        protected abstract IRoundState GetMatchState();

        protected abstract IRoundState GetRoundResultsState(RoundResults results);
    }
}
