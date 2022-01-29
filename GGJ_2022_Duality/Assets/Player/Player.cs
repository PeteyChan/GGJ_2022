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
        model = this.FindChild<Sprite3D>().FindParent<Spatial>();
    }

    Spatial model;

    Vector3 input_direction => new Vector3(right - left, 0, up - down);

    [Export]
    public float MoveSpeed = 10f;

    Vector3 move_direction;

    public override void _PhysicsProcess(float delta)
    {
        var tilt = new Vector2(input_direction.x, input_direction.z).tilt();
        move_direction = move_direction.lerp(input_direction.Normalized() * tilt, 5f * delta);
        LinearVelocity = move_direction * MoveSpeed;

        var rotate = move_direction.x * 30f;
        model.RotationDegrees = new Vector3(0, 0, rotate);

        if (shoot.on_pressed)
            Bullet.Spawn(Translation, Vector3.Forward, 20f);
    }
}
