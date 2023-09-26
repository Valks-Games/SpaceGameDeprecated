namespace SpaceGame;

public partial class Player : RigidBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        AddConstantCentralForce(Vector2.Up);
    }
}
