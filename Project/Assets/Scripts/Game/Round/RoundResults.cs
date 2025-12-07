using System.Collections.Generic;
using TowerDefence.Game.Units.Player;

namespace TowerDefence.Game.Round
{
    public class RoundResults
    {
        public bool IsTeamMode;
        public int WinnerTeamIndex = -1;
        public readonly Dictionary<PlayerComponent, bool> playerWinStates = new Dictionary<PlayerComponent, bool>();
    }
}
