namespace SpaceGame;

public partial class PlayerTurret : Turret
{
    public override Vector2 Target { get => GetGlobalMousePosition(); }
}
