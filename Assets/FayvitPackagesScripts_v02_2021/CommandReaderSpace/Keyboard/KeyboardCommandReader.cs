using UnityEngine;
using System.Collections;

namespace FayvitCommandReader
{
    public class KeyboardCommandReader : CommandReaderSupport, ICommandReader
    {
        private static KeyboardCommandReader instance;
        public static KeyboardCommandReader Instance
        {
            get
            {
                if (instance == null)
                    instance = new KeyboardCommandReader();

                return instance;
            }
        }

        private KeyboardCommandReader() { }

        public int IndexOfControl => (int)Controlador.teclado;

        public Controlador ControlId => Controlador.teclado;

        public float GetAxis(string esseGatilho)
        {
            esseGatilho = KeyStringDict.GetStringForAxis(esseGatilho);
            return RawCustomAxis.GetAxis(esseGatilho, IndexOfControl, KeyboardKeysDict.Instance);
            //Input.GetAxisRaw("joy " + IndexOfControl + " " + esseGatilho);
        }

        public bool GetButton(int numButton)
        {
            bool retorno = false;

            try
            {
                retorno = Input.GetButton("joystick " + IndexOfControl + " button " + numButton);
            }
            catch
            {
                //Debug.Log("Não existe o cadastro para o botao: " + numButton);
            }

            if(KeyboardKeysDict.dicKeys.ContainsKey(numButton))
            for (int i=0;i< KeyboardKeysDict.dicKeys[numButton].Count;i++)
                retorno|= Input.GetKey(KeyboardKeysDict.dicKeys[numButton][i]);
            return retorno;
            //return Input.GetButton("joystick " + IndexOfControl + " button " + numButton);
        }

        public bool GetButtonDown(int numButton)
        {
            
            bool retorno = false;

            try
            {
                retorno = Input.GetButtonDown("joystick " + IndexOfControl + " button " + numButton);
            }
            catch {

            }

            if (KeyboardKeysDict.dicKeys.ContainsKey(numButton))
                for (int i = 0; i < KeyboardKeysDict.dicKeys[numButton].Count; i++)
                    retorno |= Input.GetKeyDown(KeyboardKeysDict.dicKeys[numButton][i]);
            return retorno;
            //return Input.GetButtonDown("joystick " + IndexOfControl + " button " + numButton);
        }

        public bool GetButtonUp(int numButton)
        {
            bool retorno = false;

            try
            {
                retorno = Input.GetButtonUp("joystick " + IndexOfControl + " button " + numButton);
            }
            catch
            {
                //Debug.Log("Não existe o cadastro para o botao: " + numButton);
            }

            if (KeyboardKeysDict.dicKeys.ContainsKey(numButton))
                for (int i = 0; i < KeyboardKeysDict.dicKeys[numButton].Count; i++)
                    retorno |= Input.GetKeyUp(KeyboardKeysDict.dicKeys[numButton][i]);
            return retorno;
            //return Input.GetButtonUp("joystick " + IndexOfControl + " button " + numButton);
        }

        public bool SubmitButtonDown()
        {
            return GetButtonDown(7) || GetButtonDown(0);
        }

        public int GetIntTriggerDown(string esseGatilho)
        {
            int retorno = 0;
            float val = RawCustomAxis.GetAxis(esseGatilho, IndexOfControl, KeyboardKeysDict.Instance); 
            //Input.GetAxisRaw("joy " + IndexOfControl + " " + esseGatilho);
            retorno = VerificaValorSeZerado(esseGatilho,val,0.01f);

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
