using TowerDefence.Core;
using TowerDefence.Game.AI;
using TowerDefence.Game.AI.Navigation;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Round.States;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Spawning;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
using TowerDefence.Infrastructure.Factory;
using TowerDefence.Providers;
using Unity.Cinemachine;
using UnityEngine;

namespace TowerDefence.Game.Round.Rules
{
    public abstract class BaseRoundManager : MonoBehaviour, IRoundManager
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

        private PlayerBuilder _playerBuilder;

        private ITickDispatcher _tickDispatcher;
        private IAIManager _aiManager;

        protected readonly StateMachine _stateMachine = new StateMachine();
        private IState _warmupState;

        protected IPlayerRegistry _playerRegistry;
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
            _tickDispatcher.Unsubscribe(_stateMachine.Tick);
            _stateMachine.SetState(null);
        }

        public void Init()
        {
            DoComposition();

            _aiManager = Services.Get<IAIManager>();
            _tickDispatcher = Services.Get<ITickDispatcher>();
            _tickDispatcher.Subscribe(_stateMachine.Tick);
            _warmupState = new WarmupState(this, _playerRegistry, warmupDuration);
        }

        // TODO: Move to SceneCompositionRoot
        private void DoComposition()
        {
            _playerRegistry = new PlayerRegistry();
            _waypointGeneratorFactory = new SquarePlaneRandomPositionGeneratorFactory(
                Vector2.one * -25f,
                Vector2.one * 25f,
                0f);

            _botStatesFactory = new BotStatesFactory(botsSettings, _playerRegistry, _waypointGeneratorFactory.Create());
            _botFactory = new BotFactory(_botStatesFactory);
        }

        public void SpawnAllPlayers()
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                var spawnPoint = spawnPoints[i % spawnPoints.Length];
                int teamIndex = i % teamSettings.Teams.Length;
                bool isRealPlayer = i == 0;
                SpawnPlayer(spawnPoint, teamIndex, isRealPlayer);
            }
        }

        public void DespawnAllPlayers()
        {
            _playerRegistry.DisposeAllPlayers();
        }

        public void SetCameraTarget(Transform target)
        {
            cinemachineCamera.Target = new CameraTarget
            {
                TrackingTarget = target,
                LookAtTarget = target
            };
        }

        public void SetPlayerInputActive(bool active)
        {
            foreach (var player in _playerRegistry.Players)
            {
                player.SetInputEnabled(active);
            }

            uiControls.gameObject.SetActive(active);
        }

        // TODO: move to PlayerSpawner or something like this
        private void SpawnPlayer(SpawnPointComponent spawnPoint, int teamIndex, bool isRealPlayer)
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
        }

        public void SetWarmupState() => _stateMachine.SetState(_warmupState);

        public void SetMatchState() => _stateMachine.SetState(GetMatchState());

        protected abstract IState GetMatchState();

        public abstract void SetRoundEndState(RoundResults results);

        public void SetPostRoundState() => _stateMachine.SetState(null);
    }
}
