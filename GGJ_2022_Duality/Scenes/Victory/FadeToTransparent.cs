using Godot;
using System;

public class FadeToTransparent : ColorRect
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    float time;

    public override void _PhysicsProcess(float delta)
    {
        time += delta;
        Modulate = Colors.White.lerp(new Color(1, 1, 1, 0), time);

        if (Inputs.key_enter.OnPress() || Inputs.joy1_start.OnPress())
            Scene.Load("res://Scenes/Level/Level.tscn");
    }
}
