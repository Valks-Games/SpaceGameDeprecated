namespace SpaceGame;

[Tool]
public partial class Shield : Sprite2D
{
    public override void _Ready()
    {
        // Make all sub-resources of Shield unique
        Material = (ShaderMaterial)Material.Duplicate(subresources: true);
        CollisionShape2D collision =
            GetNodeOrNull<CollisionShape2D>("Area2D/CollisionShape2D");

        if (collision == null)
            return;

        collision.Shape = (CircleShape2D)collision.Shape.Duplicate();
    }

    [Export] float Radius
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
