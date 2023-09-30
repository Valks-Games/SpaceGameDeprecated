using Godot;
using System;

public partial class ShipWeapon : Sprite2D
{
    // sets rotation of sprite up instead of right
    float rotation = 90;

    public override void _PhysicsProcess(double delta)
    {
        LookAt(GetGlobalMousePosition());
        Rotation += (rotation * (float)delta);
    }
}
