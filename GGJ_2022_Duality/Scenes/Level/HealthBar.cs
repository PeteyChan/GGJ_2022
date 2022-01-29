using Godot;
using System;

public class HealthBar : Panel
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = Scene.Current.FindChild<Player>();        
        max_health = player.health;
        rect = this.FindChild<ColorRect>();
    }

    Player player;
    ColorRect rect;

    float max_health;

    public override void _Process(float delta)
    {
        float amount = (float)player.health / max_health;

        rect.RectScale = new Vector2(amount , 1);
        var target_color = Colors.Red.lerp(Colors.PaleGreen, amount);

        rect.Modulate = target_color.lerp( new Color(target_color.r + .2f, target_color.g + .2f, target_color.b + .2f, 1), Mathf.Sin(Time.seconds_since_startup * 12f) *.5f+ .5f);
    }
}
