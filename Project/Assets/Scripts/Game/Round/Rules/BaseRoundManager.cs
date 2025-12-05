using System.Collections.Generic;
using TowerDefence.Core;
using TowerDefence.Game.AI;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Round.States;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Spawning;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
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
        [SerializeField] private Player playerPrefab;

        [Header("Rules")]
        [SerializeField, Min(1)] private int maxPlayers = 4;
        [SerializeField, Min(0f)] private float warmupDuration = 1f;
        [SerializeField, Min(0f)] private float waitForGameOverDuration = 3f;

        private readonly List<Player> _players = new List<Player>();
        private PlayerBuilder _playerBuilder;

        private readonly StateMachine _stateMachine = new StateMachine();
        private IState _warmupState;

        private ITickDispatcher _tickDispatcher;

        public IReadOnlyList<Player> Players => _players;
        public Player RealPlayer => _players[0];
        private PlayerBuilder PlayerBuilder => _playerBuilder ??= new PlayerBuilder(playerPrefab);

        public void Init()
        {
            _warmupState = new WarmupState(this, warmupDuration);

            _tickDispatcher = Services.Get<ITickDispatcher>();
            _tickDispatcher.Subscribe(_stateMachine.Tick);
        }

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

        protected abstract IState GetMatchState();

        public void SpawnAllPlayers()
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                int spawnPointIndex = i % spawnPoints.Length;
                int teamIndex = i % teamSettings.Teams.Length;
                IPlayerInputSource inputSource = i == 0 ? uiControls : new SimpleBot();
                SpawnPlayer(spawnPoints[spawnPointIndex], teamIndex, inputSource);
            }
        }

        public void DespawnAllPlayers()
        {
            for (int i = _players.Count-1; i >= 0; i--)
            {
                Destroy(_players[i].gameObject);
            }

            _players.Clear();
        }

        public void SetCameraTarget(Transform target)
        {
            cinemachineCamera.Target = new CameraTarget
            {
                TrackingTarget = target,
                LookAtTarget = target
            };
        }

        private void SpawnPlayer(SpawnPointComponent spawnPoint, int teamIndex, IPlayerInputSource inputSource)
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
                .WithInput(inputSource)
                .Build();

            _players.Add(player);
        }

        public void SetWarmupState() => _stateMachine.SetState(_warmupState);

        public void SetMatchState() => _stateMachine.SetState(GetMatchState());

        public void SetWinState() => _stateMachine.SetState(new RoundEndState(this, true, waitForGameOverDuration));

        public void SetLoseState() => _stateMachine.SetState(new RoundEndState(this, false, waitForGameOverDuration));

        public void SetPostRoundState() => _stateMachine.SetState(null);
    }
}
