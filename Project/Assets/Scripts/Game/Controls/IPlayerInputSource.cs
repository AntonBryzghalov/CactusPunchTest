using UnityEngine;

namespace TowerDefence.Game.Controls
{
    public interface IPlayerInputSource
    {
        Vector2 MoveInput { get; }
        bool AttackPressed { get; }
        void EnableInput();
        void DisableInput();
    }
}