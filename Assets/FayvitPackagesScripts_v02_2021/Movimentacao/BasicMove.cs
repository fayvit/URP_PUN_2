using UnityEngine;
using FayvitSupportSingleton;
using FayvitEventAgregator;
using System.Collections;

namespace FayvitMove
{
    [System.Serializable]
    public class BasicMove
    {
        //public delegate void Acoes();

        [SerializeField] private Transform lockTarget;
        [SerializeField] private MoveFeatures movFeatures;
        [SerializeField] private float standardFallSpeed = 0;
        [SerializeField] private float overlapTaxRadius = .8f;

        //[SerializeField] private ElementosDeMovimentacao elementos;


        private Transform transform;
        private Vector3 directionalMove = Vector3.zero;
        

        private float targetSpeed = 0;
        private bool retornoDonoChao;
        private bool isGrounded;
        private bool wasGrounded;
        //private float groundedRadius = .1f;
        private Transform groundCheck;

        public bool ApplicableGravity { get; set; } = true;

        public CharacterController Controller { get; private set; }

        public JumpManager _JumpM { get; private set; }

        public Transform LockTarget { get => lockTarget; set => lockTarget = value; }

        public BasicMove() { }
        public BasicMove(MoveFeatures movFeatures,float standardFallSpeed=1,float overlapTaxRadius = .9f) 
        {
            this.movFeatures = movFeatures;
            this.standardFallSpeed = standardFallSpeed;
            this.overlapTaxRadius = overlapTaxRadius;
        }

        public void StartFields(Transform T)
        {
            transform = T;
            Controller = T.GetComponent<CharacterController>();
            _JumpM = new JumpManager(movFeatures.jumpFeat, transform, Controller);
        }

        #region Suprimidos
        //public bool NoChao(float distanciaFundamentadora)
        //{
        //    if (Time.timeScale > 0)
        //        noChao = noChaoS(elementos.controle, distanciaFundamentadora);

        //    return noChao;
        //}

        //public static bool noChaoS(CharacterController controle, float erroDeAncora, bool especial = false)
        //{

        //    if (especial)
        //    {
        //        return
        //            Physics.Raycast(controle.transform.position, Vector3.down, erroDeAncora);
        //    }
        //    else
        //    {

        //        //CharacterController controle =  GetComponent<CharacterController>();
        //        CollisionFlags collisionFlags = controle.collisionFlags;
        //        bool um = (collisionFlags & CollisionFlags.CollidedBelow) != 0;
        //        return um ||
        //        Physics.Raycast(controle.transform.position, Vector3.down, erroDeAncora);
        //    }
        //}

        //public void Fundamentador(bool comControle = false)
        //{
        //    if (!noChaoS(elementos.controle, 0.01f) && !caracMov.caracPulo.estouPulando)
        //        velocidadeDescendo = Mathf.Lerp(velocidadeDescendo, caracMov.caracPulo.velocidadeDescendo, 25 * Time.deltaTime);
        //    else
        //        velocidadeDescendo = Mathf.Lerp(velocidadeDescendo, 0, 25 * Time.deltaTime);


        //    if (comControle)
        //        elementos.controle.Move(velocidadeDescendo * Vector3.down * Time.deltaTime);
        //}

        //public void AplicaGravidade()
        //{
        //    AplicaGravidade(9.8f, 5);
        //}

        //public void AplicaGravidade(float velMax, float amortecimento)
        //{
        //    velocidadeDescendo = Mathf.Lerp(velocidadeDescendo, velMax, amortecimento * Time.deltaTime);
        //    controle.Move((direcaoMovimento * targetSpeed + velocidadeDescendo * Vector3.down) * Time.deltaTime);
        //}

        //public void AplicaPulo(Vector3 direcaoAlvo)
        //{
        //    pulo.VerificaPulo(direcaoAlvo,NoChao);
        //}
        #endregion

        private Transform GroundCheck
        {
            get
            {
                if (groundCheck == null)
                {
                    if (Controller != null)
                    {
                        groundCheck = new GameObject().transform;
                        groundCheck.position = Controller.transform.position
                            + Controller.center + .5f * Controller.height * Vector3.down;
                        groundCheck.SetParent(Controller.transform);
                    }
                }

                return groundCheck;
            }
        }

        
        public bool IsGrounded
        {
            get
            {
                // bool noChao = false;
                isGrounded = false;
                Vector3 overCapsPos = GroundCheck.position + Controller.height * .25f * Vector3.up;
                Collider[] colliders = Physics.OverlapCapsule(GroundCheck.position, overCapsPos, Controller.radius * overlapTaxRadius, 1);
                    //Physics.OverlapSphere(GroundCheck.position, groundedRadius, 1);
                

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (Controller.transform.gameObject.name == "teste")
                        Debug.Log(colliders[i].name);

                    if (colliders[i].gameObject != Controller.transform.gameObject)
                    {
                        //noChao = true;
                        retornoDonoChao = true;
                        isGrounded = true;
                        wasGrounded = true;
                    }
                }

                if (!isGrounded && wasGrounded)
                {
                    wasGrounded = false;
                    retornoDonoChao = true;
                    SupportSingleton.Instance.StartCoroutine(GhostJumpSupport());


                }
                return retornoDonoChao;
            }
        }

