using UnityEngine;

namespace Enemies
{
    public class SkeletonRun : StateMachineBehaviour
    {
        private Transform player;
        private Rigidbody2D rb;
        private Enemy enemy;

        public float speed = 2f;

        // Static triggers/bools/values for animations hashed
        private static readonly int Attack = Animator.StringToHash("attack");

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            enemy = animator.GetComponent<Enemy>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetComponent<AIPlayerDetector>().playerDetected)
            {
                enemy.LookAtPlayer();

                var position = rb.position;
                Vector2 target = new Vector2(player.position.x, position.y);
                Vector2 newPos = Vector2.MoveTowards(position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger(Attack);
        }
    }
}
