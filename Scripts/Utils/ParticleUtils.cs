namespace SpaceGame;

public partial class ParticleUtils : Node
{
    static ParticleUtils instance;

    public override void _Ready()
    {
        instance = this;
    }

    public static void Spawn(PackedScene prefab, Vector2 pos, double lifeTime)
    {
        GpuParticles2D particles = prefab.Instantiate<GpuParticles2D>();
        instance.GetTree().Root.AddChild(particles);
        particles.Position = pos;
        particles.Emitting = true;
        particles.CreateTween().TweenCallback(Callable.From(() =>
        {
            particles.QueueFree();
        })).SetDelay(lifeTime);
    }
}
