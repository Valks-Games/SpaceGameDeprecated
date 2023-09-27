namespace SpaceGame;

public partial class Projectile : Sprite2D
{
    public override void _Ready()
    {
        GetTree().CreateTimer(5.0).Timeout += () => QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Up.Rotated(Rotation) * 10;
    }
}
