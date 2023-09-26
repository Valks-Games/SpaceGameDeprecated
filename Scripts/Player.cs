namespace SpaceGame;

public partial class Player : RigidBody2D
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
        if (Input.IsActionPressed("move_left"))
        {
            AngularVelocity -= rotateAcceleration;
        }

        if (Input.IsActionPressed("move_right"))
        {
            AngularVelocity += rotateAcceleration;
        }

        if (Input.IsActionPressed("move_up"))
        {
            ApplyCentralImpulse(Vector2.Up.Rotated(Rotation) * thrustAcceleration);
        }

        if (Input.IsActionJustPressed("move_up"))
        {
            engineParticles.ForEach(x =>
            {
                x.Lifetime = 1f;
                x.Emitting = true;
            });
        }

        if (Input.IsActionJustReleased("move_up"))
        {
            engineParticles.ForEach(x =>
            {
                x.Lifetime = 0.5f;
                x.Emitting = false;
            });
        }
    }
}
