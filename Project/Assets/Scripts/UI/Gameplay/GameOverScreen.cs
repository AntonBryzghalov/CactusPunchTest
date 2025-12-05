using TowerDefence.Core;
using TowerDefence.Game;
using TowerDefence.Game.Events;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UI.Gameplay
{
    public class GameOverScreen : BaseScreen
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backToMenuButton;

        private IEventBus _eventBus;

        protected override void Awake()
        {
            base.Awake();
            _eventBus = Services.Get<IEventBus>();
            _restartButton.onClick.AddListener(OnRestartClick);
            _backToMenuButton.onClick.AddListener(OnBackToMenuClick);
        }

        protected override  void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestartClick);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuClick);
            base.OnDestroy();
        }

        private void OnRestartClick()
        {
            _eventBus.Publish(new RestartRoundEvent());
        }

        private void OnBackToMenuClick()
        {
            _eventBus.Publish(new ReturnToMenuRequestedEvent());
        }
    }
}
