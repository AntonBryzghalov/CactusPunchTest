using System.Collections.Generic;
using TowerDefence.Game.Units;

namespace TowerDefence.Game.Round
{
    public class RoundResults
    {
        public bool IsTeamMode;
        public int WinnerTeamIndex = -1;
        public readonly Dictionary<Player, bool> playerWinStates = new Dictionary<Player, bool>();
    }
}
