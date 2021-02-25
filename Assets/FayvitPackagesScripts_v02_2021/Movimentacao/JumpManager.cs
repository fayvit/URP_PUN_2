using UnityEngine;
using FayvitEventAgregator;
using System;

namespace FayvitMove
{
    [System.Serializable]
    public class JumpManager
    {
        private JumpFeatures features;
        private Transform transform;
        private CharacterController controle;
        private Vector3 verticalMove = Vector3.zero;
        private float lastGroundedY = 0;
        private float timeInJump = 0;
        private float timeOfRising = 0;
        private bool isRising = false;


        public JumpManager(JumpFeatures caracteristicas,Transform T,CharacterController c )
        {
            this.features = caracteristicas;
            transform = T;
            controle = c;
        }

        public bool isJumping
        {
            get { return features.isJumping; }
        }

        public void StartFall()
        {
            features.isJumping = true;
            features.wasJumping = true;
        }

        public void StartApplyJump()
        {
            lastGroundedY = transform.position.y;
            features.isJumping = true;
            controle.Move(Vector3.up * features.initialImpulse);
            EventAgregator.Publish(new GameEvent(EventKey.animateStartJump,controle.gameObject));
        }

        public void UpdateJump(Vector3 moveDirection, bool isGrounded, bool jump)
        {

            VerifyIsWasJump();

            if (
                isRising == true
                &&
                transform.position.y - lastGroundedY < features.jumpHeight
                &&
                timeInJump < features.maxTimeJump
                &&
                jump
                )
            {

                RisingJump(moveDirection);

            }
            else if (
              (transform.position.y - lastGroundedY >= features.jumpHeight
                ||
                timeInJump >= features.maxTimeJump
                || !jump
                )
              &&
              isRising == true
              )
            {
                KeyOfJumpTransition();
            }
            else if (isRising == false)
            {

                FallingJump(moveDirection, isGrounded);

            }
        }

        void VerifyIsWasJump()
        {
            if (features.wasJumping == false && features.isJumping == true)
            {
                timeInJump = 0;
                isRising = true;
            }

            features.wasJumping = features.isJumping;
        }

        void RisingJump(Vector3 direcaoMovimento)
        {
            timeInJump += Time.deltaTime;
            
            verticalMove = (direcaoMovimento * features.inJumpSpeed
                + Vector3.up * features.risingSpeed);
            controle.Move(verticalMove * Time.deltaTime);
        }

        void KeyOfJumpTransition()
        {
            timeOfRising = timeInJump;
            isRising = false;
            controle.Move(verticalMove * Time.deltaTime);
        }

        void FallingJump(Vector3 direcaoMovimento,bool noChao)
        {
            timeOfRising += Time.deltaTime;
            float amortecimento = features.verticalDamping;

            verticalMove = FallingHorizontalMove(direcaoMovimento)
                + FallingVerticalMove(amortecimento);


            controle.Move(verticalMove * Time.deltaTime);

            if (noChao && timeOfRising > features.minTimeJump)
                NotJumping();
        }

        Vector3 FallingVerticalMove(float damping)
        { 
        return new Vector3(0,
                        Mathf.Lerp(verticalMove.y, -features.fallSpeed, damping * Time.deltaTime),
                    0);
        }

        Vector3 FallingHorizontalMove(Vector3 moveDirection)
        {
            Vector3 V = new Vector3(verticalMove.x, 0, verticalMove.z);
            Vector3 V2 = new Vector3(moveDirection.x, 0, moveDirection.z) * features.inJumpSpeed;
            return Vector3.Lerp(V, V2, features.horizontalDamping * Time.deltaTime);
        }

        public void NotJumping()
        {

            if (features.isJumping)
                EventAgregator.Publish(new GameEvent(EventKey.animateDownJump,controle.gameObject));

            features.isJumping = false;
            features.wasJumping = false;
            
            verticalMove = Vector3.zero;
        }
    }
}