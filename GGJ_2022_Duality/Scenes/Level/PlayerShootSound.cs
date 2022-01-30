using Godot;
using System;

public class PlayerShootSound : AudioStreamPlayer
{
    static PlayerShootSound instance;

    public override void _EnterTree()
    {
        instance = this;
    }

    public static void PlaySound()
    {
        instance.PitchScale = Rand.FloatRange(0.8f, 1.2f);
        instance.Play();
    }

}
