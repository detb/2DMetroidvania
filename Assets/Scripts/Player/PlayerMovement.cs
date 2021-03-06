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
        }

        private void FixedUpdate()
        {
            if(controller.IsFrozen()) return;
            controller.Move(horizontalMove * Time.deltaTime, jump);
            jump = false;
            
        }

        public float GetRunSpeed()
        {
            return runSpeed;
        }

        //Made only for sound
        void Walk()
        {
            FindObjectOfType<AudioManager>().Play("PlayerWalk");
        }

        void Jump()
        {
            FindObjectOfType<AudioManager>().Play("PlayerJump");
        }

        void Land()
        {
            FindObjectOfType<AudioManager>().Play("PlayerLand");
        }


    }
}
