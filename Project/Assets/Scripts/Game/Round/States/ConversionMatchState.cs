using Cysharp.Threading.Tasks;
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
        private readonly ConversionClashRules _rules;
        private readonly ITeamRegistry _teamRegistry;
        private readonly float _respawnDelay;
        private readonly IEventBus _eventBus;
        private IEventToken _playerKilledToken;

        public ConversionMatchState(ConversionClashRules rules, ITeamRegistry teamRegistry, float respawnDelay)
        {
            _rules = rules;
            _teamRegistry = teamRegistry;
            _respawnDelay = respawnDelay;
            _eventBus = Services.Get<IEventBus>();
        }

        public void OnEnter()
        {
            _eventBus.Subscribe<PlayerKilledEvent>(OnPlayerKilled);
            Debug.Log("Conversion Match started!");
        }

        public void Tick(float deltaTime)
        {
            foreach (var player in _rules.Players)
            {
                player.Tick(deltaTime);
            }
        }

        public void OnExit()
        {
            _eventBus.Unsubscribe(_playerKilledToken);
            Debug.Log("Conversion Match ended!");
        }

        private void OnPlayerKilled(PlayerKilledEvent evt)
        {
            var newTeamIndex = evt.Attacker.Team.TeamIndex;
            CheckForGameOver();
            Respawn(evt.Victim, newTeamIndex).Forget();
        }

        private void CheckForGameOver()
        {
            // TODO: implement
            var isGameOver = false;
            if (isGameOver)
                _rules.SetMatchState();
        }

        private async UniTask Respawn(Player victim, int teamIndex)
        {
            if (_respawnDelay > 0f)
                await UniTask.WaitForSeconds(_respawnDelay);

            victim.Health.ResetHealth();
            victim.Team.SetTeam(_teamRegistry, teamIndex);
        }
    }
}