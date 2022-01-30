using Godot;
using System;
using System.Collections.Generic;

public class BulletHitEffect : Spatial
{
    static Stack<BulletHitEffect> effects = new Stack<BulletHitEffect>();

    public static BulletHitEffect Spawn(Vector3 position)
    {
        BulletHitEffect effect = null;
        while (effects.Count > 0)
        {
            effect = effects.Pop();
            if(effect.IsValid()) break;
        }
        effect = GD.Load<PackedScene>("res://Assets/Player/Bullet/bullet_hit_effect.tscn").Instance<BulletHitEffect>();
        effect.Translation = position;
        effect.time = 0;
        Scene.Current.AddChild(effect);
        BulletHitSound.PlaySound();
        return effect;
    }

    public override void _Ready()
    {
        sprite = this.FindChild<AnimatedSprite3D>();
        time= 0;
    }
    AnimatedSprite3D sprite;
    [Export] public float frames_per_second = 12;
    [Export] public int end_frame = 5;

    float time;
    public override void _PhysicsProcess(float delta)
    {
        time += delta;
        int frame = (int)(frames_per_second * time);
        if (frame > end_frame)
        {
            GetParent().RemoveChild(this);
            effects.Push(this);
        }
        sprite.Frame = frame;
    }
}
