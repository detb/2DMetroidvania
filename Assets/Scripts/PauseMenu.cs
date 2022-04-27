using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;

    public GameObject PauseMenuUI;

    private static bool spawned = false;
    private PlayerController pc;
    void Awake(){
        DontDestroyOnLoad (this);
        if(spawned)     
            Destroy(gameObject);
        else        
            spawned = true;
    }
    private void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        PauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetButtonDown("Cancel") || SceneManager.GetActiveScene().buildIndex == 0) return;
        if (isPaused)
            Resume();
        else
            Pause();
    }
    public void Resume()
    {
        pc.Unfreeze();
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pc.Freeze();
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        var playerpos = GameObject.Find("Player").transform.position;
        PlayerPrefs.SetFloat("posX", playerpos.x);
        PlayerPrefs.SetFloat("posY", playerpos.y);
        PlayerPrefs.SetFloat("posZ", playerpos.z);
        PlayerPrefs.SetInt("levelIndex", SceneManager.GetActiveScene().buildIndex);
        
        var pi = GameObject.Find("Player").GetComponent<PlayerInventory>();
        PlayerPrefs.SetInt("coins", pi.GetCoins());
        PlayerPrefs.SetFloat("resPosX", pi.GetRespawnPoint().x);
        PlayerPrefs.SetFloat("resPosY", pi.GetRespawnPoint().y);
        PlayerPrefs.SetFloat("resPosZ", pi.GetRespawnPoint().z);
        PlayerPrefs.SetInt("respawnIndex", pi.GetRespawnIndex());

        if(pi.HasUpgrade(PlayerInventory.Upgrades.DoubleJump))
            PlayerPrefs.SetInt("DoubleJump", 1);
        if(pi.HasUpgrade(PlayerInventory.Upgrades.Dash))
            PlayerPrefs.SetInt("Dash", 1);
        if(pi.HasUpgrade(PlayerInventory.Upgrades.HeavyAttack))
            PlayerPrefs.SetInt("HeavyAttack", 1);
        if(pi.HasUpgrade(PlayerInventory.Upgrades.WallJump))
            PlayerPrefs.SetInt("WallJump", 1);
        if(pi.HasUpgrade(PlayerInventory.Upgrades.DamageLevelUp))
            PlayerPrefs.SetInt("DamageLevelUp", 1);
        PlayerPrefs.SetInt("isSaved", 1);
        Application.Quit();
    }
}
