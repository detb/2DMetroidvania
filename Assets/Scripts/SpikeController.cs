using Audio;
using Player;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private PlayerController player;

    void Start()
    {
        // Setting sleepmode to neversleep, to register hit better.
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        player = FindObjectOfType<PlayerController>();
    }

    void HurtPlayer()
    {
        player.TakeDamage(20);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        FindObjectOfType<AudioManager>().Play("SpikeTrap");
        HurtPlayer();

    }
}
