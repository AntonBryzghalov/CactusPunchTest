using System;
using UnityEngine;

namespace TowerDefence.Game.Settings
{
    [CreateAssetMenu(fileName = "RaceSettings", menuName = "My Awesome Game/RaceSettings")]
    public class RaceSettings : ScriptableObject
    {
        [SerializeField] private RaceInfo[] races;
        public RaceInfo[] Races => races;
    }

    [Serializable]
    public sealed class RaceInfo
    {
        [SerializeField] private string name;
        [SerializeField] private Sprite icon;

        public string Name => name;
        public Sprite Icon => icon;
    }
}
