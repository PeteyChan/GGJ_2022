using Godot;
using System;

public class Player : Area
{
    InputAction left = new InputAction(Inputs.key_a, Inputs.joy1_lstick_left);
    InputAction right = new InputAction(Inputs.key_d, Inputs.joy1_lstick_right);
    InputAction up = new InputAction(Inputs.key_s, Inputs.joy1_lstick_down);
    InputAction down = new InputAction(Inputs.key_w, Inputs.joy1_lstick_up);
    InputAction shoot = new InputAction(Inputs.key_space, Inputs.joy1_right_shoulder, Inputs.joy1_button_cross, Inputs.mouse_left_click);

    bool enable_invincible;
    AnimationPlayer animator;
    Spatial model;
    AnimatedSprite3D sprite;
    Boss boss;

    Vector3 input_direction => new Vector3(right - left, 0, up - down);

    [Export] public float move_speed = 20f;
    [Export] public float damaged_move_speed = 7f;
    [Export] public int health = 10;
    Vector3 move_direction;
    public StateMachine<States> stateMachine = new StateMachine<States>();

    public enum States
    {
        SetUp,
        Default,
        Invincible,
        EnemyColliison,
        Damaged,
        Destroyed,
        Win
    }

    float colliison_timer;

    float damage_timer;

    Vector3 enemy_position;

    bool damageable => stateMachine.current == States.Default ||
                            stateMachine.current == States.EnemyColliison;

    bool shooting;

    public override void _PhysicsProcess(float delta)
    {
        shooting = false;

        stateMachine.Update(delta);

        switch (stateMachine.current)
        {
            case States.SetUp:
            {
                if (stateMachine.entered_state)
                {
                    sprite = this.FindChild<AnimatedSprite3D>();
                    model = sprite.FindParent<Spatial>();
                    animator = this.FindChild<AnimationPlayer>();
                    boss = Scene.Current.FindChild<Boss>();

                    var area = this.FindChild<Area>();

                    area.OnAreaEnterArea(node =>
                    {
                        if (!damageable) return;

                        switch (node)
                        {
                            case Bullet bullet:
                            {
                                if (bullet.source is Player)
                                    break;

                                health--;
                                stateMachine.next = States.Damaged;
                                bullet.Destroy();
                            }
                            break;

                            case Enemy enemy:
                            {
                                enemy_position = enemy.sprite.GlobalTransform.origin;
                                stateMachine.next = States.EnemyColliison;
                            }
                            break;

                            case Boss boss:
                            {
                                enemy_position = boss.GlobalTransform.origin;
                                stateMachine.next = States.EnemyColliison;
                            }
                            break;
                        }
                    });
                }
                stateMachine.next = States.Default;
            }
            break;

            case States.Invincible:
            {
                if (stateMachine.entered_state)
                    sprite.Modulate = Colors.LightBlue;

                if (!enable_invincible)
                    stateMachine.next =  States.Default;

                CanMove(move_speed);
                CanShoot();
            }
            break;

            case States.Default:
            {
                if (enable_invincible)
                    stateMachine.next = States.Invincible;

                if (stateMachine.entered_state)
                    sprite.Modulate = Colors.White;

                CanMove(move_speed);
                CanShoot();
            }
            break;

            case States.EnemyColliison:
            {
                if (stateMachine.entered_state)
                {
                    health--;
                    OnEnemyCollision.PlaySound();
                    PlayerDamageSound.PlaySound();
                }

                DamageEffect();

                Translation += ((Translation - enemy_position).Normalized() * 10f * delta).lerp(Vector3.Zero, stateMachine.current_time * 4f);

                if (stateMachine.current_time > .25f)
                    stateMachine.next = States.Default;

                if (stateMachine.exiting_state)
                    move_direction = new Vector3();

                if (health <= 0)
                    stateMachine.next = States.Destroyed;
            }
            break;

            case States.Damaged:
            {
                if (stateMachine.entered_state)
                    PlayerDamageSound.PlaySound();

                CanMove(damaged_move_speed);
                CanShoot();
                DamageEffect();

                if (stateMachine.current_time > .5f)
                    stateMachine.next = States.Default;

                if (health <= 0)
                    stateMachine.next = States.Destroyed;

            }
            break;

            case States.Destroyed:
            {
                if (stateMachine.entered_state)
                    FadeToColor.SetColor(new Color(0,0,0,1), 3f);


                sprite.Visible = false;
                if (stateMachine.current_time > 4f)
                {
                    Coroutine.Defer(
                        () => Scene.Load("res://Scenes/Level/Level.tscn")
                    );
                }
            }
            break;

            case States.Win:

                if (stateMachine.entered_state)
                {
                    FadeToColor.SetColor(Colors.White , 3f);
                }

                if (stateMachine.current_time > 4f)
                    Scene.Load("res://Scenes/Victory/Victory.tscn");
                
                sprite.Modulate = Colors.White;
                Translation = Translation + Vector3.Forward * 8f * delta;
                break;
        }

        if (stateMachine.current != States.Win)
            Translation = new Vector3(Translation.x.clamp(x_min, x_max), 0, Translation.z.clamp(z_min, z_max));

        if (stateMachine.current != States.Win && boss?.health <= 0)
            stateMachine.next = States.Win;

#if DEBUG
        //Debug.Label("Player position:", Translation);
#endif

        if (shooting)
        {
            animator.Play("Shoot");
        }
        else animator.Play("Idle");

        void DamageEffect()
        {
            sprite.Modulate = Colors.White.Altenate(Colors.Red, 20f);
        }

        void CanShoot()
        {
            if (shoot.pressed)
                shooting = true;

            /*
            if (shoot.pressed && Time.seconds_since_startup - last_shot > shot_cooldown)
            {
                Bullet.Spawn(this, Translation + new Vector3(-.5f, 0, 0), Vector3.Forward, 20f, 3f);
                Bullet.Spawn(this, Translation + new Vector3(.5f, 0, 0), Vector3.Forward, 20f, 3f);
                last_shot = Time.seconds_since_startup;
            }*/
        }

        void CanMove(float move_speed)
        {
            var tilt = new Vector2(input_direction.x, input_direction.z).tilt();
            move_direction = move_direction.lerp(input_direction.Normalized() * tilt, 5f * delta);
            Translation = Translation += move_direction * move_speed * delta;
        }
    }

    float last_shot;

    [Export] float shot_cooldown = .25f;

    [Export] public float x_min = -11;
    [Export] public float x_max = 11;
    [Export] public float z_min = -10;
    [Export] public float z_max = 10;

    [Export] public float shot_speed = 20f;

    public void ShootLeft()
    {
        Bullet.Spawn(this, Translation + new Vector3(-.75f, 0, -1f), Vector3.Forward, shot_speed);
        PlayerShootSound.PlaySound();
    }

    public void ShootRight()
    {
        Bullet.Spawn(this, Translation + new Vector3(.75f, 0, -1f), Vector3.Forward, shot_speed);
        PlayerShootSound.PlaySound();
    }

    static void GodMode(Command args)
    {
        var player = Scene.Current.FindChild<Player>();
        if (player.IsValid())
        {
            player.enable_invincible = !player.enable_invincible;
        }
    }
}

