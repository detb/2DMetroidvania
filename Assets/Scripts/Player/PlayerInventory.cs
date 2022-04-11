using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private static int playerCoins;
        private PlayerController player;
        private Vector3 respawnPoint = new Vector3(-2.5f, 0, 1); // Start respawn point.
        private int respawnIndex = 1;
        private List<Upgrades> playerUpgrades = new List<Upgrades>();

        public TextMeshProUGUI countText;
        public TextMeshProUGUI respawnHint;
        public TextMeshProUGUI respawnSet;
        public enum Upgrades
        {
            DoubleJump, Dash, HeavyAttack, Coin, DamageLevelUp, WallJump
        }

        // Nothing in this for now, might be useful later?
        public bool HasUpgrade(Upgrades upgrade)
        {
            foreach (var upg in playerUpgrades)
            {
                if (upg == upgrade)
                    return true;
            }
            return false;
        }
        public void UpgradePlayer(Upgrades upgrade)
        {
            switch (upgrade)
            {
                case Upgrades.DoubleJump: 
                    player.extraJumpsValue = 2;
                    player.extraJumps = 2;
                    // Add upgrade to list of upgrades player has.
                    playerUpgrades.Add(Upgrades.DoubleJump);
                    //Plays cool sound for picking up new power
                    FindObjectOfType<AudioManager>().Play("PowerPickup");
                    // Set's respawn point in cave, as if the player dies after the powerup,
                    // the player is returned to try again
                    respawnPoint = player.transform.position;
                    respawnIndex = 2;
                    break;
                case Upgrades.Dash:
                    // Add upgrade to list of upgrades player has.
                    playerUpgrades.Add(Upgrades.Dash);
                    //Plays cool sound for picking up new power
                    FindObjectOfType<AudioManager>().Play("PowerPickup"); 
                    break;
                case Upgrades.Coin: AddCoin();
                    //Plays coind sound when picking up coins
                    FindObjectOfType<AudioManager>().Play("CoinPickup"); break;
            }

        }

        void Start()
        {
            respawnHint.enabled = false;
            respawnSet.enabled = false;
            DontDestroyOnLoad(gameObject);
            player = GetComponent<PlayerController>();
            SetCountText();
        }

        private void AddCoin()
        {
            playerCoins++;
            SetCountText();
        }

        void SetCountText()
        {
            countText.text = "" + playerCoins;
        }

        public void SetCoins(int amount)
        {
            playerCoins = amount;
        }

        public int GetCoins()
        {
            return playerCoins;
        }
        public Vector3 GetRespawnPoint()
        {
            return respawnPoint;
        }

        public int GetRespawnIndex()
        {
            return respawnIndex;
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            if (!col.CompareTag("Respawn") || !Input.GetButton("Interact")) return;
            StartCoroutine(DisplayHint(true));
            player.HealPlayer();
            FindObjectOfType<AudioManager>().Play("Shrine");
            respawnIndex = SceneManager.GetActiveScene().buildIndex;
            respawnPoint = transform.position;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Respawn")) return;
            StartCoroutine(DisplayHint(false));
        }

        private IEnumerator DisplayHint(bool activated)
        {
            if (!activated)
            {
                respawnHint.enabled = true;
                yield return new WaitForSeconds(3f);
                respawnHint.enabled = false;
            }
            else // hint doesn't work correctly yet. displays on top of another
            {
                respawnHint.enabled = false;
                respawnSet.enabled = true;
                yield return new WaitForSeconds(1.5f);
                respawnSet.enabled = false;
            }
        }
    }
}
