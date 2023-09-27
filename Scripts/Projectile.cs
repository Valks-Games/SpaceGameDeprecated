namespace SpaceGame;

public partial class Projectile : Sprite2D
{
    public ulong OwnerId { get; set; }

    public override void _Ready()
    {
        CreateTween().TweenCallback(Callable.From(() => {
            QueueFree();
        })).SetDelay(5.0);

        GetNode<Area2D>("Area2D").BodyEntered += body =>
        {
            if (body.GetInstanceId() == OwnerId)
                return;

            Sprite2D shield = body.GetNode<Sprite2D>("Shield");

            if (shield != null)
            {
                ShaderMaterial material = shield.Material as ShaderMaterial;
                material.SetShaderParameter("intensity", 0.0);

                string shaderUniform = "material:shader_parameter/intensity";

                Tween tween = body.CreateTween();
                
                tween.TweenProperty(
                    shield,
                    shaderUniform, 
                    0.6, 
                    duration: 0.1);

                tween.TweenProperty(
                    shield,
                    shaderUniform,
                    0.0,
                    duration: 0.2
                    );
            }

            QueueFree();
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Up.Rotated(Rotation) * 10;
    }
}
