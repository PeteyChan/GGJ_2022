using Godot;
using System;
using System.Collections.Generic;

public class Level : Node
{
    [Export] public float InactiveEnemyScrollSpeed = 5f;
    [Export] public float ActiveEnemyScrollSpeed = 1f;
    [Export] public bool BossOnly = false;

    List<Spatial> LevelObjects = new List<Spatial>();

    public override void _Ready()
    {
        LevelObjects.Clear();
        ActiveObjects.Clear();

        if (BossOnly)
        {
            var boss = this.FindChild<Boss>();
            LevelObjects.Add(boss);
            boss.Translation = boss.Translation.setZ(-15);        
        }
        else
        {
            foreach (var child in this.GetChildren())
            {
                if (child is Spatial spatial)
                    LevelObjects.Add(spatial);
            }
        }
    }

    public static List<Spatial> ActiveObjects = new List<Spatial>();

    public override void _PhysicsProcess(float delta)
    {
        for (int i = LevelObjects.Count - 1; i >= 0; --i)
        {
            var item = LevelObjects[i];
            var zpos = item.Translation.z;

            if (zpos > -12)
            {
                LevelObjects.RemoveAt(i);
                ActiveObjects.Add(item);
                item.Send<EnterPlayArea>(null);
            }
            else item.Translate(new Vector3(0, 0, InactiveEnemyScrollSpeed) * delta);
        }

        for (int i = ActiveObjects.Count - 1; i >= 0; --i)
        {
            var item = ActiveObjects[i];

            if (!item.IsValid())
            {
                ActiveObjects.RemoveAt(i);
                continue;
            }

            item.Translation += new Vector3(0, 0, GetActiveScrollSpeed(item) * delta * ActiveEnemyScrollSpeed);

            var x = item.Translation.x;
            var z = item.Translation.z;
            if (x < -14 || x > 14 || z > 32)
            {
                ActiveObjects.RemoveAt(i);
                item.Send<ExitPlayArea>(null);
            }
        }
    }

    public float GetActiveScrollSpeed(object obj)
    {
        switch (obj)
        {
            case EnemySettings enemy:
                return enemy.level_scroll_speed;

            case Boss boss:
                return boss.active ? 0 : 1f;

            default:
                return 1f;
        }
    }

    static void Restart(Command args)
    {
        Scene.Load("res://Scenes/Level/Level.tscn");
    }

    public class EnterPlayArea { }
    public class ExitPlayArea { }
}

public static partial class Extensions
{
    public static Emitter OnEnterPlayArea<Emitter>(this Emitter emitter, Action action) where Emitter : Spatial
    {
        emitter.OnCallback<Level.EnterPlayArea>(area => action());
        return emitter;
    }

    public static Emitter OnExitPlayArea<Emitter>(this Emitter emitter, Action action) where Emitter : Spatial
    {
        emitter.OnCallback<Level.ExitPlayArea>(area => action());
        return emitter;
    }
}
