using System;

namespace TowerDefence.Core
{
    public interface IStateWithIntention<TIntention> : IState where TIntention : struct, Enum
    {
        TIntention Intention { get; }
        object Payload { get; }
    }
}