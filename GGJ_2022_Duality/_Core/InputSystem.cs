using Godot;
using System.Collections.Generic;
using GameSystems;

public enum Inputs : byte
{
    //keyboard
    key_a,
    key_b,
    key_c,
    key_d,
    key_e,
    key_f,
    key_g,
    key_h,
    key_i,
    key_j,
    key_k,
    key_l,
    key_m,
    key_n,
    key_o,
    key_p,
    key_q,
    key_r,
    key_s,
    key_t,
    key_u,
    key_v,
    key_w,
    key_x,
    key_y,
    key_z,

    key_1,
    key_2,
    key_3,
    key_4,
    key_5,
    key_6,
    key_7,
    key_8,
    key_9,
    key_0,

    key_f1,
    key_f2,
    key_f3,
    key_f4,
    key_f5,
    key_f6,
    key_f7,
    key_f8,
    key_f9,
    key_f10,
    key_f11,
    key_f12,

    key_print_scrren,
    key_num_lock,
    key_scroll_lock,
    key_pause_break,
    key_escape,
    key_back_quote,
    key_caps_lock,
    key_context_menu,

    key_tab,
    key_space,
    key_shift,
    key_control,
    key_alt,

    key_left_bracket,
    key_right_bracket,
    key_semi_colon,
    key_quote,
    key_less_than,
    key_greater_than,
    key_back_slash,
    key_forward_slash,

    key_minus,
    key_equals,
    key_backspace,
    key_enter,

    key_insert,
    key_home,
    key_page_up,
    key_page_down,
    key_delete,
    key_end,

    key_up_arrow,
    key_down_arrow,
    key_left_arrow,
    key_right_arrow,

    key_pad_divide,
    key_pad_multiply,
    key_pad_minus,
    key_pad_plus,

    key_pad_0,
    key_pad_1,
    key_pad_2,
    key_pad_3,
    key_pad_4,
    key_pad_5,
    key_pad_6,
    key_pad_7,
    key_pad_8,
    key_pad_9,
    key_pad_dot,
    key_pad_enter,

    //mouse

    mouse_left_click,
    mouse_right_click,
    mouse_middle_click,
    mouse_wheel_up,
    mouse_wheel_down,
    mouse_move_up,
    mouse_move_down,
    mouse_move_left,
    mouse_move_right,
    mouse_extra_button1,
    mouse_extra_button2,

    // controllers

    joy1_start,
    joy1_select,
    joy1_dpad_up,
    joy1_dpad_down,
    joy1_dpad_left,
    joy1_dpad_right,
    joy1_lstick_up,
    joy1_lstick_down,
    joy1_lstick_left,
    joy1_lstick_right,
    joy1_rstick_up,
    joy1_rstick_down,
    joy1_rstick_left,
    joy1_rstick_right,
    joy1_left_shoulder,
    joy1_left_trigger,
    joy1_left_hat,
    joy1_right_shoulder,
    joy1_right_trigger,
    joy1_right_hat,
    joy1_button_cross,
    joy1_button_circle,
    joy1_button_triangle,
    joy1_button_square,
    joy1_home,


    joy2_start,
    joy2_select,
    joy2_dpad_up,
    joy2_dpad_down,
    joy2_dpad_left,
    joy2_dpad_right,
    joy2_lstick_up,
    joy2_lstick_down,
    joy2_lstick_left,
    joy2_lstick_right,
    joy2_rstick_up,
    joy2_rstick_down,
    joy2_rstick_left,
    joy2_rstick_right,
    joy2_left_shoulder,
    joy2_left_trigger,
    joy2_left_hat,
    joy2_right_shoulder,
    joy2_right_trigger,
    joy2_right_hat,
    joy2_button_cross,
    joy2_button_circle,
    joy2_button_triangle,
    joy2_button_square,
    joy2_home,

