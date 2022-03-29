using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
                }
    }

    private void Start()
    {
        Play("AmbienceSounds");
        Play("AmbienceMusic");
    }
    public void Play (string name) 
    {
       Sound s = Array.Find(sounds, s => s.name == name);
       s.source.Play();
    }
}
