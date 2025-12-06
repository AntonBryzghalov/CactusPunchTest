using System;
using UnityEngine;

namespace TowerDefence.Game.Attack
{
    [Serializable]
    public struct BotAttackHints
    {
        public Vector2 desiredAttackRange;
        public float maxHitAngle; // TODO: angle over distance
        public bool requireDirectVision;
    }
}