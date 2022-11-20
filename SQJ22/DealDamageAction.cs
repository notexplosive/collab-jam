using System;
using ExTween;

namespace SQJ22;

public class DealDamageAction : ITokenAction
{
    public ITween Execute(GridSpace space, EntityData data)
    {
        return new SequenceTween();
    }
}
