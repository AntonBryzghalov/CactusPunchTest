using TowerDefence.ExtensionMethods;
using TowerDefence.Game.Units.Player;
using TowerDefence.Providers;
using UnityEngine;

namespace TowerDefence.Game.AI
{
    public static class AIUtils
    {
        public static PlayerComponent GetClosestEnemyInSight(IPlayerRegistry playerRegistry, PlayerComponent botPlayer,
            IProvider<Vector3> botPosition, float visionRangeSquared)
        {
            var players = playerRegistry.Players;
            float closestDistance = float.MaxValue;
            PlayerComponent result = null;
            foreach (var player in players)
            {
                if (botPlayer == player) continue;
                if (botPlayer.Team.IsSameTeam(player.Team.TeamIndex)) continue;

                var distanceSquared = (player.transform.position.ToVector2XZ() - botPosition.Value.ToVector2XZ()).sqrMagnitude;
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
