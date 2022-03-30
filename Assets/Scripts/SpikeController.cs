using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void HurtPlayer()
    {
        player.TakeDamage(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { 
            FindObjectOfType<AudioManager>().Play("SpikeTrap");
            HurtPlayer();
        }
           
    }
}
