namespace SpaceGame;

public partial class ShipWeapon : Sprite2D
{
    [Export] float rotationSpeed = 0.05f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 diff = GetGlobalMousePosition() - GlobalPosition;

        float angle = diff.Angle() + Mathf.Pi / 2;

        Rotation = Mathf.LerpAngle(Rotation, angle, rotationSpeed);
    }
}
