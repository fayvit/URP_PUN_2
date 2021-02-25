using UnityEngine;
using FayvitEventAgregator;
using System.Collections.Generic;

namespace FayvitCam
{
    [System.Serializable]
    public class DirectionalCamera
    {
        [SerializeField] private CamFeatures features;

        private float x;
        private float y;
        private bool immediateFocusPosition = false;

        private InducedDirection dir = new InducedDirection();
        private Quaternion alvoQ;

        public Transform MyCamera
        {
            get { return features.MyCamera; }
        }

        public StateCam State { get; set; } = StateCam.controlable;

        #region Suprimido

        //void Focar()
        //{
        //    Quaternion alvoQ = Quaternion.LookRotation(caracteristicas.alvo.forward +
        //                                                caracteristicas.alturaAlvoDaCamera / 10 * Vector3.down);
        //    x = Mathf.LerpAngle(x, alvoQ.eulerAngles.y, caracteristicas.velocidadeMaxFoco * Time.deltaTime);
        //    y = Mathf.LerpAngle(y, alvoQ.eulerAngles.x, caracteristicas.velocidadeMaxFoco * Time.deltaTime);


        //    float paraContinha = Mathf.Min(Mathf.Abs(x - alvoQ.eulerAngles.y), Mathf.Abs(360 - Mathf.Abs(x - alvoQ.eulerAngles.y) % 360));


        //    if (paraContinha % 360 < 5 && Mathf.Abs(y - alvoQ.eulerAngles.x) % 360 < 15)
        //        EstadoAtual = EstadoDeCamera.controlando;

        //}

        //[System.NonSerialized] private Transform posCamHeroi;
        //[System.NonSerialized] private Transform posCamCriature;
        //private DirecaoInduzida dir = new DirecaoInduzida();


        //public Vector3 DirecaoInduzida(float h, float v)
        //{
        //    return dir.Direcao((estadoAtual == EstadoDeCamera.focando), MinhaCamera, h, v);
        //}

        //void SetarTransformsDeRetorno()
        //{
        //    Transform camera = caracteristicas.minhaCamera;

        //    if (caracteristicas.alvo.tag == "Player")
        //    {
        //        if (posCamHeroi == null)
        //        {
        //            posCamHeroi = new GameObject().transform;
        //            //posCamHeroi.parent = GameController.g.transform;
        //            posCamHeroi.name = "Transform de guardar heroi";
        //        }


        //        posCamHeroi.position = camera.position;
        //        posCamHeroi.rotation = posCamHeroi.rotation;
        //    }
        //    else if (caracteristicas.alvo.name == "CriatureAtivo")
        //    {
        //        if (posCamCriature == null)
        //        {
        //            posCamCriature = new GameObject().transform;
        //            //posCamCriature.parent = GameController.g.transform;
        //            posCamCriature.name = "Transform de guardar criature";
        //        }

        //        posCamCriature.position = camera.position;
        //        posCamCriature.rotation = camera.rotation;
        //    }
        //}
        #endregion

        public DirectionalCamera(CamFeatures car)
        {
            this.features = car;
        }

        public void SetStartFeaturesElements(Transform cam,Transform alvo)
        {
            this.features.Target = alvo;
            this.features.MyCamera = cam;
            immediateFocusPosition = true;
        }

        public void SetFeatures(
            CamFeatures car,
            Vector3 camPosition = default,
            Quaternion camRotation = default)
        {
            features = car;
            car.MyCamera.position = camPosition;
            car.MyCamera.rotation = camRotation;
        }

        public Vector3 SmoothInducedDirection(float h, float v)
        {
            if (dir == null)
                dir = new InducedDirection();

            if (dir != null && MyCamera != null)
                return dir.Direction(State == StateCam.inFocusing, MyCamera, h, v);
            else if (MyCamera)
                return MyCamera.TransformDirection(new Vector3(h, 0, v));
            else
                return new Vector3(h, 0, v);
        }

        public void RequireImmediateFocusPosition()
        {
            immediateFocusPosition = true;
        }

        void SetPositionAndRotation()
        {
            CamFeatures c = features;
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            c.MyCamera.rotation = rotation;

            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -c.sphericalDistance)
                + features.Target.position + (c.varVerticalHeightPoint + c.HeightCharacter) * Vector3.up;

            c.MyCamera.position = position;
        }

