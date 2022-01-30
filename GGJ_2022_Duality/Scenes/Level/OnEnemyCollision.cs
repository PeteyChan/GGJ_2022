using Godot;
using System;

public class OnEnemyCollision : AudioStreamPlayer
{
    static OnEnemyCollision instance;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        instance = this;        
    }

    public static void PlaySound()
    {
        instance.Play();
    }
}
