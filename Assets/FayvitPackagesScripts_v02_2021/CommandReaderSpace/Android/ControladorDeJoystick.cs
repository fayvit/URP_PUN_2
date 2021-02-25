using UnityEngine;
using UnityEngine.UI;
using FayvitEventAgregator;

namespace FayvitCommandReader
{
    public class ControladorDeJoystick : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private DragSupport moveJoy;
        [SerializeField] private DragSupport camJoy;
        [SerializeField] private MyButtonEvents[] buttons;
#pragma warning restore 0649

        public static ControladorDeJoystick cj;

        public MyButtonEvents GetButton(int numButton)
        {
            if (buttons.Length > numButton)
                return buttons[numButton];
            else return null;
        }

        private void Start()
        {
            if (cj == null)
                cj = this;
            else
                Destroy(gameObject);

            if (GetComponent<Image>() == null)
            {
                Debug.LogError("There is no joystick image attached to this script.");
            }

            if (transform.GetChild(0).GetComponent<Image>() == null)
            {
                Debug.LogError("There is no joystick handle image attached to this script.");
            }

            if(moveJoy)
                moveJoy.Start();

            if(camJoy)
                camJoy.Start();

            #region EventosEspecificosDoJogo_Start
            /*  Eventos especificos do jogo */
            //FayvitCommandReaderEventAgregator.AddListener(EventKey.startCheckPoint, OnStartCheckPoint);
            //FayvitCommandReaderEventAgregator.AddListener(EventKey.requestToFillDates, OnRequestFillDates);
            //FayvitCommandReaderEventAgregator.AddListener(EventKey.starterHudForTest, VerifiqueBtnDash);
            //FayvitCommandReaderEventAgregator.AddListener(EventKey.allAbilityOn, VerifiqueBtnDash);
            //FayvitCommandReaderEventAgregator.AddListener(EventKey.inicializaDisparaTexto, OnStartTalk);
            //FayvitCommandReaderEventAgregator.AddListener(EventKey.finalizaDisparaTexto, OnFinishTalk);
            #endregion

            EventAgregator.AddListener(EventKey.requestHideControllers, OnRequestHideControlls);
            EventAgregator.AddListener(EventKey.requestShowControllers, OnRequestShowControlls);
            EventAgregator.AddListener(EventKey.changeHardwareController, OnChangeHardwareController);

            Debug.LogWarning("Interessante chamar o Evento changeHardwareController quando a cena for carregada");

            // OnChangeHardwareController(new FayvitCommandReaderEvent(EventKey.changeHardwareController, GlobalController.g.Control));
        }

        private void OnDestroy()
        {

            EventAgregator.RemoveListener(EventKey.requestHideControllers, OnRequestHideControlls);
            EventAgregator.RemoveListener(EventKey.requestShowControllers, OnRequestShowControlls);
            EventAgregator.RemoveListener(EventKey.changeHardwareController, OnChangeHardwareController);

            #region EventosEspecificosDoJogo_Destroy
            /*  Eventos especificos do jogo */
            //FayvitCommandReaderEventAgregator.RemoveListener(EventKey.inicializaDisparaTexto, OnStartTalk);
            //FayvitCommandReaderEventAgregator.RemoveListener(EventKey.finalizaDisparaTexto, OnFinishTalk);
            //FayvitCommandReaderEventAgregator.RemoveListener(EventKey.startCheckPoint, OnStartCheckPoint);
            //FayvitCommandReaderEventAgregator.RemoveListener(EventKey.requestToFillDates, OnRequestFillDates);
            //FayvitCommandReaderEventAgregator.RemoveListener(EventKey.starterHudForTest, VerifiqueBtnDash);
            //FayvitCommandReaderEventAgregator.RemoveListener(EventKey.allAbilityOn, VerifiqueBtnDash);
            #endregion

        }

        private void OnChangeHardwareController(IGameEvent e)
        {
            transform.parent.gameObject.SetActive((Controlador)e.MySendObjects[0] == Controlador.Android);

        }



