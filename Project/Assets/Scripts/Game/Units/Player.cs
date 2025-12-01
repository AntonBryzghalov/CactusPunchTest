using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Health;
using TowerDefence.Game.Movement;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Game.Units
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private HealthComponent health;
        [SerializeField] private TeamComponent team;
        [SerializeField] private PlayerUiView uiView;
        [SerializeField] private RaceSettings raceSettings;
        [SerializeField] private AutomaticAttackTrigger attackTrigger;
        [SerializeField] private BaseAttack attack;

        private IPlayerInputSource _inputSource;

        public PlayerMovement Movement => movement;
        public HealthComponent Health => health;
        public TeamComponent Team => team;
        public RaceInfo Race { get; private set; }

        public void SetInputSource(IPlayerInputSource inputSource)
        {
            _inputSource = inputSource;
        }

        private void Start()
        {
            Race = raceSettings.Races[Random.Range(0, raceSettings.Races.Length)];
            uiView.Initialize(health, Race.Icon);
        }

        private void Update()
        {
            UpdateInput();
            UpdateAttack();
        }

        private void UpdateInput()
        {
            if (_inputSource == null) return;

            movement.SetInput(_inputSource.MoveInput);
            if (_inputSource.AttackPressed) attackTrigger.SetAttackMode(true);
            if (_inputSource.AttackReleased) attackTrigger.SetAttackMode(false);
        }

        private void UpdateAttack()
        {
            if (attackTrigger.CanAttack)
            {
                attack.PerformAttack();
                attackTrigger.OnAttackPerformed();
            }
        }
    }
}
