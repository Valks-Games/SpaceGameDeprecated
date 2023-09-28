namespace SpaceGame;

public partial class EnemyShip : Ship
{
    RigidBody2D player;

    public override void _Ready()
	{
        base._Ready();
        player = GetTree().GetFirstNodeInGroup("Player") as RigidBody2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        //Position += (player.Position - Position) / 50;
        //ApplyCentralImpulse(Vector2.Up * thrustAcceleration);

        LookAt(player.Position);
        ApplyCentralImpulse(Vector2.Right.Rotated(Rotation) * thrustAcceleration);
    }
}
