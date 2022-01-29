public class StateMachine<States> where States : struct, System.Enum
{
    public StateMachine(States initial_state = default)
    {
        next = current;
    }

    public bool enable_transitions = true;

    States? _next_state;
    public States? next
    {
        get => _next_state;
        set
        {
            if (enable_transitions)
                _next_state = value;
        }
    }

    public int total_update_count { get; private set; }

    /// <value></value>
    public States previous { get; private set; }

    /// <summary>
    /// times update was called while in previous state
    /// </summary>
    /// <value></value>
    public int previous_update_count { get; private set; }

    /// <summary>
    /// time spent in the previous state
    /// </summary>
    public float previous_time { get; private set; }

    public States current { get; private set; }

    public int current_update_count { get; private set; }

    /// <summary>
    /// time spent in the current state
    /// </summary>
    public float current_time { get; private set; }

    public bool entered_state => current_time == 0;

    public bool exiting_state => next != null;

    /// <summary>
    /// time between updates
    /// </summary>
    public float delta_time { get; private set; }

    public void Update(float delta_time)
    {
        total_update_count++;
        this.delta_time = delta_time;

        if (next.HasValue)
        {
            previous = current;
            current = next.Value;
            next = null;
            previous_time = current_time;
            previous_update_count = current_update_count;
            current_time = 0;
            current_update_count = 0;
        }
        else
        {
            current_time += delta_time;
            current_update_count++;
        }
    }

    public override string ToString() => $"{typeof(States).Name.Replace("_", " ")}: {current.ToString().Replace("_", " ")}";
}

public class StateMachine
{
    public float previous_time { get; private set; } = 0;
    public object previous { get; private set; }
    public object current { get; private set; }
    public float current_time { get; private set; } = 0;
    public object next;
    public bool entered_state => current_time == 0;
    public bool exiting_state => next != null;
    public void Update(float delta)
    {
        current_time += delta;
        if (next != null)
        {
            previous_time = current_time;
            current_time = 0;
            previous = current;
            current = next;
            next = null;
        }
    }
}