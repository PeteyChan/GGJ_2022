using Godot;
using System;

public class Player : RigidBody
{
    InputAction left = new InputAction(Inputs.key_a, Inputs.joy1_lstick_left);
    InputAction right = new InputAction(Inputs.key_d, Inputs.joy1_lstick_right);
    InputAction up = new InputAction(Inputs.key_s, Inputs.joy1_lstick_down);
    InputAction down = new InputAction(Inputs.key_w, Inputs.joy1_lstick_up);
    InputAction shoot = new InputAction(Inputs.key_space, Inputs.joy1_right_shoulder, Inputs.joy1_button_cross, Inputs.mouse_left_click);

    public override void _Ready()
    {
        sprite = this.FindChild<Sprite3D>();
        model = sprite.FindParent<Spatial>();
        
        previous_health = Health;
        

        this.OnEnterBody(node => {

            switch(node)
            {
                case Enemy enemy:
                    LinearVelocity = (Translation - enemy.Translation).Normalized() * 10f;
                    stateMachine.next = States.EnemyColliison;
                break;
            }

        });
    }

    Spatial model;
    Sprite3D sprite;

    Vector3 input_direction => new Vector3(right - left, 0, up - down);

    [Export]
    public float MoveSpeed = 10f;

    [Export]
    public int Health = 10;
    int previous_health;

    Vector3 move_direction;

    Color color = Colors.White;

    public StateMachine<States> stateMachine = new StateMachine<States>();

    public enum States
    {
        Default,
        EnemyColliison
    }

    float colliison_timer;

    float damage_timer;

    public override void _PhysicsProcess(float delta)
    {
        if (previous_health != Health)
        {
            damage_timer = .5f;
            previous_health = Health;
        }

        if (damage_timer > 0)
        {
            damage_timer -= delta;
            sprite.Modulate = Colors.White.lerp(Colors.Red, Mathf.Sin(Time.seconds_since_startup * 15f)/ 2f + .5f);
        }
        else sprite.Modulate = Colors.White;

        stateMachine.Update(delta);

        switch (stateMachine.current)
        {
            case States.Default:
            {
                var tilt = new Vector2(input_direction.x, input_direction.z).tilt();
                move_direction = move_direction.lerp(input_direction.Normalized() * tilt, 5f * delta);
                LinearVelocity = move_direction * MoveSpeed;

                var rotate = move_direction.x * 20f;
                model.RotationDegrees = new Vector3(0, 0, -rotate);

                if (shoot.on_pressed)
                {
                    Bullet.Spawn(this, Translation + Vector3.Left * .5f, Vector3.Forward, 20f);
                    Bullet.Spawn(this, Translation + Vector3.Right * .5f, Vector3.Forward, 20f);
                }
            }
            break;

            case States.EnemyColliison:
            {
                if (stateMachine.entered_state)
                    colliison_timer = 0;
                
                LinearVelocity = LinearVelocity.lerp(Vector3.Zero, delta);
                colliison_timer += delta;
                if (colliison_timer > .25f)
                    stateMachine.next = States.Default;

                if (stateMachine.exiting_state)
                    move_direction = new Vector3();
            }
            break;
        }
    }

    [Event] static void LogStates(Events.FrameUpdate update)
    {
        var state = Scene.Current.FindChild<Player>().stateMachine.current;
        Debug.Label(state);
    }
}

