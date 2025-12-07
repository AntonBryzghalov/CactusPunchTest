using TowerDefence.Game.Round.States;

namespace TowerDefence.Game.Round.Rules
{
    public class ConversionClashRoundManager : BaseRoundManager
    {
        private IRoundState _matchState;

        protected override IRoundState GetMatchState()
        {
            return _matchState ??= new ConversionMatchState(this, teamSettings);
        }

        protected override IRoundState GetRoundResultsState(RoundResults results)
        {
            var isPlayerWon = results.playerWinStates.TryGetValue(RealPlayer, out var playerWon) && playerWon;
            return new ConversionRoundResultsState(isPlayerWon, waitForGameOverDuration);
        }
    }
}
