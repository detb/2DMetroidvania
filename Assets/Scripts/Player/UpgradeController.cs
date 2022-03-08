using System.Collections;
using System.Collections.Generic;
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
            transform.position = new Vector3(transform.position.x,
            originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
            transform.position.z);
        }
    }

    public void UpgradePlayer()
    {
        player.UpgradePlayer(upgrade);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            UpgradePlayer();
            Destroy(gameObject);
        }
    }
}
