using System;

namespace TowerDefence.Core
{
    public class StateMachineWithIntention<TIntention> : IStateMachineWithIntention<TIntention>
        where TIntention : struct, Enum
    {
        public IStateWithIntention<TIntention> CurrentState { get; private set; }

        public void SetState(IStateWithIntention<TIntention> newState)
        {
            if (CurrentState == newState)
            {
                return;
            }

            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState?.OnEnter();
        }

        public void Tick(float deltaTime)
        {
            CurrentState?.Tick(deltaTime);
        }
    }
}
