namespace SpaceGame;

public partial class Player : RigidBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        // TODO: Actual movement
        AddConstantCentralForce(Vector2.Up);
    }
}
