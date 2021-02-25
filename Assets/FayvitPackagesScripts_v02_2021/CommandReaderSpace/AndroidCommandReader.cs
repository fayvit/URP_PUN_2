using UnityEngine;
using System.Collections;

namespace FayvitCommandReader
{
    public class AndroidCommandReader : CommandReaderSupport, ICommandReader
    {
        private static AndroidCommandReader instance;
        public static AndroidCommandReader Instance
        {
            get
            {
                if (instance == null)
                    instance = new AndroidCommandReader();

                return instance;
            }
        }

        private AndroidCommandReader() { }

        public int IndexOfControl => (int)Controlador.Android;

        public Controlador ControlId => Controlador.Android;

        public float GetAxis(string esseGatilho)
        {
            float val = 0;

            esseGatilho = KeyStringDict.GetStringForAxis(esseGatilho);

            if (ControladorDeJoystick.cj != null)
                val = ControladorDeJoystick.cj.GetInputVal(esseGatilho);
            return val;
        }

        public bool GetButton(int numButton)
        {
            MyButtonEvents b = TryGetButton(numButton);
            if (b != null)
                return b.buttonPress;

            return false;
        }

        public bool GetButtonDown(int numButton)
        {
            MyButtonEvents b = TryGetButton(numButton);
            if (b != null)
                return b.buttonDown;

            return false;
        }

        public bool GetButtonUp(int numButton)
        {
            MyButtonEvents b = TryGetButton(numButton);
            if (b != null)
                return b.buttonUp;

            return false;
        }

        public bool SubmitButtonDown()
        {
            return GetButtonDown(0);
        }

        public int GetIntTriggerDown(string esseGatilho)
        {
            int retorno = 0;
            float val = 0;
            if (ControladorDeJoystick.cj != null)
                val = ControladorDeJoystick.cj.GetInputVal(esseGatilho);

            retorno = VerificaValorSeZerado(esseGatilho, val, 0.1f);

            return retorno;
        }

        public bool VerifyThisControlUse()
        {
            return Input.touchCount > 0;
        }

        public Vector3 DirectionalVector()
        {

            float h = Mathf.Clamp(GetAxis("horizontal"), -1, 1);
            float v = Mathf.Clamp(GetAxis("vertical"), -1, 1);

            return VetorDirecao(h, v);
        }

        private MyButtonEvents TryGetButton(int numButton)
        {
            if (ControladorDeJoystick.cj != null)
            {
                MyButtonEvents b = ControladorDeJoystick.cj.GetButton(numButton);
                return b;
            }
            else
                return null;
        }

        public bool GetButton(string nameButton)
        {
            return GetButton(nameButton, this);
        }

        public bool GetButtonDown(string nameButton)
        {
            return GetButtonDown(nameButton, this);
        }

        public bool GetButtonUp(string nameButton)
        {
            return GetButtonUp(nameButton, this);
        }
    }
}