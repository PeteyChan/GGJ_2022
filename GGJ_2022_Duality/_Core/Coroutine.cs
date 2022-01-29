using System;
using System.Collections;

public static class Coroutine
{
    /// <summary>
    /// calls the update until update returns false
    /// </summary>
    public static void Start(System.Func<bool> update)
    {
        Updater.Instance.Add(update);
    }

    /// <summary>
    /// Performs the action until the condition is false
    /// </summary>
    public static void Start(System.Func<bool> condition, System.Action action)
    {
        Start(() => { action(); return condition(); });
    }

    /// <summary>
    /// Starts coroutine to update each frame
    /// </summary>
    public static void Start(this IEnumerator coroutine)
    {
        Start(() => coroutine.MoveNext());
    }

    /// <summary>
    /// Calls action next frame
    /// </summary>
    /// <param name="action"></param>
    public static void Defer(System.Action action)
        => DeferFrames(0, action);

    /// <summary>
    /// Delays an action to be performed until after frames have passed
    /// </summary>
    public static void DeferFrames(int frames, System.Action action)
    {
        int target_frame = Time.frame_count + frames;

        Start(() =>
        {
            if (Time.frame_count > target_frame)
            {
                action();
                return false;
            }
            return true;
        });
    }

    public static void DeferSeconds(float seconds, System.Action action)
    {
        float target_seconds = Time.seconds_since_startup + seconds;

        Start(() =>
        {
            if (Time.seconds_since_startup > target_seconds)
            {
                action();
                return false;
            }
            return true;
        });
    }

    public static Node Defer<Node>(this Node node, System.Action action) where Node : Godot.Node
    {
        Coroutine.DeferFrames(0, action);
        return node;
    }

    public static Node Defer<Node>(this Node node, float seconds, System.Action action) where Node : Godot.Node
    {
        Coroutine.DeferSeconds(seconds, action);
        return node;        
    }

    public static Node Defer<Node>(this Node node, int frames, System.Action action) where Node: Godot.Node
    {
        Coroutine.DeferFrames(frames, action);
        return node;
    }

    class Updater : Godot.Node
    {
        static Updater _instance;
        public static Updater Instance
        {
            get
            {
                if (_instance.IsNull())
                {
                    _instance = new Updater{Name = "Coroutine Manager"};
                    Scene.Current.CallDeferred("add_child", _instance);
                }
                return _instance;
            }
        }

        public void Add(System.Func<bool> action)
        {
            coroutines[coroutine_count] = action;
            coroutine_count++;
            if (coroutine_count == coroutines.Length)
                System.Array.Resize(ref coroutines, coroutines.Length * 2);
        }

        System.Func<bool>[] coroutines = new System.Func<bool>[16];
        int coroutine_count;

        bool process = true;
        public override void _Process(float delta)
        {
            for (int i = coroutine_count - 1; i >= 0; --i)
            {
                if (!coroutines[i]())
                {
                    coroutine_count--;
                    coroutines[i] = coroutines[coroutine_count];
                    coroutines[coroutine_count] = default;
                }
            }
        }
    }
}