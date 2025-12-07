using System;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Rules.ConversionClash;
using TowerDefence.Game.Settings;
using UnityEngine;

namespace TowerDefence.Game.Units.Player
{
    /// <summary>
    /// This class ensures the correct order of Player initialization (components injection)
    /// </summary>
    public class PlayerBuilder
    {
        private readonly PlayerComponent _playerPrefab;
        private PlayerComponent _player;
        private RaceInfo _race;
        private IPlayerInputSource _inputSource;
        private Weapon _weapon;
        private ITeamRegistry _teamRegistry;
        private int _teamIndex;
        private Vector3 _position;
        private Quaternion _rotation;

        public PlayerBuilder(PlayerComponent playerPrefab)
        {
            if (playerPrefab == null) throw new ArgumentNullException(nameof(playerPrefab));
            _playerPrefab = playerPrefab;
        }

        public PlayerBuilder CreateNewPlayer()
        {
            if (_player != null)
            {
                throw new InvalidOperationException("Previous Player is not built yet");
            }

            _player = UnityEngine.Object.Instantiate(_playerPrefab);
            return this;
        }

        public PlayerBuilder WithRace(RaceInfo race)
        {
            _race = race;
            return this;
        }

        public PlayerBuilder WithWeapon(Weapon weapon)
        {
            _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            return this;
        }

        public PlayerBuilder WithTeam(ITeamRegistry teamRegistry, int teamIndex)
        {
            _teamRegistry = teamRegistry ?? throw new ArgumentNullException(nameof(teamRegistry));
            _teamIndex = teamIndex;
            return this;
        }

        public PlayerBuilder InPosition(Vector3 position)
        {
            _position = position;
            return this;
        }

        public PlayerBuilder WithRotation(Quaternion rotation)
        {
            _rotation = rotation;
            return this;
        }

        public PlayerBuilder WithInput(IPlayerInputSource inputSource)
        {
            _inputSource = inputSource ?? throw new ArgumentNullException(nameof(inputSource));
            return this;
        }

        public PlayerComponent Build()
        {
            if (_player == null) CreateNewPlayer();

            _player.SetRace(_race);
            if (_inputSource != null)
            {
                _player.SetInputSource(_inputSource);
            }

            if (_weapon != null) _player.SetWeapon(_weapon);
            if (_teamIndex >= 0) _player.Team.SetTeam(_teamRegistry, _teamIndex);
            _player.Movement.TeleportTo(_position);
            _player.Movement.SetRotation(_rotation);

            var playerBuild = _player;
            Cleanup();
            return playerBuild;
        }

        private void Cleanup()
        {
            _player = null;
            _race = null;
            _inputSource = null;
            _weapon = null;
            _teamRegistry = null;
            _teamIndex = -1;
            _position = Vector3.zero;
            _rotation = Quaternion.identity;
        }
    }
}
