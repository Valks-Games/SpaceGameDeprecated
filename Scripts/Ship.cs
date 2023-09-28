namespace SpaceGame;

public abstract partial class Ship : RigidBody2D
{
    [Export] int maxShieldHP = 3;
    [Export] int maxHullHP = 3;
    [Export] double shieldRegenDelay = 3;
    [Export] protected float thrustAcceleration = 2.0f;
    [Export] protected float rotateAcceleration = 0.1f;
    [Export] bool enginesEmittingOnReady;

    protected List<GpuParticles2D> engineParticles = new();
    protected List<Marker2D> gunFirePositions = new();

    ShaderMaterial shieldMaterial;
    Shield shield;
    GTween tweenShield;

    int shieldHP;
    int hullHP;

    public override void _Ready()
    {
        shieldHP = maxShieldHP;
        hullHP = maxHullHP;

        InitEngineParticles();
        InitGuns();
        InitShield();
    }

    public void HullDamage(int damage)
    {
        hullHP -= damage;

        ParticleUtils.Spawn(Prefabs.ProjectileHit, Position, lifeTime: 3);

        if (hullHP <= 0)
        {
            // Spawn explosion particles
            ParticleUtils.Spawn(Prefabs.Explosion, Position, lifeTime: 3);

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
            shield.Deactivate();

            CreateTween().TweenCallback(Callable.From(() =>
            {
                shield.Activate();
                shieldHP = maxShieldHP;
            })).SetDelay(shieldRegenDelay);
        }
        else
        {
            AnimateShield();
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

        shieldArea.AreaEntered += area =>
        {
            if (area.GetParent() is not Projectile projectile || 
                projectile.OwnerId == GetInstanceId())
                return;

            ShieldDamage(projectile.Damage);
            projectile.QueueFree();
        };
    }

    void InitGuns()
    {
        Node gunsParent = GetNodeOrNull("Guns");

        if (gunsParent == null)
            return;

        foreach (Marker2D marker in gunsParent.GetChildren())
        {
            gunFirePositions.Add(marker);
        }
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
