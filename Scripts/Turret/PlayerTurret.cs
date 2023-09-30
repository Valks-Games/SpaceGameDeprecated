namespace SpaceGame;

public partial class PlayerTurret : Turret
{
    public override void _PhysicsProcess(double delta)
    {
        RotateTowardsTarget(GetGlobalMousePosition());
    }
}
