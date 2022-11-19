using System.Collections.Generic;
using ExplogineMonoGame;
using ExTween;

namespace SQJ22;

public class Animation
{
    private readonly IBacker _backer = new InstantBacker();

    public Animation(bool isInstant)
    {
        if (!isInstant)
        {
            _backer = new TweenBacker();
        }
    }

    public void Enqueue(ITween tween)
    {
        _backer.Enqueue(tween);
    }

    public void Update(float dt)
    {
        _backer.Update(dt);
    }

    public bool IsPlaying()
    {
        return _backer.IsPlaying();
    }

    private interface IBacker
    {
        public void Enqueue(ITween tween);
        public void Update(float dt);
        bool IsPlaying();
    }

    private class InstantBacker : IBacker
    {
        private readonly Queue<ITween> _queue = new();

        public void Enqueue(ITween tween)
        {
            _queue.Enqueue(tween);
        }

        public void Update(float dt)
        {
            foreach (var item in _queue)
            {
                item.SkipToEnd();
            }
        }

        public bool IsPlaying()
        {
            return false;
        }
    }

    private class TweenBacker : IBacker
    {
        private readonly SequenceTween _sequence = new();

        public void Enqueue(ITween tween)
        {
            _sequence.Add(tween);
        }

        public void Update(float dt)
        {
            _sequence.Update(dt);

            if (_sequence.IsDone())
            {
                if (_sequence.ChildrenWithDurationCount > 0)
                {
                    // Client.Debug.Log("~~ Animation complete ~~");
                }

                _sequence.Clear();
            }
        }

        public bool IsPlaying()
        {
            return !_sequence.IsDone();
        }
    }
}
