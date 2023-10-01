namespace SpaceGame;

public abstract partial class Ship : RigidBody2D
{
    [Export] int maxShieldHP = 3;
    [Export] int maxHullHP = 3;
    [Export] double fullShieldRegenDelay = 3;
    [Export] double passiveShieldRegenDelay = 2;
    [Export] protected float thrustAcceleration = 100;
    [Export] protected float rotateAcceleration = 0.1f;
    [Export] bool enginesEmittingOnReady;

    public bool ShieldsActive { get; private set; }

    protected List<GpuParticles2D> engineParticles = new();
    protected List<Marker2D> hullTurretMarkers = new();

    ShaderMaterial shieldMaterial;
    Shield shield;
    GTween tweenShield;
    Sprite2D shipSprite;

    double passiveShieldRegenDelayCounter;

    int shieldHP;
    int hullHP;

    public override void _Ready()
    {
        shieldHP = maxShieldHP;
        hullHP = maxHullHP;
        shipSprite = GetNode<Sprite2D>("Ship");

        InitEngineParticles();
        InitHullTurrets();
        InitShield();
    }

    public override void _PhysicsProcess(double delta)
    {
        PassiveShieldRegeneration(delta);
    }

    public void HullDamage(Vector2 projectilePosition, int damage)
    {
        hullHP -= damage;

        ParticleUtils.Spawn(Particles.ProjectileHit, projectilePosition, lifeTime: 5);

        if (hullHP <= 0)
        {
            SpawnExplosionParticles();

            // Destroy ship
            QueueFree();
        }
    }

    protected void SetEngineParticlesEmitting(bool emitting, double lifeTime = -1)
    {
        foreach (GpuParticles2D particle in engineParticles)
        {
            particle.Emitting = emitting;

            if (lifeTime != -1)
                particle.Lifetime = lifeTime;
        }
    }

    void ShieldDamage(int damage)
    {
        shieldHP -= damage;

        if (shieldHP <= 0)
        {
            GpuParticles2D particles = 
                ParticleUtils.Spawn(Particles.ShieldBreak, Position, 0.5f);

            ShieldsActive = false;
            shield.Deactivate();

            CreateTween().TweenCallback(Callable.From(() =>
            {
                ShieldsActive = true;
                shield.Activate();
                shieldHP = maxShieldHP;
            })).SetDelay(fullShieldRegenDelay);
        }
        else
        {
            AnimateShield();
        }
    }

    void PassiveShieldRegeneration(double delta)
    {
        passiveShieldRegenDelayCounter += delta;

        if (passiveShieldRegenDelayCounter >= passiveShieldRegenDelay)
        {
            passiveShieldRegenDelayCounter = 0;

            // Regen shield HP if shield is active and less than max shield HP
            if (ShieldsActive && shieldHP < maxShieldHP)
            {
                shieldHP++;
            }
        }
    }

    void AnimateShield()
    {
        // This must be set to 0.0 (double) before tweening the shader uniform
        // or a Nil float mismatch error and tween failed to run error will be seen
        shieldMaterial.SetShaderParameter("intensity", 0.0);

        tweenShield = new GTween(shield);

        // The full path must be set here in order for this to work
        string shaderUniform = "material:shader_parameter/intensity";

        const double MAX_SHIELD_INTENSITY = 0.6;
        const double MIN_SHIELD_INTENSITY = 0.0;

        tweenShield.Animate(
            prop: shaderUniform,
            finalValue: MAX_SHIELD_INTENSITY,
            duration: 0.1);

        tweenShield.Animate(
            prop: shaderUniform,
            finalValue: MIN_SHIELD_INTENSITY,
            duration: 0.2
            );
    }

    void SpawnExplosionParticles()
    {
        // Particle properties are dynamically calculated based on ship size
        Vector2 size = shipSprite.Texture.GetSize();
        float avgSize = (size.X + size.Y) / 2;

        const float LIFETIME_WEIGHT = 0.15f;
        const float VELOCITY_WEIGHT = 5.0f;
        const float AMOUNT_WEIGHT   = 1.0f;

        int explosionAmount = (int)(avgSize * AMOUNT_WEIGHT);
        float explosionLifeTime = avgSize * LIFETIME_WEIGHT;
        float velocity = avgSize * VELOCITY_WEIGHT;

        // Spawn explosion particles
        GpuParticles2D particles =
                ParticleUtils.Spawn(Particles.Explosion, Position, explosionLifeTime);

        var material = particles.ProcessMaterial as ParticleProcessMaterial;

        material.InitialVelocityMin = velocity;
        material.InitialVelocityMax = velocity + 50;

        particles.Amount = explosionAmount;
    }

    void InitShield()
    {
        shield = GetNodeOrNull<Shield>("Shield");

        if (shield == null)
            return;

        shieldMaterial = shield.Material as ShaderMaterial;

        if (shieldMaterial == null)
        { 
            Logger.LogWarning("A shield sprite was created but it has no shader");    
        }

        Area2D shieldArea = shield.GetNodeOrNull<Area2D>("Area2D");

        if (shieldArea == null)
        {
            Logger.LogWarning("A shield sprite was created but it has no Area2D");
            return;
        }

        ShieldsActive = true;

        shieldArea.AreaEntered += area =>
        {
            if (area.GetParent() is not Projectile projectile || 
                projectile.OwnerId == GetInstanceId())
                return;

            ShieldDamage(projectile.Damage);
            projectile.QueueFree();
        };
    }

    void InitHullTurrets()
    {
        Node turrets = GetNodeOrNull("Hull Turrets");

        if (turrets == null)
            return;

        foreach (Marker2D marker in turrets.GetChildren())
            hullTurretMarkers.Add(marker);
    }

    void InitEngineParticles()
    {
        Node engineParticlesParent = GetNodeOrNull("Engine Particles");

        if (engineParticlesParent == null)
            return;

        foreach (GpuParticles2D particles in engineParticlesParent.GetChildren())
        {
            particles.Emitting = enginesEmittingOnReady;
            engineParticles.Add(particles);
        }
    }
}
