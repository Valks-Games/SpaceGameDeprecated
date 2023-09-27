namespace SpaceGame;

public partial class Player : RigidBody2D
{
	[Export] float thrustAcceleration = 2.0f;
	[Export] float rotateAcceleration = 0.1f;
	[Export] PackedScene greenLaser;

	List<GpuParticles2D> engineParticles = new();
	List<Marker2D> gunFirePositions = new();
	float fireCooldown = 250;
	GTimer timerCooldown;

	public override void _Ready()
	{
		timerCooldown = new(this, fireCooldown) { Loop = false };

		foreach (GpuParticles2D particles in GetNode("Engine Particles").GetChildren())
		{
			particles.Emitting = false;
			engineParticles.Add(particles);
		}

		foreach (Marker2D marker in GetNode("Guns").GetChildren())
		{ 
			gunFirePositions.Add(marker);    
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("jump"))
		{
			Shoot();
		}

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

	void Shoot()
	{
		if (timerCooldown.IsActive())
			return;

		timerCooldown.Start();

		foreach (Marker2D marker in gunFirePositions)
		{
			Projectile laser = greenLaser.Instantiate<Projectile>();
			laser.Position = marker.GlobalPosition;
			laser.Rotation = Rotation;
			GetTree().Root.AddChild(laser);
		}
	}
}
