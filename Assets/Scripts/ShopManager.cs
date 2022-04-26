using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[3, 3];

    public GameObject ShopMenuUI;
    private static bool spawned = false;
    void Awake(){
        DontDestroyOnLoad (this);
        if(spawned)     
            Destroy(gameObject);
        else        
            spawned = true;
    }
    void Start()
    {
        ShopMenuUI.SetActive(false);
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        
        shopItems[2, 1] = 50;
        shopItems[2, 2] = 150;
        shopItems[2, 3] = 30;
    }

    public void Buy(int id)
    {
        var pi = GameObject.Find("Player").GetComponent<PlayerInventory>();

        if (pi.GetCoins() < shopItems[2, id]) return;
        pi.SetCoins(pi.GetCoins() - shopItems[2, id]);
        switch (id)
        {
            case 1:
                pi.UpgradePlayer(PlayerInventory.Upgrades.HealthUp);
                break;
            case 2:
                pi.UpgradePlayer(PlayerInventory.Upgrades.DamageLevelUp);
                break;
        }
    }

    public void OpenShop()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().Freeze();
        ShopMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().Unfreeze();
        ShopMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
