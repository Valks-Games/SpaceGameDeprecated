namespace SpaceGame;

public abstract partial class Turret : Sprite2D
{
    [Export] protected float rotationSpeed = 0.05f;

    public List<Marker2D> Barrels { get; } = new();

    public override void _Ready()
    {
        foreach (Marker2D marker in GetChildren())
            Barrels.Add(marker);
    }

    protected void RotateTowardsTarget(Vector2 target)
    {
        Vector2 diff = target - GlobalPosition;

        float shipRot = GetOwner<Node2D>().Rotation;
        float offset = Mathf.Pi / 2;
        float turretRot = diff.Angle() + offset - shipRot;

        Rotation = Mathf.LerpAngle(Rotation, turretRot, rotationSpeed);
    }
}
