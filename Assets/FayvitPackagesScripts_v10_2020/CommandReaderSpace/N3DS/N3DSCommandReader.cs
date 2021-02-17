using UnityEngine;
using System.Collections;


namespace FayvitCommandReader
{
    public class N3DSCommandReader : CommandReaderSupport, ICommandReader
    {
        private static N3DSCommandReader instance;
        public static N3DSCommandReader Instance
        {
            get
            {
                if (instance == null)
                    instance = new N3DSCommandReader();

                return instance;
            }
        }

        private N3DSCommandReader() { }

        public int IndexOfControl => (int)Controlador.N3DS;

        public Controlador ControlId => Controlador.N3DS;

        public float GetAxis(string esseGatilho)
        {
            esseGatilho = KeyStringDict.GetStringForAxis(esseGatilho);
            return RawCustomAxis.GetAxis(esseGatilho,(int)Controlador.N3DS,N3DS_KeysDic.Instance);
        }

        public bool GetButton(int numButton)
        {
            bool retorno = false;

            for (int i = 0; i < KeyboardKeysDict.dicKeys[numButton].Count; i++)
                retorno |= Input.GetKey(N3DS_KeysDic.Instance.DicKeys[numButton][i]);
            return retorno;
        }

        public bool GetButtonDown(int numButton)
        {
            bool retorno = false;

            for (int i = 0; i < KeyboardKeysDict.dicKeys[numButton].Count; i++)
                retorno |= Input.GetKeyDown(N3DS_KeysDic.Instance.dicKeys[numButton][i]);
            return retorno;
        }

        public bool GetButtonUp(int numButton)
        {
            bool retorno = false;

            for (int i = 0; i < KeyboardKeysDict.dicKeys[numButton].Count; i++)
                retorno |= Input.GetKeyUp(N3DS_KeysDic.Instance.dicKeys[numButton][i]);
            return retorno;
        }

        public bool SubmitButtonDown()
        {
            return GetButtonDown(0);
        }

        public int GetIntTriggerDown(string esseGatilho)
        {

            int retorno = 0;
            float val = RawCustomAxis.GetAxis(esseGatilho,IndexOfControl,N3DS_KeysDic.Instance);
            retorno = VerificaValorSeZerado(esseGatilho, val, 0.1f);

            return retorno;
        }

        public bool VerifyThisControlUse()
        {
            return false;
        }

        public Vector3 DirectionalVector()
        {
            
            float h = Mathf.Clamp(RawCustomAxis.GetAxis("horizontal",IndexOfControl,N3DS_KeysDic.Instance) 
                + RawCustomAxis.GetAxis("HDpad",IndexOfControl,N3DS_KeysDic.Instance), -1, 1);
            float v = Mathf.Clamp(RawCustomAxis.GetAxis("vertical",IndexOfControl,N3DS_KeysDic.Instance) 
                + RawCustomAxis.GetAxis("VDpad",IndexOfControl,N3DS_KeysDic.Instance), -1, 1);

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