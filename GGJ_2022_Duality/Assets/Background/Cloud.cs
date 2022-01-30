using Godot;
using System;

public class Cloud : Spatial
{
    [Export]public float move_speed = 2f;

    public override void _Process(float delta)
    {
        Translate(Vector3.Back * delta * move_speed);
        if (Translation.z > 20)
            Translation = Translation.setZ(-20);
    }
}
