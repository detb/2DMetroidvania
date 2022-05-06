using Audio;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace Enemies
{
    public class Spell : MonoBehaviour
    {
        private Animator enemyAnimator;
        private AIPlayerDetector playerDetector;
        private AISpellCastDetector spellCastDetector;
        private Transform player;
        private AudioManager am;

        [Header("Player")]
        [SerializeField]
        private LayerMask playerLayer;

        [Header("Health")]
        [SerializeField]
        private int maxHealth = 100;
        private int currentHealth;

        [Header("Spell variables")]
        [SerializeField]
        private Transform spellPoint;
        [SerializeField]
        private float spellRange = 0.5f;
        [SerializeField]
        private float castSpeed = 4f;
        [SerializeField]
        private int spellDamage;
        private float castTime = 0f;

        // Static triggers/bools/values for animations hashed
        private static readonly int PlayerDetected = Animator.StringToHash("playerDetected");
        private static readonly int Spell1 = Animator.StringToHash("spell");

        // Start is called before the first frame update
        void Start()
        {
            am = FindObjectOfType<AudioManager>();
            player = GameObject.Find("Player").transform;
            enemyAnimator = GetComponent<Animator>();
            playerDetector = GetComponent<AIPlayerDetector>();
            spellCastDetector = GetComponent<AISpellCastDetector>();
            currentHealth = maxHealth;
        }

        void Update()
        {
            if (playerDetector.playerDetected)
                enemyAnimator.SetBool(PlayerDetected, true);
            else if (!playerDetector.playerDetected)
                enemyAnimator.SetBool(PlayerDetected, false);


            if (Time.time >= castTime)
            {
                if (spellCastDetector.playerDetected)
                {
                    castTime = Time.time + 2f / castSpeed;
                    enemyAnimator.SetTrigger(Spell1);
                }
            }
        }


        private void OnDrawGizmosSelected()
        {
            if (spellPoint == null)
                return;
            Gizmos.DrawWireSphere(spellPoint.position, spellRange);
        }


        //Made for enemy sounds
        void EnemySound(string sound)
        {
            am.Play(sound);
        }

        public void LightningSpell()
        {
            Collider2D playerHit = Physics2D.OverlapCircle(spellPoint.position, spellRange, playerLayer);
            if (playerHit != null)
            {
                playerHit.GetComponent<PlayerController>().TakeDamage(spellDamage);
                Destroy(gameObject, 5f);
            }
        }
    }
}