using System;
using System.Collections.Generic;
using TowerDefence.Core;
using TowerDefence.Game.Events;
using TowerDefence.Helpers;
using UnityEngine;

namespace TowerDefence.Game.AI
{
    public class AIManager : IAIManager
    {
        private readonly List<IBot> _bots = new();
        private readonly PooledBufferList<IBot> _buffer = new();
        private IEventToken _roundStartToken;
        private IEventToken _roundEndToken;
        private bool _isBufferCollectionDirty;
        private bool _subscribedForTick;

        public void Init()
        {
            if (Services.TryGet<IEventBus>(out var eventBus))
            {
                _roundStartToken = eventBus.Subscribe<MatchStartEvent>(OnMatchStart);
                _roundStartToken = eventBus.Subscribe<MatchEndEvent>(OnMatchEnd);
            }
        }

        public void Dispose()
        {
            if (Services.TryGet<IEventBus>(out var eventBus))
            {
                if (_roundStartToken != null) eventBus.Unsubscribe(_roundStartToken);
                if (_roundEndToken != null) eventBus.Unsubscribe(_roundEndToken);
            }
        }

        public void RegisterBot(IBot bot)
        {
            if (_bots.Contains(bot))
            {
                Debug.LogWarning($"Bot already registered in {nameof(AIManager)}");
                return;
            }

            _bots.Add(bot);
            _isBufferCollectionDirty = true;
        }

        public void UnregisterBot(IBot bot)
        {
            if (_bots.Remove(bot))
            {
                _isBufferCollectionDirty = true;
            }
        }

        public void DisposeAllBots()
        {
            foreach (var bot in _bots)
            {
                if (bot is IDisposable disposableBot)
                {
                    disposableBot.Dispose();
                }
            }

            _bots.Clear();
            _isBufferCollectionDirty = true;
        }

        private void OnMatchStart(MatchStartEvent _)
        {
            if (_subscribedForTick) throw new InvalidOperationException($"{nameof(AIManager)} is subscribed to tick already");

            var tickDispatcher = Services.Get<ITickDispatcher>();
            tickDispatcher.Subscribe(Tick);
            _subscribedForTick = true;
            foreach (var bot in _bots)
                bot.OnMatchStart();
        }

        private void OnMatchEnd(MatchEndEvent _)
        {
            var tickDispatcher = Services.Get<ITickDispatcher>();
            tickDispatcher.Unsubscribe(Tick);
            _subscribedForTick = false;
            foreach (var bot in _bots)
                bot.OnMatchEnd();
        }

        public void Tick(float deltaTime)
        {
            if (_isBufferCollectionDirty)
            {
                _buffer.CopyFrom(_bots);
            }

            for (int i = 0; i < _buffer.Count; i++)
            {
                _buffer[i].Tick(deltaTime);
            }
        }
    }
}
