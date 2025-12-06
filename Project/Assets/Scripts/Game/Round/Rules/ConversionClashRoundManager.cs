using TowerDefence.Core;
using TowerDefence.Game.Round.States;

namespace TowerDefence.Game.Round.Rules
{
    public class ConversionClashRoundManager : BaseRoundManager
    {
        private IState _matchState;

        public override void SetRoundEndState(RoundResults results)
        {
            var isPlayerWon = results.playerWinStates.TryGetValue(RealPlayer, out var playerWon) && playerWon;
            _stateMachine.SetState(new RoundEndState(this, isPlayerWon, waitForGameOverDuration));
        }

        protected override IState GetMatchState()
        {
            return _matchState ??= new ConversionMatchState(this, teamSettings, _playerRegistry);
        }
    }
}
