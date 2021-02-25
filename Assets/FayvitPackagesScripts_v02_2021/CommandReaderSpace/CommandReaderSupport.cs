using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public abstract class CommandReaderSupport
    {

        private static Dictionary<string, bool> zerados = new Dictionary<string, bool>();

        public static bool VerificaUsoDesseControle(ICommandReader c)
        {
            bool retorno = false;
            //Input.ResetInputAxes();
            if (c.GetAxis("horizontal") != 0 ||
                c.GetAxis("vertical") != 0 ||
                c.GetAxis("triggers") != 0 ||
                c.GetAxis("Xcam") != 0 ||
                c.GetAxis("Ycam") != 0 ||
                c.GetAxis("HDpad") != 0 ||
                c.GetAxis("VDpad") != 0)
            {
                retorno = true;
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    if (c.GetButton(i))
                        retorno = true;
                }
            }
            return retorno;
        }

        protected static int VerificaValorSeZerado(string esseGatilho, float val, float valTolerance)
        {
            int retorno = 0;
            if (!zerados.ContainsKey(esseGatilho))
                zerados[esseGatilho] = true;

            if (zerados[esseGatilho])
            {
                if (val != 0)
                {
                    zerados[esseGatilho] = false;

                }

                if (val > 0)
                    retorno = 1;
                else if (val < 0)
                    retorno = -1;

            }
            else
            {

                retorno = 0;
                if (val > -valTolerance && val < valTolerance)
                    zerados[esseGatilho] = true;

            }

            return retorno;
        }

        protected static Vector3 VetorDirecao(float h, float v)
        {
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

            forward.y = 0;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            return (h * right + v * forward);
        }

        public bool GetButton(string nameButton,ICommandReader c)
        {
            return c.GetButton(KeyStringDict.GetIntForString(nameButton));
        }

        public bool GetButtonDown(string nameButton, ICommandReader c)
        {
            return c.GetButtonDown(KeyStringDict.GetIntForString(nameButton));
        }

        public bool GetButtonUp(string nameButton, ICommandReader c)
        {
            return c.GetButtonUp(KeyStringDict.GetIntForString(nameButton));
        }

    }
}