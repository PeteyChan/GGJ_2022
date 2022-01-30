using Godot;
using System;

public class BGM : AudioStreamPlayer
{
    static BGM instance;

    AudioStream next;

    float max_volume;

    public override void _Ready()
    {
        max_volume = VolumeDb;
        instance = this;
    }

    public override void _Process(float delta)
    {
        if (next.IsValid())
        {
            float volume = 1f-(Time.seconds_since_startup-next_start)*2f;
            if (volume <= 0)
            {
                Stream = next;
                next = null;
                VolumeDb = max_volume;
            }
            else
            {
                VolumeDb = max_volume * volume;
            }
        }

        if (!Playing)
            Play();
    }

    static float next_start = 0;

    public static void Play(AudioStream stream)
    {
        next_start = Time.seconds_since_startup;
        instance.Stream = stream;
        instance.Play();
    }
}