    joy3_start,
    joy3_select,
    joy3_dpad_up,
    joy3_dpad_down,
    joy3_dpad_left,
    joy3_dpad_right,
    joy3_lstick_up,
    joy3_lstick_down,
    joy3_lstick_left,
    joy3_lstick_right,
    joy3_rstick_up,
    joy3_rstick_down,
    joy3_rstick_left,
    joy3_rstick_right,
    joy3_left_shoulder,
    joy3_left_trigger,
    joy3_left_hat,
    joy3_right_shoulder,
    joy3_right_trigger,
    joy3_right_hat,
    joy3_button_cross,
    joy3_button_circle,
    joy3_button_triangle,
    joy3_button_square,
    joy3_home,

    joy4_start,
    joy4_select,
    joy4_dpad_up,
    joy4_dpad_down,
    joy4_dpad_left,
    joy4_dpad_right,
    joy4_lstick_up,
    joy4_lstick_down,
    joy4_lstick_left,
    joy4_lstick_right,
    joy4_rstick_up,
    joy4_rstick_down,
    joy4_rstick_left,
    joy4_rstick_right,
    joy4_left_shoulder,
    joy4_left_trigger,
    joy4_left_hat,
    joy4_right_shoulder,
    joy4_right_trigger,
    joy4_right_hat,
    joy4_button_cross,
    joy4_button_circle,
    joy4_button_triangle,
    joy4_button_square,
    joy4_home,

}

public class InputAction
{
    /// <summary>
    /// Input values less than this will return 0
    /// </summary>
    public float deadzone = .2f;

    public InputAction(params Inputs[] inputs)
    {
        foreach (var input in inputs)
            this.inputs.Add(input);
    }

    /// <summary>
    /// Returns true if remapping was successful
    /// </summary>
    public InputAction Remap(params Inputs[] remapped)
    {
        inputs.Clear();
        inputs.AddRange(remapped);
        return this;
    }

    public Inputs[] GetInputs() => inputs.ToArray();

    List<Inputs> inputs = new List<Inputs>();

    public bool pressed => raw_value > deadzone;
    public bool released => raw_value < deadzone;

    public bool on_pressed
    {
        get
        {
            if (pressed)
            {
                foreach (var item in inputs)
                {
                    if (item.GetPreviousValue() > deadzone)
                        return false;
                }
                return true;
            }
            return false;
        }
    }

    public bool on_released
    {
        get
        {
            if (released)
            {
                foreach (var item in inputs)
                {
                    if (item.GetPreviousValue() > deadzone)
                        return true;
                }
            }
            return false;
        }
    }

    public float raw_value
    {
        get
        {
            float value = 0;
            foreach (var item in inputs)
            {
                value = value.min(item.GetCurrentValue());
            }
            return value;
        }
    }

    public float value
    {
        get
        {
            var val = raw_value;
            return val < deadzone ? 0 : val;
        }
    }

    public static implicit operator float(InputAction input) => input == null ? 0 : input.value;

    public bool ContainsInput(Inputs input)
    {
        return inputs.Contains(input);
    }

    public bool RemoveInput(Inputs input)
    => inputs.Remove(input);

    public bool ContainsInput(InputAction action)
    {
        if (action is null) return false;
        foreach (var item in inputs)
        {
            if (action.ContainsInput(item))
                return true;            
        }
        return false;
    }

    public void RemoveInput(InputAction action)
    {
        if (action is null) return;
        foreach(var item in action.inputs)
            inputs.Remove(item);
    }
}

public static class InputSystem
{
    public static float MouseMoveSensitivity = 10f;
    public static Vector2 MousePosition { get; set; }

    public static bool Pressed(this Inputs input, float deadzone = .2f)
    => current_inputs[(int)input] > deadzone;

    public static bool Released(this Inputs input, float deadzone = .2f)
    => current_inputs[(int)input] < deadzone;

    public static bool OnPress(this Inputs input, float deadzone = .2f)
    => current_inputs[(int)input] > deadzone && previous_inputs[(int)input] < deadzone;

    public static bool OnRelease(this Inputs input, float deadzone = .2f, int device = 0)
    => current_inputs[(int)input] < deadzone && previous_inputs[(int)input] > deadzone;

