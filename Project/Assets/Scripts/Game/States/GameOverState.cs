using TowerDefence.Core;
using TowerDefence.Game.Events;
using TowerDefence.Systems;
using TowerDefence.UI;
using TowerDefence.UI.Gameplay;
using UnityEngine;

namespace TowerDefence.Game
{
    public class GameOverState : IState
    {
        private readonly IEventBus _eventBus;
        private IEventToken _returnToMenuToken;
        private IEventToken _restartRoundToken;

        public GameOverState()
        {
            _eventBus = Services.Get<IEventBus>();
        }

        public async void OnEnter()
        {
            _returnToMenuToken = _eventBus.Subscribe<ReturnToMenuRequestedEvent>(OnReturnToMenu);
            _restartRoundToken = _eventBus.Subscribe<RestartRoundEvent>(OnRestartRound);
            var uiRegistry = Services.Get<IUIRegistry>();
            if (uiRegistry.TryGetScreen<GameOverScreen>("GameOverScreen", out var gameOverScreen))
            {
                var screenRouter = Services.Get<IScreenRouter>();
                screenRouter.Clear();
                await screenRouter.PushAsync(gameOverScreen);
            }
        }

        public void OnExit()
        {
            if (_eventBus == null) return;
            if (_returnToMenuToken != null) _eventBus.Unsubscribe(_returnToMenuToken);
            if (_restartRoundToken != null) _eventBus.Unsubscribe(_restartRoundToken);
        }

        public void Tick(float deltaTime) { }

        private async void OnReturnToMenu(ReturnToMenuRequestedEvent _)
        {
            Time.timeScale = 1f;

            var sceneLoader = Services.Get<ISceneLoader>();
            var config = Services.Get<GameConfig>();
            await sceneLoader.LoadSceneAsync(config.MenuSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);

            var stateMachine = Services.Get<IStateMachine>();
            stateMachine.SetState(new MenuState());
        }

        private void OnRestartRound(RestartRoundEvent _)
        {
            IStateMachine stateMachine = Services.Get<IStateMachine>();
            stateMachine.SetState(new GameplayState());
        }
    }
}
