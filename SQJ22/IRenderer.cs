using ExplogineCore.Data;
using ExplogineMonoGame;

namespace SQJ22;

public interface IRenderer
{
    void DrawEntity(Painter painter, RenderSettings renderSettings, Entity entity, Depth depth);
}