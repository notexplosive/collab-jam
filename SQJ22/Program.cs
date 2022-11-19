using ExplogineDesktop;
using ExplogineMonoGame;
using Microsoft.Xna.Framework;
using SQJ22;

var config = new WindowConfigWritable
{
    WindowSize = new Point(1600, 900),
    Title = "NotExplosive.net"
};
Bootstrap.Run(args, new WindowConfig(config), new SqGameCartridge());