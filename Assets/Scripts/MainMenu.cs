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
        FindObjectOfType<AudioManager>().Stop("MenuMusic");
        FindObjectOfType<AudioManager>().Play("ForestMusic");
        FindObjectOfType<AudioManager>().Play("ForestAmbience");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
