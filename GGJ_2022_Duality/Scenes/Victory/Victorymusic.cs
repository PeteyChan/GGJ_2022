using Godot;
using System;

public class Victorymusic : AudioStreamPlayer
{
    public override void _Process(float delta)
    {
        if (!Playing)
            Play();

    }
}
