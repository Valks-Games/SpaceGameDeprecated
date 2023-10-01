namespace SpaceGame;

public static class Particles
{
    public static PackedScene ProjectileHit { get; } = Load("Particles/projectile_hit");
    public static PackedScene Explosion { get; } = Load("Particles/explosion");

    static PackedScene Load(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/{path}.tscn");
}

public static class Prefabs
{
    public static PackedScene Shield { get; } = Load("Components/shield");
    public static PackedScene Options { get; } = Load("UI/options");

    static PackedScene Load(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/{path}.tscn");
}
