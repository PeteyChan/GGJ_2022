using Godot;
using System;

public class Boss : Area
{
    [Export] public int max_health = 40;
    public int health;

    float health_left => ((float)health / (float)max_health);

    [Export] public AudioStream music;

    [Export] public float shoot_speed;

    public bool active => statemachine.current != States.Setup;


    enum States
    {
        Setup,
        GoToNeutral,
        Default,
        Destroyed,
    }

    StateMachine<States> statemachine = new StateMachine<States>();

    Sprite3D sprite;

    float damage_timer;

    Vector3 enter_translation;

    Spatial shooter;

    float shoot_timer = 0;

    public override void _PhysicsProcess(float delta)
    {
        statemachine.Update(delta);
        switch (statemachine.current)
        {
            case States.Setup:
            {
                if (statemachine.entered_state)
                {
                    health = max_health;
                    sprite = this.FindChild<Sprite3D>();
                    this.OnEnterPlayArea(() =>
                    {
                        statemachine.next = States.GoToNeutral;
                        BGM.Play(music);
                    });

                    shooter = new Spatial();
                    Scene.Current.AddChild(shooter);

                    this.OnAreaEnterArea(node =>
                    {
                        switch (node)
                        {
                            case Bullet bullet:
                            {
                                if (active && bullet.source is Player)
                                {
                                    health--;
                                    damage_timer = .5f;
                                    bullet.Destroy();
                                }
                            }
                            break;
                        }

                    });
                }
            }
            break;

            case States.GoToNeutral:
            {
                if (statemachine.entered_state)
                    enter_translation = Translation;

                Translation = enter_translation.lerp(new Vector3(0, 0, -8), statemachine.current_time * 2f);

                if (statemachine.current_time * 2f > 1f)
                    statemachine.next = States.Default;
            }
            break;

            case States.Default:
                shooter.Translation = Translation;
                shooter.RotateY(90f * delta);

                if (shoot_timer > (.2f * health_left) + .02)
                {
                    Bullet.Spawn(this, Translation, shooter.GlobalTransform.basis.z, 10f);
                    Bullet.Spawn(this, Translation, -shooter.GlobalTransform.basis.z, 10f);
                    Bullet.Spawn(this, Translation, shooter.GlobalTransform.basis.x, 10f);
                    Bullet.Spawn(this, Translation, -shooter.GlobalTransform.basis.x, 10f);
                    shoot_timer = 0;
                }

                shoot_timer += delta;
                break;

            case States.Destroyed:
                Debug.Label("You Win");
                break;
        }

        if (damage_timer > 0)
        {
            sprite.Modulate = Colors.White.Altenate(Colors.Red, 15f);
            damage_timer -= delta;
        }
        else sprite.Modulate = Colors.White;

        if (health <= 0)
            statemachine.next = States.Destroyed;
    }

}