        IEnumerator GhostJumpSupport()
        {
            yield return new WaitForSeconds(0.1f);
            if (!isGrounded && !wasGrounded)
                retornoDonoChao = false;
        }

        public void MoveApplicator(Vector3 dir, bool run = false, bool startJump = false , bool pressJump = false)
        {

            bool grounded = IsGrounded;

            dir = (dir != Vector3.zero) ? dir.normalized *Mathf.Max(dir.magnitude,1) : Vector3.zero;

            
            if ((grounded || !ApplicableGravity) && !startJump && !_JumpM.isJumping)
            {
                UpdateMove(dir, run);
            }
            else if (grounded && startJump && !_JumpM.isJumping)
            {
                _JumpM.StartApplyJump();
            }
            else if (_JumpM.isJumping)
            {
                _JumpM.UpdateJump(dir, IsGrounded, pressJump);
            }
            else if (ApplicableGravity)
            {
                EventAgregator.Publish(new GameEvent(EventKey.animateFall,transform.gameObject));
                _JumpM.StartFall();
                //AplicaGravidade();
            }
        }

        void UpdateMove(Vector3 targetDirection, bool run = false)
        {
            if (_JumpM == null)
                _JumpM = new JumpManager(movFeatures.jumpFeat, transform, Controller);

            //pulo.NaoEstouPulando();
            targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);



            targetSpeed *= run ?
                movFeatures.runSpeed :
                movFeatures.walkSpeed;


            if (targetDirection != Vector3.zero)
            {

                if (Controller.velocity.magnitude < movFeatures.walkSpeed &&!movFeatures.rotAlways /*&& VerificaChao.noChao(elementos.controle, elementos.transform)*/)
                {
                    directionalMove = targetDirection.normalized;
                }
                else
                {
                    directionalMove = Vector3.RotateTowards(directionalMove, targetDirection, 500 * Mathf.Deg2Rad * Time.deltaTime, 1000);

                    directionalMove = directionalMove.normalized;
                }
            }
            else
            {
                directionalMove = Vector3.Lerp(directionalMove, Vector3.zero, 1);
            }

            if (LockTarget)
            {
                if (targetDirection.sqrMagnitude > 0.25f)
                {
                    Vector3 lookAt = LockTarget.position - transform.position;
                    lookAt.y = 0;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookAt), 30 * Time.deltaTime);
                }
            }
            else
            if (targetDirection.magnitude > 0.3f)
                transform.rotation = Quaternion.LookRotation(new Vector3(directionalMove.x, 0, directionalMove.z));

            Controller.Move((directionalMove * targetSpeed + standardFallSpeed * Vector3.down) * Time.deltaTime);

            EventAgregator.Publish(
                new GameEvent(
                    EventKey.changeMoveSpeed, 
                    Controller.gameObject, 
                    Controller.velocity,
                    lockTarget
                    ));

        }

        public void VerifyJumpInput()
        {
            _JumpM.StartApplyJump();
        }

        public void ChangeWalkSpeed(float newSpeed)
        {
            movFeatures.walkSpeed = newSpeed;
        }

    }

    [System.Serializable]
    public class MoveFeatures : System.ICloneable
    {
        public float walkSpeed = 6;
        public float runSpeed = 12;
        public bool rotAlways = false;
        public JumpFeatures jumpFeat;

        public object Clone()
        {
            return new MoveFeatures()
            {
                walkSpeed = this.walkSpeed,
                runSpeed = this.runSpeed,
                jumpFeat = this.jumpFeat
            };
        }
    }

    [System.Serializable]
    public class JumpFeatures
    {
        public float jumpHeight = 2;
        public float minTimeJump = .1f;
        public float maxTimeJump = 1;
        public float risingSpeed = 3;
        public float fallSpeed = 4.8f;
        public float inJumpSpeed = 6;
        public float verticalDamping = 3f;
        public float horizontalDamping = 5f;
        public float initialImpulse = 0.3f;
        [HideInInspector] public bool isJumping = false;
        [HideInInspector] public bool wasJumping = false;
    }
}