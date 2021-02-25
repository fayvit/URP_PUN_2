using UnityEngine;
using System.Collections;

namespace FayvitCam
{
    [System.Serializable]
    public class BasicCam
    {

        [SerializeField] private Transform target;
        [SerializeField] private float height = 20;
        [SerializeField] private float horizontalDistance = 20;
        [SerializeField] private float camVel = 10;
        [SerializeField] private float frontDistanceForFocus = 2;

        private Transform transform;
        private Vector3 targetDir;
        private bool dodgeWall = false;
        private bool dirOfObj = false;
        private float LerpVel = 1;


        // Use this for initialization
        public void Start(Transform cameraTransform)
        {
            transform = cameraTransform;

            if (!this.target)
            {
                GameObject gOfTarget = GameObject.FindGameObjectWithTag("Player");
                if (gOfTarget)
                    this.target = gOfTarget.transform;
            }

            if (this.target)
            {
                targetDir = this.target.position - horizontalDistance * Vector3.forward + height * Vector3.up;
                transform.position = targetDir;
                transform.LookAt(this.target.position + frontDistanceForFocus * Vector3.forward);
            }
        }

        // Update is called once per frame
        public void Update()
        {
            Vector3 dirCamera = Vector3.forward;
            if (dirOfObj)
                dirCamera = -this.target.forward;
            targetDir = target.position - horizontalDistance * dirCamera + height * Vector3.up;

            LerpVel = camVel * Mathf.Max(1,
                Vector3.Distance(targetDir, transform.position) / Mathf.Sqrt(Mathf.Pow(height, 2) + Mathf.Pow(horizontalDistance, 2)
                ));            

            if (dodgeWall && Physics.Linecast(targetDir, target.position))
            {
                transform.position = targetDir;
                FayvitCameraSupport.DodgeWall(transform, target.position, 1, true, false);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position,
                    targetDir
                    , LerpVel * Time.deltaTime);
            }
        }

        public void NewFocus(Transform target, float height, float distance, bool dodgeWall, bool forwardOfObj)
        {
            this.height = height;
            this.horizontalDistance = distance;
            this.target = target;
            this.dodgeWall = dodgeWall;
            this.dirOfObj = forwardOfObj;

            if (transform == null)
                transform = Camera.main.transform;

            Vector3 dirCamera = Vector3.forward;

            if (forwardOfObj)
                dirCamera = -this.target.forward;

            transform.rotation = Quaternion.LookRotation(this.target.position + frontDistanceForFocus * dirCamera -
                (target.position - horizontalDistance * dirCamera + height * Vector3.up));

            Update();
        }
    }
}