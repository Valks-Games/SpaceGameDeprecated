using Godot;
using System;

public partial class enemy_ship : RigidBody2D
{
    [Export] float thrustAcceleration = 2.0f;
    [Export] float rotateAcceleration = 0.1f;

    List<GpuParticles2D> engineParticles = new();

    public override void _Ready()
	{
        foreach (GpuParticles2D particles in GetNode("Engine Particles").GetChildren())
        {
            particles.Emitting = false;
            engineParticles.Add(particles);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        RigidBody2D player = GetTree().GetFirstNodeInGroup("Player") as RigidBody2D;

        //Position += (player.Position - Position) / 50;
        //ApplyCentralImpulse(Vector2.Up * thrustAcceleration);
        engineParticles.ForEach(x =>
        {
            x.Lifetime = 1f;
            x.Emitting = true;
        });

        LookAt(player.Position);
        ApplyCentralImpulse(Vector2.Right.Rotated(Rotation) * thrustAcceleration);
    }
}
