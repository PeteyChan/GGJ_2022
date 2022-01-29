using Godot;
using System;
using System.Collections.Generic;

public class Level : Node
{
    [Export]
    public float ScrollingSpeed = 5f;

    List<Spatial> LevelObjects = new List<Spatial>();

    public override void _Ready()
    {
        foreach(var child in this.GetChildren())
        {
            if (child is Spatial spatial)
                LevelObjects.Add(spatial);
        }    
    }

    List<Spatial> ActiveObjects = new List<Spatial>();

    public override void _PhysicsProcess(float delta)
    {
        for(int i = LevelObjects.Count-1; i >= 0; -- i)
        {
            var item = LevelObjects[i];
            var zpos = item.Translation.z;
            if (zpos > -12)
            {
                Debug.Log(item.Name, "entered play area");
                LevelObjects.RemoveAt(i);
                ActiveObjects.Add(item);
                item.Send<EnterPlayArea>(null);
            }
            else item.Translate(new Vector3(0, 0, ScrollingSpeed) * delta);
        }

        for(int i = ActiveObjects.Count - 1; i >= 0; --i)
        {
            var item = ActiveObjects[i];

            if (!item.IsValid())
            {
                ActiveObjects.RemoveAt(i);
                continue;
            }

            var x = item.Translation.x;
            var z = item.Translation.z;
            if (x < -14 || x > 14 || z < -14 || z > 14)
            {
                ActiveObjects.RemoveAt(i);
                item.Send<ExitPlayArea>(null);   
                item.QueueFree();
            }
        }
    }

    static void Restart(Command args)
    {
        Scene.Load("res://Scenes/Level/Level.tscn");
    }

    public class EnterPlayArea{}
    public class ExitPlayArea{}
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
