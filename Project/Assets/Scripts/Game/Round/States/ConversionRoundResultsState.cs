using Cysharp.Threading.Tasks;
using TowerDefence.Core;
using TowerDefence.Game.Units.Player;
using TowerDefence.UI;
using TowerDefence.UI.CommonScreens;
using UnityEngine;

namespace TowerDefence.Game.Round.States
{
    public sealed class ConversionRoundResultsState : IRoundState
    {
        private readonly bool _win;
        private readonly float _duration;
        private readonly IScreenRouter _screenRouter;
        private readonly IUIRegistry _uiRegistry;
        private readonly IPlayerRegistry _playerRegistry;
        private float _timer;

        public RoundStateType Intention { get; private set; }
        public object Payload { get; }

        public ConversionRoundResultsState(bool win, float duration)
        {
            _win = win;
            _duration = duration;
            _screenRouter = Services.Get<IScreenRouter>();
            _uiRegistry = Services.Get<IUIRegistry>();
            _playerRegistry = Services.Get<IPlayerRegistry>();
        }

        public void OnEnter()
        {
            Debug.Log($"{nameof(ConversionRoundResultsState)} started!");

            _timer = _duration;
            if (_win)
            {
                ShowWin();
            }
            else
            {
                ShowLose();
            }
        }

        public void OnExit()
        {
            _screenRouter.HideModalAsync().AsUniTask().Forget();
            Debug.Log($"{nameof(ConversionRoundResultsState)} ended!");
        }

        public void Tick(float deltaTime)
        {
            _timer -= deltaTime;

            if (_timer <= 0)
            {
                Intention = RoundStateType.PostRound;
            }
        }

        private void ShowWin()
        {
            _playerRegistry.Players[0].SetWinState();
            if (_uiRegistry.TryGetScreen("MessageScreen", out SimpleCaptionScreen screen))
            {
                screen.SetData("You Win!");
                _screenRouter.ShowModalAsync(screen).AsUniTask().Forget();
            }
        }

        private void ShowLose()
        {
            _playerRegistry.Players[0].SetLoseState();
            if (_uiRegistry.TryGetScreen("MessageScreen", out SimpleCaptionScreen screen))
            {
                screen.SetData("You Lose!");
                _screenRouter.ShowModalAsync(screen).AsUniTask().Forget();
            }
        }
    }
}