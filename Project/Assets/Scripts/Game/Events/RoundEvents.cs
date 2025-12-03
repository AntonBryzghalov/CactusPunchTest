using TowerDefence.Game.Teams;

namespace TowerDefence.Game.Events
{
    public struct RoundStartedEvent
    {
    }

    /// <summary>
    /// Fired when we have a winner
    /// </summary>
    public struct RoundFinishedEvent
    {
        public TeamInfo WinningTeam;
    }

    /// <summary>
    /// Fired when we stop showing the winner and move to the post game menu
    /// </summary>
    public struct EnterPostRoundEvent
    {
    }

    public struct RestartRoundEvent
    {
    }
}
