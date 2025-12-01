using UnityEngine;

namespace TowerDefence.Game.Teams
{
    [CreateAssetMenu(fileName = "TeamSettings", menuName = "My Awesome Game/TeamSettings")]
    public class TeamSettings : ScriptableObject
    {
        [SerializeField] private TeamInfo[] teams;
        public TeamInfo[] Teams => teams;
    }
}
