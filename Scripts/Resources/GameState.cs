namespace SpaceGame;

[GlobalClass]
public partial class GameState : Resource
{
    [Export] public OptionsManager OptionsManager;
    [Export] public Player Player;
}
