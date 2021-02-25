using UnityEngine;
using System.Collections;

namespace FayvitCam
{
    [System.Serializable]
    public class FightCam
    {
        [SerializeField] private Transform tEnemy;
        [SerializeField] private Transform target;
        [SerializeField] private float height = 1;
        [SerializeField] private float velMaxFocus = 10f;
        [SerializeField] private float distance = 6.0f;
        [SerializeField] private float focusVerticalVar = 3;

        private float x = 0;
        private float y = 0;

        private Transform transform;

        public Transform T_Enemy
        {
            get { return tEnemy; }
            set { tEnemy = value; }
        }

        public void Start(Transform aCamera, Transform alvo)
        {
            transform = aCamera;
            this.target = alvo;
            x = transform.rotation.eulerAngles.y;
            y = transform.rotation.eulerAngles.x;
        }

        // Use this for initialization
        public void Start(Transform aCamera, Transform alvo, float altura, float distancia,float escalaA = 1)
        {
            transform = aCamera;
            this.target = alvo;

            this.height = altura;
            this.distance = distancia;

            CharacterController controll = alvo.GetComponent<CharacterController>();
            if (controll)
                this.focusVerticalVar = controll.height+ escalaA;
            else
                focusVerticalVar = escalaA;
        }

        // Update is called once per frame
        public void Update()
        {
            if (tEnemy && target && transform)
                FightFocus();
            else
                Debug.LogAssertion("transforms não setados corretamente, inimigo = " + tEnemy + ", alvo= " + target + ", camera = " + transform);
        }

        void FightFocus()
        {

            Vector3 direcaoDaVisao
                = Vector3.ProjectOnPlane(tEnemy.position - transform.position, Vector3.up);

            Quaternion alvoQ = Quaternion.LookRotation(direcaoDaVisao +
                                                       height / 10 * Vector3.down);

            x = Mathf.LerpAngle(x, alvoQ.eulerAngles.y, velMaxFocus * Time.deltaTime);
            y = Mathf.LerpAngle(y, alvoQ.eulerAngles.x, velMaxFocus * Time.deltaTime);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position
                + (focusVerticalVar + height / 8) * Vector3.up;

            transform.rotation = Quaternion.Lerp(transform.rotation,
                                rotation,
                                               50 * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position,
                                            position,
                                            50 * Time.deltaTime);

            FayvitCameraSupport.DodgeWall(transform, target.position, focusVerticalVar);

        }




    }
}