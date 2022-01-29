using Godot;
using System;

public class EnemyDestroyAnimation : Spatial
{
    [Export] public float fps = 12f;
    [Export] public int end_frame = 6;

    public static void Spawn(Vector3 position)
    {
        var anim = GD.Load<PackedScene>("res://Assets/Enemy/EnemyDestroyAnimation.tscn").Instance<EnemyDestroyAnimation>();
        anim.Translation = position;
        Scene.Current.AddChild(anim);
        anim.sprite = anim.FindChild<AnimatedSprite3D>();
    }

    AnimatedSprite3D sprite;
    float time;
    public override void _PhysicsProcess(float delta)
    {
        time += delta;
        var frame = (int) (time * fps);
        sprite.Frame = frame;        
        if (frame > end_frame)
            QueueFree();
        
    }
}
