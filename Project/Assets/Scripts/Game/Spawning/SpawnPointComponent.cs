using UnityEngine;

namespace TowerDefence.Game.Spawning
{
    // TODO: improve, probably make a visitor pattern to set position/rotation to visitors
    public class SpawnPointComponent : MonoBehaviour
    {
        [SerializeField] private int teamIndex = -1;
        public int TeamIndex => teamIndex;
    }
}
