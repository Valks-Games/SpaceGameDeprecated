namespace SpaceGame;

public partial class AIShip
{
    [Export]
    float shieldRadius
    {
        get
        {
            Shield shield = GetNodeOrNull<Shield>("Shield");
            if (shield != null)
            {
                return shield.Radius;
            }

            return 0;
        }
        set
        {
            Shield shield = GetNodeOrNull<Shield>("Shield");
            if (shield != null)
            {
                shield.Radius = value;
            }
        }
    }

    [Export] float detectionRadius
    {
        get
        {
            CircleShape2D circle = GetCircle();

            if (circle != null)
                return circle.Radius;

            return 0;
        }
        set
        {
            CircleShape2D circle = GetCircle();

            if (circle != null)
                circle.Radius = value;
        }
    }

    [Export] int pursuePlayerTime = 5000;

    CircleShape2D GetCircle()
    {
        Area2D detection = GetNodeOrNull<Area2D>("Detection");

        if (detection != null)
        {
            CollisionShape2D collision =
                detection.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

            if (collision != null)
            {
                CircleShape2D circle = collision.Shape as CircleShape2D;
                return circle;
            }
        }

        return null;
    }
}