        void ImmediateFocusPosition()
        {
            immediateFocusPosition = false;
            CamFeatures c = features;
            Quaternion alvoQ = Quaternion.LookRotation(c.Target.forward + c.targetHeightForCam * Vector3.down);

            x = alvoQ.eulerAngles.y;
            y = alvoQ.eulerAngles.x;

            SetPositionAndRotation();

        }

        public void FocusInTheCamTarget(float vel)
        {
            CamFeatures c = features;
            
            c.contadorDeTempo += Time.deltaTime;

            if (c.distQ == 0)
            {
                alvoQ = Quaternion.LookRotation(c.Target.forward + c.targetHeightForCam * Vector3.down);
                c.distQ = Quaternion.Angle(alvoQ, c.StarterQ) * Mathf.PI  * c.sphericalDistance / 180;
            }
            
            Quaternion lerp = Quaternion.Lerp(c.StarterQ, alvoQ, c.contadorDeTempo / c.distQ*vel);

            
            x = lerp.eulerAngles.y;
            y = lerp.eulerAngles.x;
            y = ClampAngle(y, features.yMinLimit, features.yMaxLimit);


            if (c.contadorDeTempo > c.distQ / vel)
            {
                EventAgregator.Publish(new GameEvent(EventKey.controlableReached, features));
                State = StateCam.controlable;
            }

        }

        void IniciarFocarCamera(CamFeatures c)
        {
            Debug.Log("Iniciar focar camera");

            if (dir == null)
                dir = new InducedDirection();

            dir.OnStartFocus();

            c.StarterQ = c.MyCamera.rotation;
            c.contadorDeTempo = 0;
            c.distQ = 0;
        }
        

        public void ApplyCam(float moveX,float moveY,bool focar,bool autoAjust)
        {
            CamFeatures c = features;
            if (c.Target && c.MyCamera)
            {

                if (focar)
                {
                    IniciarFocarCamera(c);
                    State = StateCam.inFocusing;
                }
                else if (autoAjust && State == StateCam.controlable && c.velAutoAjust>0)
                {
                    State = StateCam.inAutoAjust;
                }
                else if (State == StateCam.inAutoAjust && !autoAjust)
                    State = StateCam.controlable;

                if (State == StateCam.controlable)
                    ControlableCam(moveX, moveY);
                else if (State == StateCam.inFocusing)
                    FocusInTheCamTarget(c.velToQ);
                else if (State == StateCam.inAutoAjust)
                {
                    IniciarFocarCamera(c);
                    FocusInTheCamTarget(c.velAutoAjust);
                }

                SetPositionAndRotation();

                if (immediateFocusPosition)
                {
                    ImmediateFocusPosition();
                    FayvitCameraSupport.ClearSmooth();
                }
                else
                    FayvitCameraSupport.DodgeWall(c.MyCamera, c.Target.position, c.varVerticalHeightPoint + c.HeightCharacter, true);
            }
        }

        void DebugPosition(Vector3 position)
        {
            Debug.DrawRay(position, Vector3.up, Color.yellow);
            Debug.DrawRay(position, Vector3.right, Color.yellow);
            Debug.DrawRay(position, Vector3.forward, Color.yellow);
        }

        

        public void ControlableCam(float X,float Y)
        {
            x += X * features.xSpeed * 0.02f;
            y -= Y * features.ySpeed * 0.02f;
            y = ClampAngle(y, features.yMinLimit, features.yMaxLimit);
        }

        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            if (angle > 180)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }

    [System.Serializable]
    public class CamFeatures
    {
        [HideInInspector] public float distQ = 0;
        [HideInInspector] public float contadorDeTempo = 0;

        public float velToQ = 25f;
        public float velAutoAjust = 12f;
        public float sphericalDistance = 7.0f;
        public float targetHeightForCam = 3.0f;
        public float varVerticalHeightPoint = 0;
        public float xSpeed = 125.0f;
        public float ySpeed = 60.0f;
        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;

        public Transform MyCamera { get; set; }
        public Transform Target { get; set; }
        public Quaternion StarterQ { get; set; }
        
        

        private CharacterController controll;

        public float HeightCharacter
        {
            get
            {
                if (!controll)
                    controll = Target.GetComponent<CharacterController>();
                return controll.height / 2;
            }
        }

    }

    public enum StateCam
    {
        inFocusing,
        controlable,
        @static,
        focusInAnother,
        inAutoAjust
    }
}