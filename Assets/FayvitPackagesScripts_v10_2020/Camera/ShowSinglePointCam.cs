using UnityEngine;
using System.Collections;

namespace FayvitCam
{
    [System.Serializable]
    public class ShowSinglePointCam
    {

        [SerializeField] private Transform transform;
        [SerializeField] private Transform target;

        [SerializeField] private SinglePointCameraProperties prop;
        private float timeCount = 0;
        private float distance = 0;
        private Vector3 startPosition;
        private Vector3 startRotation;

        public SinglePointCameraProperties Prop { get => prop; }

        public void SetExhibitionElements(Transform daCamera, Transform doFoco, SinglePointCameraProperties S)
        {
            prop = S;
            timeCount = 0;
            distance = 0;
            transform = daCamera;
            target = doFoco;
            startPosition = default;
            startRotation = default;
        }

        public bool TwoPoint(Vector3 camPoint, Vector3 targetPoint)
        {
            if (startPosition == default)
            {
                startPosition = transform.position;
                startRotation = transform.forward;
            }

            float lerp;
            if (!prop.withTime)
            {
                if (distance == 0)
                    distance = Vector3.Distance(startPosition, camPoint);
                lerp = timeCount * prop.velOrTimeFocus / distance;
            }
            else
            {
                lerp = timeCount / prop.velOrTimeFocus;
            }

            timeCount += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, camPoint, lerp);
            Vector3 forward = Vector3.Lerp(startRotation,targetPoint-camPoint,lerp);
            transform.rotation = Quaternion.LookRotation(forward);
            //transform.LookAt(targetPoint + prop.deslCamFocus);

            if (prop.dodgeCam)
                FayvitCameraSupport.DodgeWall(transform, target.position, 1, true);

            if (!prop.withTime && timeCount > distance / prop.velOrTimeFocus)
            {
                return true;
            }
            else if (prop.withTime && timeCount > prop.velOrTimeFocus)
                return true;

            return false;
        }

        public bool ShowFixed(float horizontalDistance = 6, float height = 1)
        {

            Vector3 posAlvo = target.position + target.forward * horizontalDistance + Vector3.up * height;

            return TwoPoint(posAlvo, target.position);

            //    if (prop.startPosition == default)
            //        prop.startPosition = transform.position;

            //    float lerp;
            //    if (!prop.withTime)
            //    {
            //        if (distance == 0)
            //            distance = Vector3.Distance(prop.startPosition, posAlvo);
            //        lerp = timeCount * prop.velOrTimeFocus / distance;
            //    }
            //    else
            //    {
            //        lerp = timeCount / prop.velOrTimeFocus;
            //    }

            //    timeCount += Time.deltaTime;


            //    transform.position = Vector3.Lerp(prop.startPosition, posAlvo, lerp);

            //    transform.LookAt(target.position + prop.deslCamFocus);

            //    if (prop.dodgeCam)
            //        FayvitCameraSupport.DodgeWall(transform, target.position, 1, true);

            //    if (!prop.withTime && timeCount > distance / prop.velOrTimeFocus)
            //    {
            //        return true;
            //    }
            //    else if (prop.withTime && timeCount > prop.velOrTimeFocus)
            //        return true;


            //    return false;
            //}
        }
    }

    [System.Serializable]
    public struct SinglePointCameraProperties
    {
        public float velOrTimeFocus;
        public float characterHeight;
        public bool withTime;
        public bool dodgeCam;
        public Vector3 deslCamFocus;

        public SinglePointCameraProperties(float velOrTimeFocus)
        {
            this.velOrTimeFocus = velOrTimeFocus;
            withTime = false;
            deslCamFocus = default;
            dodgeCam = false;
            characterHeight = 0;
        }
    }
}