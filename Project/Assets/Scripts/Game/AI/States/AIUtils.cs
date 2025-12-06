using TowerDefence.Game.Units;
using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI.States
{
    public static class AIUtils
    {
        public static Player GetClosestEnemyInSight(IPlayerRegistry playerRegistry, Player botPlayer,
            IProvider<Vector3> botPosition, float visionRangeSquared)
        {
            var players = playerRegistry.Players;
            float closestDistance = float.MaxValue;
            Player result = null;
            foreach (var player in players)
            {
                if (botPlayer == player) continue;
                if (botPlayer.Team.IsSameTeam(player.Team.TeamIndex)) continue;

                var distanceSquared = (player.transform.position - botPosition.Value).sqrMagnitude;
                if (distanceSquared > visionRangeSquared) continue;

                if (distanceSquared < closestDistance)
                {
                    closestDistance = distanceSquared;
                    result = player;
                }
            }

            return result;
        }
    }
}
