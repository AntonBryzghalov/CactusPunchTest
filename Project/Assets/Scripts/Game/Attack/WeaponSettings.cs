using TowerDefence.Infrastructure.Randomization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Game.Attack
{
    [CreateAssetMenu(fileName = "AttackSettings", menuName = "My Awesome Game/Attack Settings")]
    public class WeaponSettings : ScriptableObject, IRandomPrefab<Weapon>
    {
        [SerializeField] private Weapon[] weaponPrefabs;

        public Weapon[] WeaponPrefabs => weaponPrefabs;

        public Weapon GetRandomPrefab() => weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
    }
}