    public static float GetCurrentValue(this Inputs input)
    => current_inputs[(int)input];

    public static float GetPreviousValue(this Inputs input)
    => previous_inputs[(int)input];


    static int INPUT_COUNT = Enum<Inputs>.Count;

    static float[] current_inputs = new float[INPUT_COUNT];
    static float[] previous_inputs = new float[INPUT_COUNT];
    static float[] buffered = new float[INPUT_COUNT];

    static void Set(Inputs input, float value)
    {
        buffered[(int)input] = value;
    }

    static Vector2 next_position, current_position, last_position;

    [Event(int.MinValue)]
    static void Update(GameSystems.Events.SystemUpdate args)
    {

        last_position = current_position;
        current_position = next_position;

        var offset = current_position - last_position;
        float x = offset.x * 1f / (MouseMoveSensitivity.min(1f)), y = offset.y * 1f / (MouseMoveSensitivity.min(1f));

        Set(Inputs.mouse_move_left, (-x).min(0));
        Set(Inputs.mouse_move_right, (x).min(0));
        Set(Inputs.mouse_move_up, (-y).min(0));
        Set(Inputs.mouse_move_down, (y).min(0));
        last_position = current_position;
        
        var temp = previous_inputs;
        previous_inputs = current_inputs;
        current_inputs = temp;
        System.Array.Copy(buffered, current_inputs, current_inputs.Length);
        buffered[(int)Inputs.mouse_wheel_up] = 0;
        buffered[(int)Inputs.mouse_wheel_down] = 0;
    }

