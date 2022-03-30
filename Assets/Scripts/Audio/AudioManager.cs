using System;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour {
        public Sound[] sounds;
        private static bool spawned = false;

        private void Awake()
        {
            DontDestroyOnLoad (this);
            if(spawned)     
                Destroy(gameObject);
            else        
                spawned = true;
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }
        }
        public void Play(string name) 
        {
            Sound s = Array.Find(sounds, s => s.name == name);
            s.source.Play();
        }

        public void SetLevel(float sliderValue)
        {
            Array.ForEach(sounds, s => s.source.volume = sliderValue);
        }
    }
}

