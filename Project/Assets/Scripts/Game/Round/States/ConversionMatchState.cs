using TowerDefence.Core;
using TowerDefence.Game.Events;
using TowerDefence.Game.Round.Rules;
using TowerDefence.Game.Rules.ConversionClash;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Round.States
{
    public class ConversionMatchState : IRoundState
    {
        private readonly IRoundManager _roundManager;
        private readonly ITeamRegistry _teamRegistry;
        private readonly IPlayerRegistry _playerRegistry;
        private readonly IEventBus _eventBus;
        private RoundResults _roundResults;

        public RoundStateType Intention { get; private set; }
        public object Payload { get; private set; }

        public ConversionMatchState(IRoundManager roundManager, ITeamRegistry teamRegistry)
        {
            _roundManager = roundManager;
            _teamRegistry = teamRegistry;
            _playerRegistry = Services.Get<IPlayerRegistry>();
            _eventBus = Services.Get<IEventBus>();
        }

        public void OnEnter()
        {
            _roundResults = new RoundResults();
            foreach (var player in _playerRegistry.Players)
            {
                player.SetReadyState();
                player.Health.OnKilled += OnPlayerKilled;
            }

            Debug.Log("Conversion Match started!");
        }

        public void OnExit()
        {
            foreach (var player in _playerRegistry.Players)
            {
                player.Health.OnKilled -= OnPlayerKilled;
            }

            Debug.Log("Conversion Match ended!");
        }

        public void Tick(float deltaTime)
        {
            // TODO: move to player state (subscribe for update ticks)
            foreach (var player in _playerRegistry.Players)
            {
                player.Tick(deltaTime);
            }
        }

        private void OnPlayerKilled(Player attacker, Player victim)
        {
            var newTeamIndex = attacker.Team.TeamIndex;
            Respawn(victim, newTeamIndex);
            _roundResults.playerWinStates[victim] = false;
            CheckForGameOver(attacker);
        }

        private void CheckForGameOver(Player attacker)
        {
            var players = _playerRegistry.Players;
            var firstTeamIndex = players[0].Team.TeamIndex;
            var allPlayersOfTheSameTeam = true;
            for (int i = 1; i < players.Count; i++)
            {
                if (players[i].Team.TeamIndex != firstTeamIndex)
                {
                    allPlayersOfTheSameTeam = false;
                    break;
                }
            }

            if (allPlayersOfTheSameTeam)
            {
                _roundResults.playerWinStates[attacker] = true;
                Intention = RoundStateType.RoundResults;
                Payload = _roundResults;
            }
        }

        private void Respawn(Player victim, int teamIndex)
        {
            victim.Health.ResetHealth();
            victim.Team.SetTeam(_teamRegistry, teamIndex);
        }
    }
}