        void OnRequestShowControlls(IGameEvent e)
        {
            Controlador c = (Controlador)e.MySendObjects[0];
            if (c == Controlador.Android)
                transform.parent.gameObject.SetActive(true);
        }

        void OnRequestHideControlls(IGameEvent e)
        {
            if(moveJoy!=null)
                moveJoy.OnPointerUp(null);

            if(camJoy)
                camJoy.OnPointerUp(null);

            Controlador c = (Controlador)e.MySendObjects[0];
            if (c == Controlador.Android)
                transform.parent.gameObject.SetActive(false);
        }

        //void MoveDrag(Vector2 localPoint, PointerEventData ped,Image bgImage,Image joystickKnobImage,ref Vector3  inputVector)
        //{
        //    localPoint.x = (localPoint.x / bgImage.rectTransform.sizeDelta.x);
        //    localPoint.y = (localPoint.y / bgImage.rectTransform.sizeDelta.y);

        //    inputVector = new Vector3(localPoint.x * 2 + 1, localPoint.y * 2 - 1, 0);

        //    unNormalizedInput = inputVector;

        //    inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        //    joystickKnobImage.rectTransform.anchoredPosition =
        //     new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x / distanciaDeAfastamentoDoJoystick),
        //                 inputVector.y * (bgImage.rectTransform.sizeDelta.y / distanciaDeAfastamentoDoJoystick));

        //    if (!joystickComPosicaoFixa)
        //    {
        //        // if dragging outside the circle of the background image
        //        if (unNormalizedInput.magnitude > inputVector.magnitude)
        //        {
        //            var currentPosition = bgImage.rectTransform.position;
        //            currentPosition.x += ped.delta.x;
        //            currentPosition.y += ped.delta.y;

        //            // keeps the joystick on the left-hand half of the screen
        //            currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + bgImage.rectTransform.sizeDelta.x, Screen.width / 2);
        //            currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - bgImage.rectTransform.sizeDelta.y);

        //            // moves the entire joystick along with the drag  
        //            bgImage.rectTransform.position = currentPosition;
        //        }
        //    }
        //}

        //public void OnDrag(PointerEventData ped)
        //{
        //    Vector2 localPoint;
        //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, ped.position, ped.pressEventCamera, out localPoint))
        //    {
        //        MoveDrag(localPoint, ped,bgImage, joystickKnobImage,ref inputVector);
        //    }
        //}

        //public void OnPointerDown(PointerEventData ped)
        //{
        //    OnDrag(ped);
        //}

        //public void OnPointerUp(PointerEventData eventData)
        //{
        //    inputVector = Vector3.zero;
        //    joystickKnobImage.rectTransform.anchoredPosition = Vector3.zero;
        //}

        public Vector3 GetInputDirection()
        {
            if (moveJoy)
                return new Vector3(moveJoy.InputVector.x, moveJoy.InputVector.y, 0);
            else
                return Vector3.zero;
        }

        public Vector3 GetCamDirection()
        {
            if (camJoy)
                return new Vector3(camJoy.InputVector.x, camJoy.InputVector.y, 0);
            else
                return Vector3.zero;
        }

        public float GetInputVal(string qualVal)
        {
            float val = 0;
            switch (qualVal)
            {
                case "horizontal":
                    if (moveJoy)
                        val = moveJoy.InputVector.x;// != 0 ? Mathf.Sign(moveJoy.InputVector.x) * Mathf.Min(1.5f * GetInputDirection().magnitude, 1) : 0;
                break;
                case "vertical":
                    if(moveJoy)
                        val = moveJoy.InputVector.y != 0 ? Mathf.Sign(moveJoy.InputVector.y) * Mathf.Min(1.5f * GetInputDirection().magnitude, 1) : 0;
                    break;
                case "Xcam":
                    if(camJoy)
                        val = camJoy.InputVector.x != 0 ? Mathf.Sign(camJoy.InputVector.x) * Mathf.Min(1.5f * GetCamDirection().magnitude, 1) : 0; 
                break;
                case "Ycam":
                    if(camJoy)
                        val = camJoy.InputVector.y;
                break;
            }
            return val;
        }
    }
}