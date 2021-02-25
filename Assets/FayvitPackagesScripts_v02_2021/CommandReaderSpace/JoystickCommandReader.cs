using UnityEngine;
using System.Collections;

namespace FayvitCommandReader
{
    public class JoystickCommandReader : CommandReaderSupport, ICommandReader
    {
        public JoystickCommandReader(Controlador C)
        {
            ControlId = C;
        }

        public JoystickCommandReader(int index)
        {
            ControlId =  (Controlador)Mathf.Clamp(index, 1, 4);

        }

        public int IndexOfControl => (int)ControlId;

        public Controlador ControlId { get; } = Controlador.nulo;

        public float GetAxis(string esseGatilho)
        {
            esseGatilho = KeyStringDict.GetStringForAxis(esseGatilho);
            return Input.GetAxisRaw("joy " + IndexOfControl + " " + esseGatilho);
        }

        public bool GetButton(int numButton)
        {
            return Input.GetKey((KeyCode)(350 + (IndexOfControl - 1) * 20 + numButton));
        }

        public bool GetButtonDown(int numButton)
        {
            return Input.GetKeyDown((KeyCode)(350 + (IndexOfControl - 1) * 20 + numButton));
        }

        public bool GetButtonUp(int numButton)
        {
            return Input.GetKeyUp((KeyCode)(350 + (IndexOfControl - 1) * 20 + numButton));
        }

        public bool SubmitButtonDown()
        {
            return GetButtonDown(0);
        }

        public int GetIntTriggerDown(string esseGatilho)
        {
            int retorno = 0;
            float val = Input.GetAxisRaw("joy " + IndexOfControl + " " + esseGatilho);
            retorno = VerificaValorSeZerado(esseGatilho, val, 0.1f);

            return retorno;
        }

        public bool VerifyThisControlUse()
        {
            return VerificaUsoDesseControle(this);
        }

        public Vector3 DirectionalVector()
        {
            float h = Mathf.Clamp(GetAxis("horizontal"), -1, 1);
            float v = Mathf.Clamp(GetAxis("vertical"), -1, 1);

            return VetorDirecao(h, v);
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
