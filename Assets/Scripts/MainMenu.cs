using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Audio;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        var am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.StopLevelMusic();
        am.PlayLevelMusic(1);
        am.Play("Play");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
