namespace SpaceGame;

public partial class AIShip
{
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

            float angle = diff.Angle() + Mathf.Pi / 2;

            Rotation = Mathf.LerpAngle(Rotation, angle, ROTATION_SPEED);

            // Apply forward thrust
            ApplyCentralForce(Vector2.Up.Rotated(Rotation) * thrustAcceleration);
        };

        return state;
    }
}
