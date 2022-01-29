using Godot;
using System;

public class Enemy : Area
{
    EnemySettings settings;
    PathFollow follow_path;
    public Sprite3D sprite;
    Color spriteColor;
    float shoot_timer;
    public enum States
    {
        Setup,
        Default,
        Damaged,
        Destroyed
    }
    StateMachine<States> stateMachine = new StateMachine<States>();
    float uptime;

    bool damageable => stateMachine.current == States.Default ||
                        stateMachine.current == States.Damaged;
    public override void _PhysicsProcess(float delta)
    {
        stateMachine.Update(delta);
        switch (stateMachine.current)
        {
            case States.Setup:
                if (stateMachine.entered_state)
                {
                    sprite = this.FindChild<Sprite3D>();
                    spriteColor = sprite.Modulate;
                    follow_path = this.FindParent<PathFollow>();
                    settings = this.FindParent<EnemySettings>();

                    if (settings._auto_start)
                        stateMachine.next = States.Default;
                    else
                        this.FindParent<Path>().OnEnterPlayArea(() =>
                        {
                            stateMachine.next = States.Default;
                        });

                    this.FindParent<Path>().OnExitPlayArea(() =>
                    {
                        stateMachine.next = States.Destroyed;
                    });

                    this.FindChild<Area>().OnAreaEnterArea(node =>
                    {
                        switch (node)
                        {
                            case Bullet bullet:
                            {
                                if (bullet.source is Player)
                                {
                                    if (stateMachine.current == States.Damaged ||
                                        stateMachine.current == States.Default)
                                    {
                                        settings._health--;
                                        stateMachine.next = States.Damaged;
                                    }
                                    bullet.Destroy();
                                }
                            }
                            break;

                            case Player player:
                            {
                                settings._health--;
                                stateMachine.next = States.Damaged;
                            }
                            break;

                            default:
                                //Debug.Log(node?.Name);
                            break;
                        }

                    });
                }
                break;

            case States.Default:
            {
                if (stateMachine.entered_state)
                    sprite.Modulate = spriteColor;

                Move(settings._move_speed);

                if (shoot_timer < 0 && sprite.GlobalTransform.origin.z > -10)
                {
                    Bullet.Spawn(this, sprite.GlobalTransform.origin, Vector3.Back, 15f, 3f);
                    shoot_timer = Rand.FloatRange(settings._shoot_min_interval, settings._shoot_max_interval);
                }
                shoot_timer -= delta;
            }
            break;

            case States.Damaged:
            {
                Move(settings._damaged_move_speed);
                sprite.Modulate = Colors.White.Altenate(Colors.Red, 15f);
                if (stateMachine.current_time > .5f)
                    stateMachine.next = States.Default;

                if (settings._health <= 0)
                    stateMachine.next = States.Destroyed;
            }
            break;

            case States.Destroyed:
                QueueFree();
                break;
        }

        void Move(float speed)
        {
            follow_path.Offset = uptime += delta * speed * (settings._reverse_movement ? -1f : 1f);
        }
    }

}

public static partial class Extensions
{
    public static Color Altenate(this Color initial, Color target, float speed = 1f, float offset = 0)
    {
        return initial.lerp(target, Mathf.Sin(Time.seconds_since_startup * speed + offset) / 2f + .5f);
    }
}