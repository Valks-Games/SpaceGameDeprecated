namespace SpaceGame;

public partial class EnemyShip : Ship
{
    State curState;
    RigidBody2D player;

    public override void _Ready()
	{
        base._Ready();
        player = GetTree().GetFirstNodeInGroup("Player") as RigidBody2D;

        Area2D detectionArea = GetNode<Area2D>("Detection");

        detectionArea.BodyEntered += body =>
        {
            if (body is not Player player)
                return;

            SwitchState(Attack());
        };

        detectionArea.BodyExited += body =>
        {
            if (body is not Player player)
                return;

            SwitchState(Idle());
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

    State Idle()
    {
        State state = new State(nameof(Idle));

        state.Enter = () =>
        {
            SetEngineParticlesEmitting(false, lifeTime: 0.5);
        };

        return state;
    }

    State Attack()
    {
        State state = new State(nameof(Attack));

        state.Enter = () =>
        {
            SetEngineParticlesEmitting(true, lifeTime: 1.0);
        };

        state.Update = () =>
        {
            //Position += (player.Position - Position) / 50;
            //ApplyCentralImpulse(Vector2.Up * thrustAcceleration);

            LookAt(player.Position);
            ApplyCentralImpulse(Vector2.Right.Rotated(Rotation) * thrustAcceleration);
        };

        return state;
    }
}
