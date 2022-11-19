using ExplogineCore.Data;
using ExplogineMonoGame;

namespace SQJ22;

public interface IRenderer
{
    void Draw(Painter painter, RenderSettings renderSettings, Entity entity, Depth depth);
}