using TowerDefence.Game.Units;

namespace TowerDefence.Game.AI
{
    public class BotFactory
    {
        private readonly BotStatesFactory _botStatesFactory;

        public BotFactory(BotStatesFactory botStatesFactory)
        {
            _botStatesFactory = botStatesFactory;
        }

        public IBot CreateBot(Player player)
        {
            return new SimpleBot(player, _botStatesFactory);
        }
    }
}
