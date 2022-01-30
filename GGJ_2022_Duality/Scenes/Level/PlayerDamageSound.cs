using Godot;
using System;

public class PlayerDamageSound : AudioStreamPlayer
{
    static PlayerDamageSound instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        instance = this;
    }

    public static void PlaySound()
    {
        instance.PitchScale = Rand.FloatRange(0.8f, 1.2f);
        instance.Play();
    }
}
