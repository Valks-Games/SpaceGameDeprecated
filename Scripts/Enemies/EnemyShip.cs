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
            // Slowly rotate towards target
            // To refactor this to be a constant rotation speed watch the
            // following YT video: https://www.youtube.com/watch?v=ciT_jDol9G8
            const float ROTATION_SPEED = 0.05f;

            Vector2 diff = player.Position - Position;

            float angle = diff.Angle();

            Rotation = Mathf.LerpAngle(Rotation, angle, ROTATION_SPEED);

            // Apply forward thrust
            ApplyCentralImpulse(Vector2.Right.Rotated(Rotation) * thrustAcceleration);
        };

        return state;
    }
}
