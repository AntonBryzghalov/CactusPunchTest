using System;
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
    // TODO: extract Unit base class
    public class Player : MonoBehaviour, ITickable, IDisposable
    {
        [SerializeField] private Transform rotationRoot;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private HealthComponent health;
        [SerializeField] private TeamComponent team;
        [SerializeField] private PlayerUiView uiView;

        private IPlayerInputSource _inputSource;
        private bool _inputEnabled;
        private Weapon _weapon;
        private GameObject _raceModel;

        public PlayerMovement Movement => movement;
        public HealthComponent Health => health;
        public TeamComponent Team => team;
        public RaceInfo Race { get; private set; }
        public Weapon Weapon => _weapon;

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

        public void Dispose()
        {
            if (gameObject) Destroy(gameObject);
        }

        public void Tick(float deltaTime)
        {
            if (health.IsDead) return;

            UpdateInput();
            _weapon?.AttackTrigger.Tick(deltaTime);
        }

        private void UpdateInput()
        {
            if (!_inputEnabled || _inputSource == null) return;

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

        public void SetInputEnabled(bool enabled)
        {
            _inputEnabled = enabled;
        }

        public void SetPrepareState()
        {
            // TODO: trigger some warming up animation
        }

        public void SetReadyState()
        {
            // TODO: prepare for ingame
        }

        // In this example we don't use that state, player continue from where he is, just with another team
        public void SetDeadState()
        {
            _weapon.AttackTrigger.Reset();
            // TODO: trigger death animation
        }

        public void SetWinState()
        {
            _weapon.AttackTrigger.Reset();
            // TODO: trigger some "win" animation
        }

        public void SetLoseState()
        {
            _weapon.AttackTrigger.Reset();
            // TODO: trigger some "lose" animation
        }
    }
}
