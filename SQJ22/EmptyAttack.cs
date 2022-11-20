using ExplogineCore.Data;
using ExplogineMonoGame;

namespace SQJ22;

public class EmptyAttack : EnemyMove.IAttack
{
    public bool Execute()
    {
        return true;
    }

    public string Description()
    {
        return "";
    }

    public void DrawPreview(Painter painter, RenderSettings settings, Depth depth)
    {
    }
}
