using Godot;
using System;

public class EnemyDestroyedSounds : AudioStreamPlayer
{
    static EnemyDestroyedSounds instance = default;

    public override void _Ready()
    {
        instance = this;
    }

    public static void PlaySound()
    {
        instance.Play();
    }
}
