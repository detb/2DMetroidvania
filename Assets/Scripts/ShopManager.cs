using Player;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[3, 4];

    public GameObject ShopMenuUI;
    private static bool spawned = false;
    private PlayerController pc;
    
    void Awake(){
        DontDestroyOnLoad (this);
        if(spawned)     
            Destroy(gameObject);
        else        
            spawned = true;
    }
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        ShopMenuUI.SetActive(false);
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        
        shopItems[2, 1] = 50;
        shopItems[2, 2] = 150;
        shopItems[2, 3] = 1337;
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
            case 3:
                // Mystery upgrade
                break;
        }
    }

    public void OpenShop()
    {
        pc.Freeze();
        ShopMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        pc.Unfreeze();
        ShopMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public bool IsShopOpen()
    {
        return ShopMenuUI.activeSelf;
    }
}
