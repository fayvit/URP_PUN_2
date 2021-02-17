using UnityEngine;
using System.Collections;

namespace FayvitCam
{
    [System.Serializable]
    public class ExhibitionistCam 
    {
        [SerializeField] private Transform transform;
        [SerializeField] private Transform target;
        [SerializeField] private float horizontalDistance = 8;
        [SerializeField] private float verticalDistance = 5;
        [SerializeField] private float rotationVel = .1f;

        private float characterHeight;

        private float timeCount = 0;
        private bool dodgeWall = false;
        private Vector3 startDir;

        public void ResetTimeCount()
        {
            timeCount = 0;
        }

        public ExhibitionistCam (Transform daCamera, Transform doFoco, float height, bool dodgeWall = false)
        {

            SetExhibitionElements(daCamera, doFoco, height, dodgeWall);
        }

        public void SetExhibitionElements(Transform daCamera, Transform doFoco, float height, bool dodgeWall = false)
        {

            this.dodgeWall = dodgeWall;
            ResetTimeCount();
            transform = daCamera;
            target = doFoco;
            characterHeight = height;

            RequestCamPosition();
        }

        void RequestCamPosition()
        {
            startDir = Vector3.ProjectOnPlane(transform.position - target.position, Vector3.up).normalized;

            transform.position = target.position
                    + horizontalDistance * startDir
                    + (verticalDistance + characterHeight) * Vector3.up;            
        }

        Vector3 TentativaDePosition()
        {

            Vector3 V = target.position
                + Quaternion.AngleAxis(rotationVel*timeCount , Vector3.up) * startDir * horizontalDistance
                + (verticalDistance + characterHeight) * Vector3.up;

            timeCount += Time.deltaTime;
            return V;
        }

        public void ShowAnother()
        {

            transform.position = TentativaDePosition();
            transform.LookAt(target);

            if(dodgeWall)
                FayvitCameraSupport.DodgeWall(transform, target.position, characterHeight, true);
            
         
        }
    }
}