using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class AudioManager : MonoBehaviour {
        public Sound[] sounds;
        private static bool spawned = false;
        private int prevLevel = 0;
        
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

        private void Start()
        {
        DontDestroyOnLoad(gameObject);
            FindObjectOfType<AudioManager>().Play("MenuMusic");
        }
    
        public void PlayLevelMusic(int level)
        {
            prevLevel = level;
            switch (level)
            {
                case 1:
                    FindObjectOfType<AudioManager>().Play("ForestMusic");
                    FindObjectOfType<AudioManager>().Play("ForestAmbience");
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Play("CaveMusic");
                    FindObjectOfType<AudioManager>().Play("CaveAmbience");
                    break;
            }
        }
        
        public void StopLevelMusic()
        {
            switch (prevLevel)
            {
                case 0:
                    FindObjectOfType<AudioManager>().Stop("MenuMusic");
                    break;
                case 1:
                    FindObjectOfType<AudioManager>().Stop("ForestMusic");
                    FindObjectOfType<AudioManager>().Stop("ForestAmbience");
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Stop("CaveAmbience");
                    FindObjectOfType<AudioManager>().Stop("CaveMusic");
                    break;
            }
        }

        public void Play(string name) 
        {
            Sound s = Array.Find(sounds, s => s.name == name);
            s.source.Play();
          
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, s => s.name == name);
            s.source.Stop();
        }

        public void SetLevel(float sliderValue)
        {
            Array.ForEach(sounds, s => s.source.volume = sliderValue);
        }
    }
}

