using Godot;
using System;

public class Enemy : RigidBody
{
    public int health = 3;

    public override void _Ready()
    {
        this.SetPhysicsProcess(false);
        this.CollisionLayer = 0;
        sprite = this.FindChild<Sprite3D>();
        spriteColor = sprite.Modulate;

        this.OnEnterPlayArea( () => {
            previous_health = health;
            this.CollisionLayer = 1;
            var pos = GlobalTransform.origin;
            this.SetPhysicsProcess(true);
        });

        this.OnExitPlayArea( () => {
            Debug.Log(this.Name, "left the play area");
            QueueFree();
        });
    }

    Sprite3D sprite;
    Color spriteColor;

    float shoot_timer, damage_timer;
    
    int previous_health;
    
    public override void _PhysicsProcess(float delta)
    {
        if (previous_health != health)
        {
            previous_health = health;
            damage_timer = .5f;
        }

        if (damage_timer > 0)
        {
            damage_timer -= delta;
            sprite.Modulate = Colors.White.Altenate(Colors.Red, 15f);
        }
        else sprite.Modulate = spriteColor;


        LinearVelocity = 5f*Vector3.Back;
        shoot_timer += delta;
        if (shoot_timer > 1f)
        {
            Bullet.Spawn(this, GlobalTransform.origin, Vector3.Back, 10f, 3f);
            shoot_timer = 0;
        }

        if (health <= 0) QueueFree();
    }    

}

public static partial class Extensions
{
    public static Color Altenate(this Color initial, Color target, float speed = 1f, float offset = 0)
    {
        return initial.lerp(target, Mathf.Sin(Time.seconds_since_startup * speed + offset)/2f + .5f);
    }
}