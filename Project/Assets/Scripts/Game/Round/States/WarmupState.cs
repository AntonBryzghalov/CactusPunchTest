using TowerDefence.Core;
using TowerDefence.Game.Round.Rules;
using UnityEngine;

namespace TowerDefence.Game.Round.States
{
    public class WarmupState : IState
    {
        private readonly ConversionClashRules _rules; // TODO: extract common interface for common states
        private readonly float _warmupDuration;
        private float _timer;

        public WarmupState(ConversionClashRules rules, float warmupDuration)
        {
            _rules = rules;
            _warmupDuration = warmupDuration;
        }

        public void OnEnter()
        {
            _timer = _warmupDuration;
            _rules.SpawnAllPlayers();
            _rules.SetCameraTarget(_rules.RealPlayer.transform);
            Debug.Log("Warmup started!");
        }

        public void Tick(float deltaTime)
        {
            _timer -= deltaTime;

            if (_timer <= 0)
                _rules.SetMatchState();
        }

        public void OnExit()
        {
            Debug.Log("Warmup ended!");
        }
    }
}