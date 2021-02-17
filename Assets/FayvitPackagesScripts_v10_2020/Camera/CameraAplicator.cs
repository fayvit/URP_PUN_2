using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FayvitCam
{
    public class CameraAplicator : MonoBehaviour
    {
        public static CameraAplicator cam;

        [SerializeField] private bool invetY;
        [SerializeField] private BasicCam basic;
        [SerializeField] private FightCam fightCam;
        [SerializeField] private ExhibitionistCam  cExibe;
        [SerializeField] private DirectionalCamera cDir;
        [SerializeField] private ShowSinglePointCam cShow;
        [SerializeField] private FocarAdversario focarAdv;
        [SerializeField] private float timeStandToAutoAdjustment = 1;
        [SerializeField,Tooltip("A camera deve ser setada no inspector ou no script")] 
        private ShakeCam shake;
        [ Header("Target Transform"),SerializeField] private Transform target;

        private float mouseX = 0, 
                      mouseY = 0, 
                      contTimeStand = 0;
        private bool cameraFocus = false,
                     autoAdjust = false;


        public enum EstiloDeCamera
        {
            onFree,
            fight,
            showAnother,
            focusingPoint,
            basic,
            cameraOff
        }

        public BasicCam Basic
        {
            get { return basic; }
            private set { basic = value; }
        }

        public DirectionalCamera Cdir
        {
            get { return cDir; }
        }

        public ShowSinglePointCam Cshow { get => cShow; }

        public EstiloDeCamera Style { get; private set; } = EstiloDeCamera.onFree;

        // Use this for initialization
        void Start()
        {
            cam = this;
            cDir.SetStartFeaturesElements(transform,target);
            FayvitCamEventAgregator.AddListener(FayvitCamEventKey.requestShakeCamera, OnRequestShakeCam);
            FayvitCamEventAgregator.AddListener(FayvitCamEventKey.controlableReached,OnControlableReached );
        }

        private void OnDestroy()
        {
            FayvitCamEventAgregator.RemoveListener(FayvitCamEventKey.requestShakeCamera, OnRequestShakeCam);
            FayvitCamEventAgregator.RemoveListener(FayvitCamEventKey.controlableReached, OnControlableReached);
        }

        private void OnControlableReached(IFayvitCamEvent obj)
        {
            contTimeStand = 0;
        }

        private void OnRequestShakeCam(IFayvitCamEvent obj)
        {
            ShakeAxis ax = ShakeAxis.y;
            int totalShake = 5;
            float shakeAngle = 1;

            if (obj.MySendObjects.Length > 1)
            {
                ax = (ShakeAxis)obj.MySendObjects[1];
                if (obj.MySendObjects.Length > 2)
                {
                    totalShake = (int)obj.MySendObjects[2];
                    if (obj.MySendObjects.Length > 3)
                        shakeAngle = (float)obj.MySendObjects[3];
                }
            }

            shake.IniciarShake(ax, totalShake, shakeAngle);
        }

        public void ValoresDeCamera(float mouseX, float mouseY, bool cameraFocus,bool inMove)
        {
            this.mouseX = mouseX;
            this.mouseY = mouseY * (invetY ? -1 : 1 );
            this.cameraFocus = cameraFocus;

            if (mouseX == 0 && mouseY == 0 && inMove)
                contTimeStand += Time.deltaTime;
            else 
                contTimeStand = 0;

            autoAdjust = contTimeStand > timeStandToAutoAdjustment && inMove;
        }

        //public void SetarInimigosProximosParaFoco(List<GameObject> listG)
        //{
        //    focarAdv.OsPerto = listG;
        //}

        // Update is called once per frame
        void LateUpdate()
        {
            //  if (!GameController.g.HudM.MenuDePause.EmPause)
            switch (Style)
            {
                case EstiloDeCamera.onFree:
                    if (cDir != null)
                        cDir.ApplyCam(mouseX,mouseY,cameraFocus, autoAdjust);//basica.Update();
                break;
                case EstiloDeCamera.fight:
                    focarAdv.Focar(transform,fightCam.T_Enemy, 0);
                    fightCam.Update();
                break;
                case EstiloDeCamera.showAnother:
                    cExibe.ShowAnother();
                break;
                case EstiloDeCamera.basic:
                    basic.Update();
                break;
            }

            shake.Update();
        }

        public void RetornarParaCameraDirecional()
        {
            focarAdv.RemoveMira();
            Style = EstiloDeCamera.onFree;
        }

        public void FocusBasicCam(Transform T, float height, float distance)
        {
            cDir.SetFeatures(new CamFeatures()
            {
                Target = T,
                MyCamera = transform,
                targetHeightForCam = height,
                sphericalDistance = distance
            });
            Style = EstiloDeCamera.onFree;
        }

        public void StartExibitionCam(CharacterController target)
        {
            StartExibitionCam(target.transform,
                target.GetComponent<CharacterController>().height);
        }

        public void StartExibitionCam(Transform target, float height, bool dodgeWall = false)
        {
            cExibe.SetExhibitionElements(transform, target, height, dodgeWall);//cExibe = new ExhibitionistCam (transform, target, height, dodgeWall);
            Style = EstiloDeCamera.showAnother;
        }
        public void StartFightCam(Transform target,Transform enemy)
        {
            fightCam.Start(transform, target);
            fightCam.T_Enemy = enemy;
            Style = EstiloDeCamera.fight;
        }

        public void StartFightCam(Transform target,float height, float distance,Transform enemy)
        {
            fightCam.Start(transform, target, height, distance);
            fightCam.T_Enemy = enemy;
            Style = EstiloDeCamera.fight;
        }

        public void StartShowPointCamera(Transform target)
        {
            StartShowPointCamera(target, Cshow.Prop);
        }

        public void StartShowPointCamera(Transform target,SinglePointCameraProperties S)
        {
            Style = EstiloDeCamera.focusingPoint;
            cShow.SetExhibitionElements(transform,target,S);
        }
        public bool FocusInPoint(float horizontalDistance=6,float height=1)
        {
            return cShow.ShowFixed(horizontalDistance, height);
        }

        public void NewFocusForBasicCam(Transform target, float height, float distance, bool dodgeWall = false, bool dirOfObj = false)
        {
            Style = EstiloDeCamera.basic;
            Cdir.State = StateCam.@static;
            basic.Start(transform);
            Basic.NewFocus(target, height, distance, dodgeWall, dirOfObj);
        }

        public void OffCamera()
        {
            Style = EstiloDeCamera.cameraOff;
        }

        public Vector3 SmoothCamDirectionalVector(Vector3 inputDirection)
        {
            return SmoothCamDirectionalVector(inputDirection.x, inputDirection.z);
        }

        public Vector3 SmoothCamDirectionalVector(float h, float v)
        {
            Vector3 forward = cDir.SmoothInducedDirection(h, v);

            forward.y = 0;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            return (h * right + v * forward);
        }

        #region Suprimido
        //public bool FocusInPoint(Transform target, 
        //    Vector3 deslCamFocus,
        //    float velOrTime,//velocidadeTempoDefoco --> bom verificar qual é o verdadeiro
        //    float distance = 6,
        //    float height = -1,
        //    bool withTime = false)
        //{
        //    return FocusInPoint(target,velOrTime, distance, height, withTime, default(Vector3), deslCamFocus, false,true);
        //}

        //public bool FocusInPoint(Transform target,
        //    float velOrTime,
        //    float distance = 6,
        //    float height = -1,
        //    bool withTime = false,
        //    Vector3 startDir = default(Vector3),
        //    Vector3 deslCamFocus = default(Vector3),
        //    bool focusOfTransform = false,
        //    bool dodgeWall = true
        //    )
        //{
        //    Style = EstiloDeCamera.focusingPoint;
        //    cShow.SetExhibitionElements(transform, target, dodgeWall);
        //    return cShow.ShowFixed(velOrTime, distance, height, withTime, startDir, focusOfTransform, deslCamFocus);
        //}

        //public bool FocusInPoint(Transform target, SinglePointCameraProperties S)
        //{
        //    return FocusInPoint(target, S.velOrTimeFocus, S.distance, S.height, S.withTime, S.startPosition, S.deslCamFocus, S.transformFocus, S.dodgeCam);
        //}
        #endregion
    }
}