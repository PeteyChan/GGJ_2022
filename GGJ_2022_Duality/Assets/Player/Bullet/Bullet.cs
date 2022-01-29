using Godot;
using System;
using System.Collections.Generic;

public class Bullet : Area
{
    public static void Spawn(Vector3 position, Vector3 direction, float speed)
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
        bullet.velocity = direction.Normalized() * speed;
        bullet.time_alive = 0;
        Scene.Current.AddChild(bullet);
    }

    static Stack<Bullet> bullets = new Stack<Bullet>();
    
    [Export]
    public float lifespan = 2f;
    float time_alive;

    public override void _Ready()
    {
        this.OnAreaEnterBody( node => {
            switch (node)
            {
                case Player player:
                    Debug.Log("Player hit by bullet");
                return;
            }
        });        
    }

    Vector3 velocity;

    public override void _PhysicsProcess(float delta)
    {
        time_alive += delta;
        Translation = Translation + velocity * delta;
        if (time_alive > lifespan)
        {
            GetParent().RemoveChild(this);
            bullets.Push(this);
        }
    }
}
