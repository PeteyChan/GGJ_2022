using Godot;
using System;

public class EnemySettings : Path
{    
    [Export] public bool _auto_start = false;
    [Export] public float level_scroll_speed = 1f;    
    [Export] public bool _reverse_movement = false;
    [Export] public float _shoot_min_interval = 1f;
    [Export] public float _shoot_max_interval = 3f;
    [Export] public float _move_speed = 5f;
    [Export] public float _damaged_move_speed = 1f;
    [Export] public float _health = 3f;
}
