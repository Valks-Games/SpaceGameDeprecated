namespace SpaceGame;

public partial class DetectionArea : Node2D
{
    [Signal] public delegate void DetectedEventHandler(bool detected);

    void OnDetectionAreaEntered(RigidBody2D body)
    {
        if (body.IsInGroup("Player"))
        {
            EmitSignal(SignalName.Detected, true);
            GD.Print("Detected " + body.Name);
        }    
    }

    void OnDetectionAreaExited(RigidBody2D body)
    {
        if (body.IsInGroup("Player"))
        {
            EmitSignal(SignalName.Detected, false);
            GD.Print(body.Name + " Loss");
        }
    }
}
