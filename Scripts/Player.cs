namespace SpaceGame;

public partial class Player : Ship
{
    [Export] PackedScene greenLaser;

    List<PlayerTurret> rotatingTurrets = new();
    float fireCooldown = 250;
    GTimer hullTurretsCooldown;
    GTimer rotatingTurretsCooldown;

    public override void _Ready()
    {
        base._Ready();
        hullTurretsCooldown = new(this, fireCooldown) { Loop = false };
        rotatingTurretsCooldown = new(this, fireCooldown) { Loop = false };
        InitRotatingTurrets();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("shoot_hull_turrets"))
        {
            ShootHullTurrets();
        }

        if (Input.IsActionPressed("shoot_rotating_turrets"))
        {
            ShootRotatingTurrets();
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
            ApplyCentralForce(Vector2.Up.Rotated(Rotation) * thrustAcceleration);
        }

        if (Input.IsActionJustPressed("move_up"))
        {
            SetEngineParticlesEmitting(true, lifeTime: 1.0);
        }

        if (Input.IsActionJustReleased("move_up"))
        {
            SetEngineParticlesEmitting(false, lifeTime: 0.5);
        }
    }

    void ShootRotatingTurrets()
    {
        if (rotatingTurretsCooldown.IsActive())
            return;

        rotatingTurretsCooldown.Start();

        foreach (PlayerTurret turret in rotatingTurrets)
        {
            foreach (Marker2D barrel in turret.Barrels)
            {
                Projectile laser = greenLaser.Instantiate<Projectile>();
                laser.OwnerId = GetInstanceId();
                laser.Position = barrel.GlobalPosition;
                laser.Rotation = turret.GlobalRotation;
                GetTree().Root.AddChild(laser);
            }
        }
    }

    void ShootHullTurrets()
    {
        if (hullTurretsCooldown.IsActive())
            return;

        hullTurretsCooldown.Start();

        foreach (Marker2D marker in hullTurretMarkers)
        {
            Projectile laser = greenLaser.Instantiate<Projectile>();
            laser.OwnerId = GetInstanceId();
            laser.Position = marker.GlobalPosition;
            laser.Rotation = Rotation;
            GetTree().Root.AddChild(laser);
        }
    }

    void InitRotatingTurrets()
    {
        Node turrets = GetNodeOrNull("Rotating Turrets");

        if (turrets == null)
            return;

        foreach (PlayerTurret turret in turrets.GetChildren())
            rotatingTurrets.Add(turret);
    }
}
