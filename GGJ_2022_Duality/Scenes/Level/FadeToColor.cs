using Godot;
using System;

public class FadeToColor : ColorRect
{
    static FadeToColor instance;

    Color current;

    Color? color;
    float fade_timer, current_fade;

    public override void _Ready()
    {
        instance = this;
    }

    public override void _Process(float delta)
    {
        if (color.HasValue)
        {
            if (current_fade > fade_timer)
            {
                color = null;
            }
            else
            {
                Modulate = current.lerp(color.Value, current_fade / fade_timer);
                current_fade += delta;
            }
        }
    }

    public static void SetColor(Color color, float duration = 1f)
    {
        if (!instance.IsValid()) return;
        instance.current = instance.Modulate;
        instance.fade_timer = duration;
        instance.color = color;
        instance.current_fade = 0;
        instance.Modulate = new Color(color.r, color.g, color.b, 0);
    }
}
