using Player;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    private float originalY;
    public PlayerInventory.Upgrades upgrade;
    private PlayerInventory player;

    public float floatStrength = 0.1f;

    void Start()
    {
        player = FindObjectOfType<PlayerInventory>();
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (upgrade != PlayerInventory.Upgrades.Coin)
        {
            var position = transform.position;
            position = new Vector3(position.x, originalY + ((float)Mathf.Sin(Time.time) * floatStrength), position.z);
            transform.position = position;
        }
    }

    private void UpgradePlayer()
    {
        player.UpgradePlayer(upgrade);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        UpgradePlayer();
        Destroy(gameObject);
    }
}
