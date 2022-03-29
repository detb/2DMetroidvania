using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private static int playerCoins;
        private PlayerController player;


        public TextMeshProUGUI countText;
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
    }
}
