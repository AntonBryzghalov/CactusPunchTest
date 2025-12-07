using Cysharp.Threading.Tasks;
using TowerDefence.Core;
using TowerDefence.Game.Round.Rules;
using TowerDefence.Game.Units.Player;
using TowerDefence.UI;
using TowerDefence.UI.CommonScreens;
using UnityEngine;

namespace TowerDefence.Game.Round.States
{
    public class WarmupState : IRoundState
    {
        private readonly IRoundManager _roundManager;
        private readonly IPlayerRegistry _playerRegistry;
        private readonly float _warmupDuration;
        private readonly IScreenRouter _screenRouter;
        private readonly IUIRegistry _uiRegistry;
        private float _timer;

        public RoundStateType Intention { get; private set; }
        public object Payload { get; }

        public WarmupState(IRoundManager roundManager, float warmupDuration)
        {
            _roundManager = roundManager;
            _warmupDuration = warmupDuration;
            _playerRegistry = Services.Get<IPlayerRegistry>();
            _screenRouter = Services.Get<IScreenRouter>();
            _uiRegistry = Services.Get<IUIRegistry>();
        }

        public void OnEnter()
        {
            _timer = _warmupDuration;
            foreach (var player in _playerRegistry.Players)
            {
                player.SetPrepareState();
            }

            _roundManager.SetCameraTarget(_roundManager.RealPlayer.transform);

            if (_uiRegistry.TryGetScreen("MessageScreen", out SimpleCaptionScreen screen))
            {
                screen.SetData("Get Ready!");
                _screenRouter.ShowModalAsync(screen).AsUniTask().Forget();
            }
            Debug.Log("Warmup started!");
        }

        public void OnExit()
        {
            _screenRouter.HideModalAsync().AsUniTask().Forget();
            Debug.Log("Warmup ended!");
        }

        public void Tick(float deltaTime)
        {
            _timer -= deltaTime;

            if (_timer <= 0)
            {
                Intention = RoundStateType.Match;
            }
        }
    }
}