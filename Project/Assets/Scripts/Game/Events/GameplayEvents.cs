using TowerDefence.Game.Units;

namespace TowerDefence.Game.Events
{
    public struct PlayerKilledEvent
    {
        public Player Attacker;
        public Player Victim;
    }
}
