using System;
using System.Collections;
using Audio;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private static int playerCoins;
        private PlayerController player;
        private Vector3 respawnPoint;


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
            return true;
        }
        public void UpgradePlayer(Upgrades upgrade)
        {
            switch (upgrade)
            {
                case Upgrades.DoubleJump: 
                    player.extraJumpsValue = 2;
                    player.extraJumps = 2;

                    //Plays cool sound for picking up new power
                    FindObjectOfType<AudioManager>().Play("PowerPickup");
                    break;
                case Upgrades.Dash:
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
        private void OnTriggerStay2D(Collider2D col)
        {
            if (!col.CompareTag("Respawn") || !Input.GetButton("Interact")) return;
            StartCoroutine(DisplayHint(true));
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
