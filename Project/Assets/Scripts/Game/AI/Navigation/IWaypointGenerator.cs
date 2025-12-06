using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI.Navigation
{
    public interface IWaypointGenerator
    {
        IProvider<Vector3> GetNextWaypoint();
    }
}