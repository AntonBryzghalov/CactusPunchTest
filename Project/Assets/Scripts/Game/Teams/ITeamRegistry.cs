using TowerDefence.Game.Teams;

namespace TowerDefence.Game.Rules.ConversionClash
{
    public interface ITeamRegistry
    {
        TeamInfo GetTeam(int index);
    }
}