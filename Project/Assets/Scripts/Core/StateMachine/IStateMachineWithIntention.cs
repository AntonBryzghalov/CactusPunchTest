using System;

namespace TowerDefence.Core
{
    public interface IStateMachineWithIntention<TIntention> where TIntention : struct, Enum
    {
        IStateWithIntention<TIntention> CurrentState { get; }
        void SetState(IStateWithIntention<TIntention> newState);
        void Tick(float deltaTime);
    }
}
