using System.Collections.Generic;
using System.Linq;
using TowerDefence.Core;
using TowerDefence.Game.AI;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Events;
using TowerDefence.Game.Round.States;
using TowerDefence.Game.Rules.ConversionClash;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Spawning;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Game.Round.Rules
{
    public class ConversionClashRules : MonoBehaviour, ITeamRegistry
    {
        [Header("Scene References")]
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private UiControlsInputSource uiControls;
        [SerializeField] private SpawnPointComponent[] spawnPoints;
        [Header("Prefabs and Settings")]
        [SerializeField] private TeamSettings teamSettings;
        [SerializeField] private RaceSettings raceSettings;
        [SerializeField] private WeaponSettings weaponSettings;
        [SerializeField] private Player playerPrefab;

        [Header("Rules")]
        [SerializeField, Min(1)] private int maxPlayers = 4;
        [SerializeField, Min(0f)] private float warmupDuration = 1f; // TODO: replace with countdown popup
        [SerializeField] private float respawnDelay = 1f;

        private readonly List<Player> _players = new List<Player>();
        private PlayerBuilder _playerBuilder;

        private readonly StateMachine _stateMachine = new StateMachine();
        private IState _warmupState;
        private IState _matchState;

        private ITickDispatcher _tickDispatcher;
        //private IEventBus _eventBus;

        public IReadOnlyList<Player> Players => _players;
        public Player RealPlayer => _players[0];
        private PlayerBuilder PlayerBuilder => _playerBuilder ??= new PlayerBuilder(playerPrefab);
        //private IEventBus EventBus => _eventBus ??= Services.Get<IEventBus>();

        private void Start()
        {
            _tickDispatcher = Services.Get<ITickDispatcher>();
            _warmupState = new WarmupState(this, warmupDuration);
            _matchState = new ConversionMatchState(this, teamSettings, respawnDelay);
            _stateMachine.SetState(_warmupState);
            _tickDispatcher.Subscribe(_stateMachine.Tick);
        }

        private void OnDestroy()
        {
            _tickDispatcher.Unsubscribe(_stateMachine.Tick);
        }

        TeamInfo ITeamRegistry.GetTeam(int index) => teamSettings.Teams[index];

        public void SpawnAllPlayers()
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                int teamIndex = i % teamSettings.Teams.Length;
                IPlayerInputSource inputSource = i == 0 ? uiControls : new SimpleBot();
                SpawnPlayer(teamIndex, inputSource);
            }
        }

        public void SetCameraTarget(Transform target)
        {
            cinemachineCamera.Target = new CameraTarget
            {
                TrackingTarget = target,
                LookAtTarget = target
            };
        }

        private void SpawnPlayer(int teamIndex, IPlayerInputSource inputSource)
        {
            var spawnTransform = GetSpawnPoint(teamIndex);
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

        public void DespawnAllPlayers()
        {
            for (int i = _players.Count-1; i >= 0; i--)
            {
                Destroy(_players[i].gameObject);
            }

            _players.Clear();
        }

        public void SetMatchState()
        {
            _stateMachine.SetState(_matchState);
        }

        private Transform GetSpawnPoint(int teamIndex)
        {
            var filtered = spawnPoints
                .Where(p => p.TeamIndex == -1 || p.TeamIndex == teamIndex)
                .ToArray();

            return filtered[Random.Range(0, filtered.Length)].transform;
        }
    }
}
