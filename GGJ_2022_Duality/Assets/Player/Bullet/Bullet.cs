using Godot;
using System;
using System.Collections.Generic;

public class Bullet : Area
{
    public static Bullet Spawn(Node owner, Vector3 position, Vector3 direction, float speed, float lifespan = 2f)
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
        bullet.lifespan = lifespan;
        bullet.owner = owner;
        Scene.Current.AddChild(bullet);
        return bullet;
    }

    static Stack<Bullet> bullets = new Stack<Bullet>();
        
    [Export]
    public float lifespan = 2f;
    float time_alive;
    Node owner;
    Vector3 velocity;
    Sprite3D sprite;

    public override void _Ready()
    {
        this.OnAreaEnterBody(node => {
            switch (node)
            {
                case Player player:
                {
                    if (owner is Enemy enemy)
                        player.Health --;
                }
                break;

                case Enemy enemy:
                {                    
                    if (owner is Player player)
                        enemy.health --;
                }                
                break;
            }
        });
    }

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
        if (time_alive > lifespan)
        {
            GetParent().RemoveChild(this);
            bullets.Push(this);
            SetColor(Colors.White);
        }
    }
}
