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

        private void Awake()
        {
            team.OnTeamChanged += OnTeamChanged;
            health.ResetHealth();
            health.SetOwner(this);
            uiView.BindHealthComponent(health);
        }

        private void OnDestroy()
        {
            team.OnTeamChanged -= OnTeamChanged;
        }

        public void Tick(float deltaTime)
        {
            UpdateInput();
            _weapon?.AttackTrigger.Tick(deltaTime);
        }

        private void UpdateInput()
        {
            if (_inputSource == null) return;

            movement.SetInput(_inputSource.MoveInput);
            _weapon.AttackTrigger.SetAttackMode(_inputSource.AttackPressed);
        }

        private void OnTeamChanged(TeamInfo teamInfo)
        {
            uiView.SetTeamColor(teamInfo.Color);
        }

#region Configuration

        public void SetInputSource(IPlayerInputSource inputSource)
        {
            _inputSource = inputSource;
        }

        public void SetRace(RaceInfo race)
        {
            Race = race;
            _raceModel = Instantiate(race.Prefab, rotationRoot, false);
            movement.Initialize(_raceModel.transform);
            uiView.SetRaceSprite(Race.Icon);
        }

        public void SetWeapon(Weapon weapon)
        {
            _weapon = weapon;
            _weapon.SetOwner(this);
            _weapon.transform.SetParent(_raceModel.transform, false);
        }

#endregion

        public void SetWinState()
        {
            _inputSource.DisableInput();
            _weapon.AttackTrigger.Reset();
            // TODO: Add some "win" animation
        }

        public void SetLoseState()
        {
            _inputSource.DisableInput();
            _weapon.AttackTrigger.Reset();
            // TODO: Add some "lose" animation
        }

        public void SetPrepareState()
        {
            _inputSource.DisableInput();
        }

        public void SetGameplayState()
        {
            _inputSource.EnableInput();
        }
    }
}
