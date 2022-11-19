using System.Collections.Generic;
using ExplogineCore;
using ExplogineMonoGame;
using ExplogineMonoGame.Cartridges;
using Microsoft.Xna.Framework;

namespace SQJ22;

public class SqGameCartridge : BasicGameCartridge
{
    public override CartridgeConfig CartridgeConfig { get; } = new(new Point(1920, 1080));
    public override void AddCommandLineParameters(CommandLineParametersWriter parameters)
    {
    }

    public override IEnumerable<ILoadEvent> LoadEvents(Painter painter)
    {
        yield return null;
    }

    public override void OnCartridgeStarted()
    {
    }

    public override void Update(float dt)
    {
    }

    public override void Draw(Painter painter)
    {
    }
}