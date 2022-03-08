using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int playerCoins;
    private PlayerController player;

    public enum Upgrades
    {
        DoubleJump, Dash, BiggerSword
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
            default: break;
        }
    }

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCoin()
    {
        playerCoins++;
    }
}
