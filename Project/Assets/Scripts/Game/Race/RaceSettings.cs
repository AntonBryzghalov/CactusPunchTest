using System;
using TowerDefence.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Game.Settings
{
    [CreateAssetMenu(fileName = "RaceSettings", menuName = "My Awesome Game/Race Settings")]
    public class RaceSettings : ScriptableObject, IRandom<RaceInfo>
    {
        [SerializeField] private RaceInfo[] races;

        public RaceInfo[] Races => races;

        public RaceInfo GetRandom() => races[Random.Range(0, races.Length)];
    }

    [Serializable]
    public sealed class RaceInfo
    {
        [SerializeField] private string name;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject prefab;

        public string Name => name;
        public Sprite Icon => icon;
        public GameObject Prefab => prefab;
    }
}
