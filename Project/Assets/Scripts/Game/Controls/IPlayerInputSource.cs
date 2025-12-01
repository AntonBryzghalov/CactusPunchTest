using UnityEngine;

namespace TowerDefence.Game.Controls
{
    public interface IPlayerInputSource
    {
        Vector2 MoveDirection { get; }
        bool AttackPressed { get; }
        bool AttackReleased { get; }
    }
}