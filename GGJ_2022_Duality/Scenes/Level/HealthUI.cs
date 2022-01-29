using Godot;
using System;

public class HealthUI : Label
{
    Player player;

    public override void _Ready()
    {
        player = Scene.Current.FindChild<Player>();
    }

    int current;

    public override void _Process(float delta)
    {
        if (current != player.health)
            Text = $"Health: {current = player.health}";
    }

}
