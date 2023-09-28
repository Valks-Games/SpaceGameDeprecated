namespace SpaceGame;

public partial class Projectile : Sprite2D
{
    [Export] public int Damage = 1;
    [Export] double lifeTime = 5.0; // in seconds
    [Export] float speed = 10;

    public ulong OwnerId { get; set; }

    public override void _Ready()
    {
        CreateTween().TweenCallback(Callable.From(() => {
            QueueFree();
        })).SetDelay(lifeTime);

        GetNode<Area2D>("Area2D").BodyEntered += body =>
        {
            if (OwnerId != body.GetInstanceId() && body is Ship ship)
            {
                ship.HullDamage(Damage);
                QueueFree();
            }
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Up.Rotated(Rotation) * speed;
    }
}
