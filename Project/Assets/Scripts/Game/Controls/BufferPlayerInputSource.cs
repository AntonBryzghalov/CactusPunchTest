using UnityEngine;

namespace TowerDefence.Game.Controls
{
    public class BufferPlayerInputSource : IPlayerInputSource
    {
        public Vector2 MoveInput { get; set; }
        public bool AttackPressed { get; set; }
    }
}
