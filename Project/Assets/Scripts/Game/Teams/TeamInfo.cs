using System;
using UnityEngine;

namespace TowerDefence.Game.Teams
{
    [Serializable]
    public sealed class TeamInfo
    {
        [SerializeField] private string name;
        [SerializeField] private Color color;

        public string Name => name;
        public Color Color => color;
    }
}