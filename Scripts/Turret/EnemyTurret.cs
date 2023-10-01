namespace SpaceGame;

public partial class EnemyTurret : Turret
{
    [Export] GameState gameState;

    public override Vector2 Target { get => gameState.Player.Position; }
}
