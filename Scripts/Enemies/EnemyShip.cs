namespace SpaceGame;

public partial class EnemyShip : RigidBody2D
{
    [Export] float thrustAcceleration = 2.0f;
    [Export] float rotateAcceleration = 0.1f;

    RigidBody2D player;
    List<GpuParticles2D> engineParticles = new();

    bool _isChasePlayer;

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
        if (_isChasePlayer)
            ChasePlayer();
    }

    void ChasePlayer()
    {
        LookAt(player.Position);
        ApplyCentralImpulse(Vector2.Right.Rotated(Rotation) * thrustAcceleration);
    }

    void OnDetectionArea(bool detected)
    {
        if (detected)
            _isChasePlayer = true;
        else
            _isChasePlayer = false;
    }
}
