using TowerDefence.Core;
using TowerDefence.Game.Round.States;

namespace TowerDefence.Game.Round.Rules
{
    public class ConversionClashRoundManager : BaseRoundManager
    {
        private IState _matchState;

        protected override IState GetMatchState()
        {
            return _matchState ??= new ConversionMatchState(this, teamSettings);
        }
    }
}
