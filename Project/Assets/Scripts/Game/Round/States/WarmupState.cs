using Cysharp.Threading.Tasks;
using TowerDefence.Core;
using TowerDefence.Game.Round.Rules;
using TowerDefence.UI;
using TowerDefence.UI.CommonScreens;
using UnityEngine;

namespace TowerDefence.Game.Round.States
{
    public class WarmupState : IState
    {
        private readonly IRoundManager _roundManager; // TODO: replace with common interface
        private readonly float _warmupDuration;
        private readonly IScreenRouter _screenRouter;
        private readonly IUIRegistry _uiRegistry;
        private float _timer;

        public WarmupState(IRoundManager roundManager, float warmupDuration)
        {
            _roundManager = roundManager;
            _warmupDuration = warmupDuration;
            _screenRouter = Services.Get<IScreenRouter>();
            _uiRegistry = Services.Get<IUIRegistry>();
        }

        public void OnEnter()
        {
            _timer = _warmupDuration;
            _roundManager.SpawnAllPlayers();
            foreach (var player in _roundManager.Players)
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
                _roundManager.SetMatchState();
            }
        }
    }
}