using System;
using TowerDefence.Core;
using TowerDefence.Game.AI.Navigation;
using TowerDefence.Game.AI.States;
using TowerDefence.Game.Attack;
using TowerDefence.Game.Controls;
using TowerDefence.Game.Units;
using UnityEngine;

namespace TowerDefence.Game.AI
{
    public class BotStatesFactory
    {
        private readonly BotsSettings settings;
        private readonly IPlayerRegistry _playerRegistry;
        private readonly IWaypointGenerator _waypointGenerator;

        public BotStatesFactory(BotsSettings settings, IPlayerRegistry playerRegistry, IWaypointGenerator waypointGenerator)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _playerRegistry = playerRegistry ?? throw new ArgumentNullException(nameof(playerRegistry));
            _waypointGenerator = waypointGenerator ?? throw new ArgumentNullException(nameof(waypointGenerator));
        }

        public IBotState CreateIdleState(BufferPlayerInputSource botInputSource, Player botPlayer)
        {
            var durationRange = settings.IdleStateDurationRange;
            var duration = Mathf.Approximately(durationRange.x, durationRange.y)
                ? durationRange.x
                : UnityEngine.Random.Range(durationRange.x, durationRange.y);
            return new IdleState(botInputSource, botPlayer, _playerRegistry, duration, settings.VisionRange);
        }

        public IBotState CreateDeadState(BufferPlayerInputSource botInputSource, Player botPlayer)
        {
            return new DeadState(botInputSource, botPlayer);
        }

        public IBotState CreateSearchForTargetState(
            BufferPlayerInputSource botInputSource,
            Player botPlayer)
        {
            return new SearchForTargetState(
                botInputSource,
                botPlayer,
                _playerRegistry,
                _waypointGenerator,
                settings.WaypointDistanceThreshold,
                settings.VisionRange);
        }

        public IBotState CreateMoveToTargetState(
            BufferPlayerInputSource botInputSource,
            Player botPlayer,
            Player target,
            BotAttackHints attackHints)
        {
            return new MoveToTargetState(botInputSource, botPlayer, target, attackHints, settings.VisionRange);
        }

        public IBotState CreateAttackTargetState(
            BufferPlayerInputSource botInputSource,
            Player botPlayer,
            Player target)
        {
            return new AttackTargetState(botInputSource, botPlayer, target);
        }
    }
}