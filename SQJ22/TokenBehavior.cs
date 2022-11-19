using System.Collections.Generic;
using ExTween;

namespace SQJ22;

public class TokenBehavior
{
    public TokenBehaviorEvent Tapped { get; } = new();
    public TokenBehaviorEvent Nudged { get; } = new();
    public TokenBehaviorEvent Blocked { get; } = new();
    public TokenBehaviorEvent Moved { get; } = new();
    public TokenBehaviorEvent AdjacentTapped { get; } = new();
    public TokenBehaviorEvent Destroyed { get; } = new();

    public TokenBehavior OnTapped(params ITokenAction[] actions)
    {
        return AddActionsToEvent(Tapped, actions);
    }
    
    public TokenBehavior OnNudged(params ITokenAction[] actions)
    {
        return AddActionsToEvent(Nudged, actions);
    }
    
    public TokenBehavior OnBlocked(params ITokenAction[] actions)
    {
        return AddActionsToEvent(Blocked, actions);
    }
    
    public TokenBehavior OnMoved(params ITokenAction[] actions)
    {
        return AddActionsToEvent(Moved, actions);
    }
    
    public TokenBehavior OnAdjacentTapped(params ITokenAction[] actions)
    {
        return AddActionsToEvent(AdjacentTapped, actions);
    }
    
    public TokenBehavior OnDestroyed(params ITokenAction[] actions)
    {
        return AddActionsToEvent(Destroyed, actions);
    }
    
    private TokenBehavior AddActionsToEvent(TokenBehaviorEvent tokenEvent, ITokenAction[] actions)
    {
        foreach (var action in actions)
        {
            tokenEvent.Add(action);
        }

        return this;
    }

    public class TokenBehaviorEvent : ITokenAction
    {
        private readonly List<ITokenAction> _actions = new();

        public ITween Execute(Entity entity)
        {
            var result = new SequenceTween();
            foreach (var action in _actions)
            {
                result.Add(action.Execute(entity));
            }

            return result;
        }

        public void Add(ITokenAction action)
        {
            _actions.Add(action);
        }
    }
}
