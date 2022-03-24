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
            controller.Move(horizontalMove * Time.deltaTime, jump);
            jump = false;
        }

        public float GetRunSpeed()
        {
            return runSpeed;
        }
    }
}
