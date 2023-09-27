namespace SpaceGame;

public partial class Projectile : Sprite2D
{
    [Export] double lifeTime = 5.0; // in seconds
    [Export] float speed = 10;

    public ulong OwnerId { get; set; }

    public override void _Ready()
    {
        CreateTween().TweenCallback(Callable.From(() => {
            QueueFree();
        })).SetDelay(lifeTime);
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Up.Rotated(Rotation) * speed;
    }
}
