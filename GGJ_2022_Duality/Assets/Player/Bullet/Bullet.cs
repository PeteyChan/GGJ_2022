using Godot;
using System;
using System.Collections.Generic;

public class Bullet : Area
{
    public static Bullet Spawn(Node source, Vector3 position, Vector3 direction, float speed, float lifespan = 2f)
    {
        Bullet bullet = null;
        while (bullets.Count > 0)
        {
            bullet = bullets.Pop();
            if (bullet.IsValid()) break;
        }
        if (!bullet.IsValid())
        {
            bullet = GD.Load<PackedScene>("res://Assets/Player/Bullet/bullet.tscn").Instance<Bullet>();
        }

        bullet.Translation = position;
        bullet.RotationDegrees = Vector3.Zero;
        bullet.velocity = direction.Normalized() * speed;
        bullet.time_alive = 0;
        bullet.lifespan = lifespan;
        bullet.source = source;
        bullet.destroy = false;
        Scene.Current.AddChild(bullet);
        return bullet;
    }

    static Stack<Bullet> bullets = new Stack<Bullet>();

    [Export]
    public float lifespan = 2f;
    float time_alive;
    public Node source;
    [Export] Vector3 velocity = new Vector3(0, 0, 10);
    Sprite3D sprite;

    public void Destroy()
    {
        destroy = true;
    }

    bool destroy;

    public void SetColor(Color color)
    {
        if (!sprite.IsValid())
            sprite = this.FindChild<Sprite3D>();
        sprite.Modulate = color;
    }

    public override void _PhysicsProcess(float delta)
    {
        time_alive += delta;
        Translation = Translation + velocity * delta;

        if (destroy || time_alive > lifespan)
        {
            BulletHitEffect.Spawn(this.GlobalTransform.origin);
            GetParent().RemoveChild(this);
            bullets.Push(this);
            SetColor(Colors.White);
        }
    }
}
