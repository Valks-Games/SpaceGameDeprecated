namespace SpaceGame;

public partial class AIShip : Ship
{
    [Export] GameState gameState;

    GTimer pursuePlayer;
    State curState;

    public override void _Ready()
    {
        // In-Game
        if (!Engine.IsEditorHint())
        {
            base._Ready();
            Init();
        }
        // In-Editor
        else
        {
            SetPhysicsProcess(false);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        curState.Update();
    }

    void SwitchState(State newState)
    {
        curState.Exit();
        curState = newState;
        curState.Enter();
    }

    void Init()
    {
        pursuePlayer = new(this, pursuePlayerTime);
        pursuePlayer.Finished += () =>
        {
            SwitchState(Idle());
        };

        Area2D detectionArea = GetNode<Area2D>("Detection");

        detectionArea.BodyEntered += body =>
        {
            if (body is not Player player)
                return;

            pursuePlayer.Stop();

            SwitchState(Attack());
        };

        detectionArea.BodyExited += body =>
        {
            if (body is not Player player)
                return;

            pursuePlayer.Start();
        };

        curState = Idle();
        curState.Enter();
    }
}
