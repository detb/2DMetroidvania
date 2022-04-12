using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Audio;
using Player;

public class MainMenu : MonoBehaviour
{
    private PlayerController pc;
    private void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        pc.SetRBGravity(0f);
        pc.Freeze();
        Load();
    }

    public void PlayGame()
    {
        var am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.StopLevelMusic();
        if (PlayerPrefs.GetInt("isSaved") == 1)
        {
            var index = PlayerPrefs.GetInt("levelIndex");
            SceneManager.LoadScene(index);
            am.PlayLevelMusic(index);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            am.PlayLevelMusic(1);
        }
        
        am.Play("Play");
        pc.SetRBGravity(2.25f);
        pc.Unfreeze();
    }

    private void Load()
    {
        if (PlayerPrefs.GetInt("isSaved") == 0) return;
        var loadedPos = new Vector3(
            PlayerPrefs.GetFloat("posX"),
            PlayerPrefs.GetFloat("posY"),
            PlayerPrefs.GetFloat("posZ")
            );
        pc.SetPlayerPosition(loadedPos);

        var respawnPos = new Vector3(
            PlayerPrefs.GetFloat("resPosX"),
            PlayerPrefs.GetFloat("resPosY"),
            PlayerPrefs.GetFloat("resPosZ")
        );
        
        var pi = GameObject.Find("Player").GetComponent<PlayerInventory>();
        pi.SetCoins(PlayerPrefs.GetInt("coins"));
        pi.SetRespawnIndex(PlayerPrefs.GetInt("respawnIndex"));
        pi.SetRespawnPoint(respawnPos);

        var dbjmp = PlayerPrefs.GetInt("DoubleJump"); 
        
        if (dbjmp == 1)
            pi.UpgradePlayer(PlayerInventory.Upgrades.DoubleJump);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
