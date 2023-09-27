namespace SpaceGame;

public partial class EnemyShip : RigidBody2D
{
    [Export] float thrustAcceleration = 2.0f;
    [Export] float rotateAcceleration = 0.1f;

    RigidBody2D player;
    List<GpuParticles2D> engineParticles = new();

    public override void _Ready()
	{
        player = GetTree().GetFirstNodeInGroup("Player") as RigidBody2D;

        foreach (GpuParticles2D particles in GetNode("Engine Particles").GetChildren())
        {
            particles.Emitting = true;
            engineParticles.Add(particles);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        //Position += (player.Position - Position) / 50;
        //ApplyCentralImpulse(Vector2.Up * thrustAcceleration);

        LookAt(player.Position);
        ApplyCentralImpulse(Vector2.Right.Rotated(Rotation) * thrustAcceleration);
    }
}
