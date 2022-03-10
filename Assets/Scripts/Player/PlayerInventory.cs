using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private int playerCoins;
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
                    break;
                case Upgrades.Dash: break;
                case Upgrades.Coin: AddCoin(); break;
            }
        }

        void Start()
        {
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
