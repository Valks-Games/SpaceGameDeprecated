namespace SpaceGame;

public partial class AIShip
{
    State Idle()
    {
        State state = new State(nameof(Idle));

        state.Enter = () =>
        {
            SetEngineParticlesEmitting(false, lifeTime: 0.5);
        };

        return state;
    }
}
