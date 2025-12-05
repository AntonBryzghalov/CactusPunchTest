using Cysharp.Threading.Tasks;
using TowerDefence.Core;
using TowerDefence.Game.Round.Rules;
using TowerDefence.UI;
using TowerDefence.UI.CommonScreens;
using UnityEngine;

namespace TowerDefence.Game.Round.States
{
    public class RoundEndState : IState
    {
        private readonly IRoundManager _roundManager;
        private readonly bool _win;
        private readonly float _duration;
        private readonly IScreenRouter _screenRouter;
        private readonly IUIRegistry _uiRegistry;
        private float _timer;
        

        public RoundEndState(IRoundManager roundManager, bool win, float duration)
        {
            _roundManager = roundManager;
            _win = win;
            _duration = duration;
            _screenRouter = Services.Get<IScreenRouter>();
            _uiRegistry = Services.Get<IUIRegistry>();
        }

        public void OnEnter()
        {
            Debug.Log($"{nameof(RoundEndState)} started!");

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
            _roundManager.DespawnAllPlayers();
            _screenRouter.HideModalAsync().AsUniTask().Forget();
            Debug.Log($"{nameof(RoundEndState)} ended!");
        }

        public void Tick(float deltaTime)
        {
            _timer -= deltaTime;

            if (_timer <= 0)
            {
                _roundManager.SetPostRoundState();

                var stateMachine = Services.Get<IStateMachine>();
                stateMachine.SetState(new GameOverState());
            }
        }

        private void ShowWin()
        {
            _roundManager.RealPlayer.SetWinState();
            if (_uiRegistry.TryGetScreen("MessageScreen", out SimpleCaptionScreen screen))
            {
                screen.SetData("You Win!");
                _screenRouter.ShowModalAsync(screen).AsUniTask().Forget();
            }
        }

        private void ShowLose()
        {
            _roundManager.RealPlayer.SetLoseState();
            if (_uiRegistry.TryGetScreen("MessageScreen", out SimpleCaptionScreen screen))
            {
                screen.SetData("You Lose!");
                _screenRouter.ShowModalAsync(screen).AsUniTask().Forget();
            }
        }
    }
}