using Godot;
using System;

public class BulletHitSound : AudioStreamPlayer
{
    static BulletHitSound instance;
    public override void _Ready()
    {
        instance = this;
    }

    public static void PlaySound()
    {
        instance.Play();
    }
}
