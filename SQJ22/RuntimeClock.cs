namespace SQJ22;

public class RuntimeClock
{
    public void Update(float dt)
    {
        ElapsedTime += dt;
    }

    public float ElapsedTime { get; private set; }
}
