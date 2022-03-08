using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public int playerCoins;
    private PlayerController player;


    public TextMeshProUGUI countText;
    public enum Upgrades
    {
        DoubleJump, Dash, HeavyAttack, Coin, DamageLevelUp, WallJump
    }

    public bool HasUpgrade(Upgrades upgrade)
    {
        return true;
    }
    public void UpgradePlayer(Upgrades upgrade)
    {
        switch (upgrade)
        {
            case Upgrades.DoubleJump: 
                player.extraJumpsValue = 2;
                player.extraJumps = 2;
                break;
            case Upgrades.Dash: break;
            case Upgrades.Coin: AddCoin(); break;
            default: break;
        }
    }

    void Start()
    {
        player = GetComponent<PlayerController>();
        SetCountText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCoin()
    {
        playerCoins++;
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "" + playerCoins;
    }
}
