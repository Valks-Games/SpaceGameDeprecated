namespace SpaceGame;

public partial class AIShip : Ship
{
    [Export] int pursuePlayerTime = 5000;

    GTimer pursuePlayer;
    State curState;
    RigidBody2D player;

    public override void _Ready()
	{
        base._Ready();

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
}