    [Event]
    static void OnInput(Godot.InputEvent args)
    {
        if (args is Godot.InputEventMouse mouse)
        {
            if (mouse is Godot.InputEventMouseMotion motion)
            {
                next_position += motion.Relative;
                MousePosition = motion.Position;
            }
            else if (mouse is Godot.InputEventMouseButton button)
            {
                var val = button.Pressed ? 1f : 0;
                switch (button.ButtonIndex)
                {
                    case 1: Set(Inputs.mouse_left_click, val); break;
                    case 2: Set(Inputs.mouse_right_click, val); break;
                    case 3: Set(Inputs.mouse_middle_click, val); break;
                    case 4:
                        if (button.Pressed)
                            buffered[(int)Inputs.mouse_wheel_up] = 1;
                        break;
                    case 5:
                        if (button.Pressed)
                            buffered[(int)Inputs.mouse_wheel_down] = 1;
                        break;
                    case 8: Set(Inputs.mouse_extra_button1, val); break;
                    case 9: Set(Inputs.mouse_extra_button2, val); break;
                }
            }
        }
        else if (args is Godot.InputEventKey key_event)
        {
            var value = key_event.Pressed ? 1f : 0f;

            switch (key_event.Scancode)
            {
                case 16777217: Set(Inputs.key_escape, value); break;
                case 16777218: Set(Inputs.key_tab, value); break;
                case 16777220: Set(Inputs.key_backspace, value); break;
                case 16777221: Set(Inputs.key_enter, value); break;

                case 16777223: Set(Inputs.key_insert, value); break;
                case 16777224: Set(Inputs.key_delete, value); break;
                case 16777225: Set(Inputs.key_pause_break, value); break;
                case 16777226: Set(Inputs.key_print_scrren, value); break;
                case 16777229: Set(Inputs.key_home, value); break;
                case 16777230: Set(Inputs.key_end, value); break;

                case 16777231: Set(Inputs.key_left_arrow, value); break;
                case 16777232: Set(Inputs.key_up_arrow, value); break;
                case 16777233: Set(Inputs.key_right_arrow, value); break;
                case 16777234: Set(Inputs.key_down_arrow, value); break;

                case 16777235: Set(Inputs.key_page_up, value); break;
                case 16777236: Set(Inputs.key_page_down, value); break;

                case 16777237: Set(Inputs.key_shift, value); break;
                case 16777238: Set(Inputs.key_control, value); break;
                case 16777240: Set(Inputs.key_alt, value); break;
                case 16777262: Set(Inputs.key_context_menu, value); break;

                case 16777241: Set(Inputs.key_caps_lock, value); break;
                case 16777242: Set(Inputs.key_num_lock, value); break;
                case 16777243: Set(Inputs.key_scroll_lock, value); break;

                case 16777244: Set(Inputs.key_f1, value); break;
                case 16777245: Set(Inputs.key_f2, value); break;
                case 16777246: Set(Inputs.key_f3, value); break;
                case 16777247: Set(Inputs.key_f4, value); break;
                case 16777248: Set(Inputs.key_f5, value); break;
                case 16777249: Set(Inputs.key_f6, value); break;
                case 16777250: Set(Inputs.key_f7, value); break;
                case 16777251: Set(Inputs.key_f8, value); break;
                case 16777252: Set(Inputs.key_f9, value); break;
                case 16777253: Set(Inputs.key_f10, value); break;
                case 16777254: Set(Inputs.key_f11, value); break;
                case 16777255: Set(Inputs.key_f12, value); break;

                case 16777345: Set(Inputs.key_pad_multiply, value); break;
                case 16777346: Set(Inputs.key_pad_divide, value); break;
                case 16777347: Set(Inputs.key_pad_minus, value); break;
                case 16777348: Set(Inputs.key_pad_dot, value); break;
                case 16777349: Set(Inputs.key_pad_plus, value); break;

                case 16777350: Set(Inputs.key_pad_0, value); break;
                case 16777351: Set(Inputs.key_pad_1, value); break;
                case 16777352: Set(Inputs.key_pad_2, value); break;
                case 16777353: Set(Inputs.key_pad_3, value); break;
                case 16777354: Set(Inputs.key_pad_4, value); break;
                case 16777355: Set(Inputs.key_pad_5, value); break;
                case 16777356: Set(Inputs.key_pad_6, value); break;
                case 16777357: Set(Inputs.key_pad_7, value); break;
                case 16777358: Set(Inputs.key_pad_8, value); break;
                case 16777359: Set(Inputs.key_pad_9, value); break;
                case 16777222: Set(Inputs.key_pad_enter, value); break;

                case 32: Set(Inputs.key_space, value); break;
                case 59: Set(Inputs.key_semi_colon, value); break;
                case 39: Set(Inputs.key_quote, value); break;
                case 44: Set(Inputs.key_less_than, value); break;
                case 46: Set(Inputs.key_greater_than, value); break;
                case 61: Set(Inputs.key_equals, value); break;
                case 45: Set(Inputs.key_minus, value); break;

                case 48: Set(Inputs.key_0, value); break;
                case 49: Set(Inputs.key_1, value); break;
                case 50: Set(Inputs.key_2, value); break;
                case 51: Set(Inputs.key_3, value); break;
                case 52: Set(Inputs.key_4, value); break;
                case 53: Set(Inputs.key_5, value); break;
                case 54: Set(Inputs.key_6, value); break;
                case 55: Set(Inputs.key_7, value); break;
                case 56: Set(Inputs.key_8, value); break;
                case 57: Set(Inputs.key_9, value); break;

                case 65: Set(Inputs.key_a, value); break;
                case 66: Set(Inputs.key_b, value); break;
                case 67: Set(Inputs.key_c, value); break;
                case 68: Set(Inputs.key_d, value); break;
                case 69: Set(Inputs.key_e, value); break;
                case 70: Set(Inputs.key_f, value); break;
                case 71: Set(Inputs.key_g, value); break;
                case 72: Set(Inputs.key_h, value); break;
                case 73: Set(Inputs.key_i, value); break;
                case 74: Set(Inputs.key_j, value); break;
                case 75: Set(Inputs.key_k, value); break;
                case 76: Set(Inputs.key_l, value); break;
                case 77: Set(Inputs.key_m, value); break;
                case 78: Set(Inputs.key_n, value); break;
                case 79: Set(Inputs.key_o, value); break;
                case 80: Set(Inputs.key_p, value); break;
                case 81: Set(Inputs.key_q, value); break;
                case 82: Set(Inputs.key_r, value); break;
                case 83: Set(Inputs.key_s, value); break;
                case 84: Set(Inputs.key_t, value); break;
                case 85: Set(Inputs.key_u, value); break;
                case 86: Set(Inputs.key_v, value); break;
                case 87: Set(Inputs.key_w, value); break;
                case 88: Set(Inputs.key_x, value); break;
                case 89: Set(Inputs.key_y, value); break;
                case 90: Set(Inputs.key_z, value); break;

                case 91: Set(Inputs.key_left_bracket, value); break;
                case 92: Set(Inputs.key_back_slash, value); break;
                case 93: Set(Inputs.key_right_bracket, value); break;
                case 96: Set(Inputs.key_back_quote, value); break;
                case 47: Set(Inputs.key_forward_slash, value); break;
            }
        }
        else if (args is Godot.InputEventJoypadMotion joy)
        {
            int device = joy.Device.clamp(0, 3);
            float val = joy.AxisValue;

            switch (joy.Axis)
            {
                case 0:
                    if (val < 0) SetValue(Inputs.joy1_lstick_left, -val, device);
                    if (val > 0) SetValue(Inputs.joy1_lstick_right, val, device);
                    break;
                case 1:
                    if (val < 0) SetValue(Inputs.joy1_lstick_up, -val, device);
                    if (val > 0) SetValue(Inputs.joy1_lstick_down, val, device);
                    break;
                case 2:
                    if (val < 0) SetValue(Inputs.joy1_rstick_left, -val, device);
                    if (val > 0) SetValue(Inputs.joy1_rstick_right, val, device);
                    break;
                case 3:
                    if (val < 0) SetValue(Inputs.joy1_rstick_up, -val, device);
                    if (val > 0) SetValue(Inputs.joy1_rstick_down, val, device);
                    break;
                case 6:
                    SetValue(Inputs.joy1_left_trigger, val, device);
                    break;
                case 7:
                    SetValue(Inputs.joy1_right_trigger, val, device);
                    break;
            }
        }
        else if (args is Godot.InputEventJoypadButton button)
        {
            int device = button.Device.clamp(0, 3);
            float value = button.Pressed ? 1 : 0;

            switch (button.ButtonIndex)
            {
                case 0: SetValue(Inputs.joy1_button_cross, value, device); break;
                case 1: SetValue(Inputs.joy1_button_circle, value, device); break;
                case 2: SetValue(Inputs.joy1_button_square, value, device); break;
                case 3: SetValue(Inputs.joy1_button_triangle, value, device); break;

                case 4: SetValue(Inputs.joy1_left_shoulder, value, device); break;
                case 8: SetValue(Inputs.joy1_left_hat, value, device); break;

                case 5: SetValue(Inputs.joy1_right_shoulder, value, device); break;
                case 9: SetValue(Inputs.joy1_right_hat, value, device); break;

                case 12: SetValue(Inputs.joy1_dpad_up, value, device); break;
                case 13: SetValue(Inputs.joy1_dpad_down, value, device); break;
                case 14: SetValue(Inputs.joy1_dpad_left, value, device); break;
                case 15: SetValue(Inputs.joy1_dpad_right, value, device); break;

                case 16: SetValue(Inputs.joy1_home, value, device); break;

                case 10: SetValue(Inputs.joy1_select, value, device); break;
                case 11: SetValue(Inputs.joy1_start, value, device); break;
            }
        }

        
        void SetValue(Inputs input, float new_value, int dev)
        {
            const int gamepad_inputs = (int)Inputs.joy1_home - (int)Inputs.joy1_start + 1;
            input = (Inputs)((int)input + (gamepad_inputs * dev));// (int)input + (gamepad_inputs + 1) * dev);
            Set(input, new_value);
        }
    }
}
