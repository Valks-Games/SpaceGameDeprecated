namespace SpaceGame;

[Tool]
public partial class Shield : Sprite2D
{
    [Export] float radius
    {
        get 
        {
            CircleShape2D circle = GetCircle();

            if (circle != null)
                return circle.Radius;

            return 0;
        }
        set
        {
            CircleShape2D circle = GetCircle();

            if (circle != null)
                circle.Radius = Math.Abs(value);

            ShaderMaterial material = Material as ShaderMaterial;

            if (circle != null && material != null)
            {
                double shaderRadius = circle.Radius.Remap(16, 72, 0f, 0.375f);
                material.SetShaderParameter("size", shaderRadius);
            }
        }
    }

    Area2D area;

    public override void _Ready()
    {
        if (!Engine.IsEditorHint())
        {
            area = GetNode<Area2D>("Area2D");
        }
    }

    public void Deactivate()
    {
        Hide();
        area.SetDeferred("monitoring", false);
    }

    public void Activate()
    {
        Show();
        area.SetDeferred("monitoring", true);
    }

    CircleShape2D GetCircle()
    {
        CollisionShape2D collision = 
            GetNodeOrNull<CollisionShape2D>("Area2D/CollisionShape2D");

        if (collision == null)
            return null;

        CircleShape2D circle = collision.Shape as CircleShape2D;
        return circle;
    }
}
