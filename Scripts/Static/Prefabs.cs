namespace SpaceGame;

public static class Particles
{
    public static PackedScene ProjectileHit { get; } = Load("projectile_hit");
    public static PackedScene Explosion { get; } = Load("explosion");
    public static PackedScene ShieldBreak { get; } = Load("shield_break");

    static PackedScene Load(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/Particles/{path}.tscn");
}

public static class Prefabs
{
    public static PackedScene Shield { get; } = Load("Components/shield");
    public static PackedScene Options { get; } = Load("UI/options");

    static PackedScene Load(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/{path}.tscn");
}
