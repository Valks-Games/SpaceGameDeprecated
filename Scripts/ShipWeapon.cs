namespace SpaceGame;

public partial class ShipWeapon : Sprite2D
{
    [Export] float rotationSpeed = 0.05f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 diff = GetGlobalMousePosition() - GlobalPosition;

        float shipRot = GetOwner<Node2D>().Rotation;
        float offset = Mathf.Pi / 2;
        float angle = diff.Angle() + offset - shipRot;

        Rotation = Mathf.LerpAngle(Rotation, angle, rotationSpeed);
    }
}
