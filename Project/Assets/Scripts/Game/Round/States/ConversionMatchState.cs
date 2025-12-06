using TowerDefence.Core;
using TowerDefence.Game.Events;
using TowerDefence.Game.Round.Rules;
using TowerDefence.Game.Rules.ConversionClash;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.Round.States
{
    public class ConversionMatchState : IState
    {
        private readonly IRoundManager _roundManager;
        private readonly ITeamRegistry _teamRegistry;
        private readonly IPlayerRegistry _playerRegistry;
        private readonly IEventBus _eventBus;
        private RoundResults _roundResults;
        private IEventToken _playerKilledToken;

        public ConversionMatchState(IRoundManager roundManager, ITeamRegistry teamRegistry, IPlayerRegistry playerRegistry)
        {
            _roundManager = roundManager;
            _teamRegistry = teamRegistry;
            _playerRegistry = playerRegistry;
            _eventBus = Services.Get<IEventBus>();
        }

        public void OnEnter()
        {
            _roundResults = new RoundResults();

            _roundManager.SetPlayerInputActive(true);
            _playerKilledToken = _eventBus.Subscribe<PlayerKilledEvent>(OnPlayerKilled);
            _eventBus.Publish(new RoundStartEvent());
            foreach (var player in _playerRegistry.Players)
            {
                player.SetReadyState();
            }

            Debug.Log("Conversion Match started!");
        }

        public void OnExit()
        {
            _roundManager.SetPlayerInputActive(false);
            _eventBus.Publish(new RoundEndEvent());
            _eventBus.Unsubscribe(_playerKilledToken);
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

        private void OnPlayerKilled(PlayerKilledEvent evt)
        {
            var newTeamIndex = evt.Attacker.Team.TeamIndex;
            Respawn(evt.Victim, newTeamIndex);
            _roundResults.playerWinStates[evt.Victim] = false;
            CheckForGameOver(evt.Attacker);
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
                _roundManager.SetRoundEndState(_roundResults);
            }
        }

        private void Respawn(Player victim, int teamIndex)
        {
            victim.Health.ResetHealth();
            victim.Team.SetTeam(_teamRegistry, teamIndex);
        }
    }
}