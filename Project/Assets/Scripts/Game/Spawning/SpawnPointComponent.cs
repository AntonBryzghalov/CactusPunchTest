using UnityEngine;

namespace TowerDefence.Game.Spawning
{
    // TODO: improve
    // 1) probably make a visitor pattern to set position/rotation to visitors
    // 2) make a respawn cooldown or a mechanism to avoid collisions
    public class SpawnPointComponent : MonoBehaviour
    {
        [SerializeField] private int teamIndex = -1;
        public int TeamIndex => teamIndex;
    }
}
