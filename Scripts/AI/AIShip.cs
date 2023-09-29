namespace SpaceGame;

[Tool]
public partial class AIShip : Ship
{
    GTimer pursuePlayer;
    State curState;
    RigidBody2D player;

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

        player = GetTree().GetFirstNodeInGroup("Player") as RigidBody2D;

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