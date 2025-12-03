using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerDefence.Core;
using TowerDefence.Game.AI;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Events;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Spawning;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Units;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Game.Rules.ConversionClash
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
        [SerializeField] private float respawnDelay = 1f;

        private readonly List<Player> _players = new List<Player>();
        private PlayerBuilder _playerBuilder;

        private readonly StateMachine _stateMachine = new StateMachine();
        private IState _warmupState;

        private IEventBus _eventBus;
        private IEventToken _playerKilledToken;

        private PlayerBuilder PlayerBuilder => _playerBuilder ??= new PlayerBuilder(playerPrefab);
        private IEventBus EventBus => _eventBus ??= Services.Get<IEventBus>();

        private void Start()
        {
            _playerKilledToken = EventBus.Subscribe<PlayerKilledEvent>(OnPlayerKilled);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(_playerKilledToken);
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

        public IState CreateMatchState()
        {
            throw new NotImplementedException("TODO: implement CreateMatchState");
        }

        private Transform GetSpawnPoint(int teamIndex)
        {
            var filtered = spawnPoints
                .Where(p => p.TeamIndex == -1 || p.TeamIndex == teamIndex)
                .ToArray();

            return filtered[Random.Range(0, filtered.Length)].transform;
        }

        private void OnPlayerKilled(PlayerKilledEvent evt)
        {
            var newTeamIndex = evt.Attacker.Team.TeamIndex;
            StartCoroutine(Respawn(evt.Victim, newTeamIndex));
        }

        private IEnumerator Respawn(Player victim, int teamIndex)
        {
            if (respawnDelay > 0f)
                yield return new WaitForSeconds(respawnDelay);

            victim.Health.ResetHealth();
            victim.Team.SetTeam(teamSettings, teamIndex);
        }
    }
}
