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

namespace TowerDefence.Game.Units.Player
{
    // TODO: extract Unit base class
    public class PlayerComponent : MonoBehaviour, ITickable, IDisposable
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
        private PlayerState _state = PlayerState.Preparing;

        public PlayerMovement Movement => movement;
        public HealthComponent Health => health;
        public TeamComponent Team => team;
        public RaceInfo Race { get; private set; }
        public BotAttackHints WeaponAttackHints => _weapon.BotAttackHints;
        public PlayerState State => _state;

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
            if (_state == PlayerState.Dead) return;

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
            Race = race ?? throw new ArgumentNullException(nameof(race));
            
            var newRaceModel = Instantiate(race.Prefab, rotationRoot, false);
            if (_raceModel != null)
            {
                if (_weapon != null) _weapon.transform.SetParent(newRaceModel.transform, false);
                Destroy(_raceModel.gameObject);
            }
            _raceModel = newRaceModel;
            movement.Initialize(_raceModel.transform);
            uiView.SetRaceSprite(Race.Icon);
        }

        public void SetWeapon(Weapon weapon)
        {
            if (weapon == null) throw new ArgumentNullException(nameof(weapon));
            if (_weapon != null) Destroy(_weapon.gameObject);

            _weapon = weapon;
            _weapon.SetOwner(this);
            _weapon.transform.SetParent(_raceModel?.transform ?? this.transform, false);
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
            _state = PlayerState.Active;
            // TODO: prepare for ingame
        }

        // In this example we don't use that state, player continue from where he is, just with another team
        public void SetDeadState()
        {
            _state = PlayerState.Dead;
            _weapon.AttackTrigger.Reset();
            // TODO: trigger death animation
        }

        public void SetWinState()
        {
            _state = PlayerState.Won;
            _weapon.AttackTrigger.Reset();
            // TODO: trigger some "win" animation
        }

        public void SetLoseState()
        {
            _state = PlayerState.Lost;
            _weapon.AttackTrigger.Reset();
            // TODO: trigger some "lose" animation
        }
    }
}
