namespace SpaceGame;

public partial class PlayerTurret : Sprite2D
{
    [Export] float rotationSpeed = 0.05f;

    public float TurretRotation { get; private set; }
    public List<Marker2D> Barrels { get; } = new();

    public override void _Ready()
    {
        foreach (Marker2D marker in GetChildren())
            Barrels.Add(marker);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 diff = GetGlobalMousePosition() - GlobalPosition;

        float shipRot = GetOwner<Node2D>().Rotation;
        float offset = Mathf.Pi / 2;
        TurretRotation = diff.Angle() + offset - shipRot;

        Rotation = Mathf.LerpAngle(Rotation, TurretRotation, rotationSpeed);
    }
}
