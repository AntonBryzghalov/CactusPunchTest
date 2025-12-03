using TowerDefence.Core;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Health;
using TowerDefence.Game.Movement;
using TowerDefence.Game.Settings;
using TowerDefence.Game.Teams;
using TowerDefence.Game.Views;
using UnityEngine;

namespace TowerDefence.Game.Units
{
    public class Player : MonoBehaviour, ITickable
    {
        [SerializeField] private Transform rotationRoot;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private HealthComponent health;
        [SerializeField] private TeamComponent team;
        [SerializeField] private PlayerUiView uiView;

        private IPlayerInputSource _inputSource;
        private Weapon _weapon;
        private GameObject _raceModel;

        public PlayerMovement Movement => movement;
        public HealthComponent Health => health;
        public TeamComponent Team => team;
        public RaceInfo Race { get; private set; }

        public void Tick(float deltaTime)
        {
            UpdateInput();
            _weapon?.AttackTrigger.Tick(deltaTime);
        }

        private void UpdateInput()
        {
            if (_inputSource == null) return;

            movement.SetInput(_inputSource.MoveInput);
            if (_inputSource.AttackPressed) _weapon.AttackTrigger.SetAttackMode(true);
            if (_inputSource.AttackReleased) _weapon.AttackTrigger.SetAttackMode(false);
        }

#region Configuration

        public void SetInputSource(IPlayerInputSource inputSource)
        {
            _inputSource = inputSource;
        }

        public void SetRace(RaceInfo race)
        {
            if (_raceModel != null) Destroy(_raceModel);

            Race = race;
            _raceModel = Instantiate(race.Prefab, rotationRoot, false);
            movement.Initialize(_raceModel.transform);
            uiView.Initialize(health, Race.Icon);
        }

        public void SetWeapon(Weapon weapon)
        {
            _weapon = weapon;
            _weapon.SetOwner(this);
            _weapon.AttackTransform?.SetParent(rotationRoot, false);
        }

#endregion
    }
}
