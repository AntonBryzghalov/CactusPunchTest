using TowerDefence.Game.Units.Player;

namespace TowerDefence.Game.AI
{
    public class BotFactory
    {
        private readonly BotStatesFactory _botStatesFactory;

        public BotFactory(BotStatesFactory botStatesFactory)
        {
            _botStatesFactory = botStatesFactory;
        }

        public IBot CreateBot(PlayerComponent player)
        {
            return new SimpleBot(player, _botStatesFactory);
        }
    }
}
