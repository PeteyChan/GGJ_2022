using Godot;
using System;

public class EnemyShoot : AudioStreamPlayer
{
    static EnemyShoot instance;
    public override void _Ready()
    {
        instance = this;        
    }

    public static void PlaySound()
    {
        instance.Play();
    }
}
