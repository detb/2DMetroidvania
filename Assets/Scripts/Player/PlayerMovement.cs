using Audio;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("PlayerController and speed")]
            [SerializeField]
            private PlayerController controller;
            [SerializeField]
            private float runSpeed;
        
        
        private float horizontalMove = 0f;
        private bool jump = false;

        // Update is called once per frame
        void Update()
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
            //Jump sound
            if (jump) {
                FindObjectOfType<AudioManager>().Play("PlayerJump");
            }
        }

        private void FixedUpdate()
        {
            if(controller.IsFrozen()) return;
            controller.Move(horizontalMove * Time.deltaTime, jump);
            jump = false;
            
        }

        //Made only for player walk sound
        void Walk() {
            FindObjectOfType<AudioManager>().Play("PlayerWalk");
        }

        public float GetRunSpeed()
        {
            return runSpeed;
        }
    }
}
