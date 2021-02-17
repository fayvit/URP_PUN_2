using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FayvitCam
{
    public class FayvitCameraSupport
    {

        private static bool estavaAcionado = false;
        private static float tempoAvancando = 0;
        private static float totalTimeIn = .25f;
        private static float totalTimeOut = .5f;
        private static Vector3 startPos;
        private static Vector3 antPos;

        public static List<string> IgnoreTags = new List<string>()
        {
            "Player","Criature","desvieCamera"
        };

        public static bool VerifyTags(RaycastHit raioColisor)
        {
            return raioColisor.transform.tag != "Player"
                       &&
                       raioColisor.transform.tag != "Criature"
                       &&
                       raioColisor.transform.tag != "desvieCamera";
        }

        public static void ClearSmooth()
        {
            tempoAvancando = Mathf.Max(totalTimeIn,totalTimeOut);
        }

        public static bool DodgeWall(Transform cameraP, Vector3 alvo, float escalA, bool suave = false,bool changeRotation = true)
        {
            RaycastHit raioColisor;

            if (antPos == default)
                antPos = cameraP.position;

            Vector3 posAlvo = alvo + escalA * Vector3.up;
            Vector3 proj = Vector3.Project(posAlvo - cameraP.position, Vector3.up);

            Debug.DrawLine(cameraP.position, posAlvo, Color.blue);
            Debug.DrawLine(posAlvo, cameraP.position +proj, Color.green);

            tempoAvancando += Time.deltaTime;

            if (Physics.Linecast(posAlvo, cameraP.position, out raioColisor, 9))
            {
                Debug.DrawLine(cameraP.position, raioColisor.point, Color.red);


                if (suave)
                {
                    if (VerifyTags(raioColisor))
                    {
                        VerifiqueAcionamento(cameraP.position, true);

                        cameraP.position = Vector3.Lerp(startPos, raioColisor.point + raioColisor.normal * 0.2f, tempoAvancando / totalTimeIn);
                        if(changeRotation)
                            cameraP.rotation = Quaternion.LookRotation(-cameraP.position+ posAlvo);
                        antPos = cameraP.position;

                        return true;
                    }
                    else
                    {
                        VerifiqueAcionamento(cameraP.position, false);
                        cameraP.position = Vector3.Lerp(startPos, cameraP.position, tempoAvancando / totalTimeOut);

                        if(changeRotation)
                            cameraP.rotation = Quaternion.LookRotation(-cameraP.position + posAlvo);
                    }
                }
                else if (VerifyTags(raioColisor))
                {
                    cameraP.position = //Vector3.Lerp(cameraP.position,
                            raioColisor.point + cameraP.forward * 0.2f;
                    return true;
                }
            }
            else if (suave)
            {
                VerifiqueAcionamento(antPos, false);
                cameraP.position = Vector3.Lerp(startPos, cameraP.position, tempoAvancando / totalTimeOut);

                if(changeRotation)
                    cameraP.rotation = Quaternion.LookRotation(-cameraP.position + posAlvo);
            }

            return false;
        }

        static void VerifiqueAcionamento(Vector3 pos,bool f)
        {

            if (estavaAcionado != f)
            {
                tempoAvancando = Mathf.Clamp(1-tempoAvancando,0,1);
                startPos = pos;
            }
            estavaAcionado = f;
        }
    }
}