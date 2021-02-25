using UnityEngine;
using UnityEngine.UI;
using FayvitEventAgregator;
using System.Collections;

namespace FayvitUI
{
    [System.Serializable]
    public class TextDisplay
    {
        [SerializeField] private int speedLetters = 50;
        [SerializeField] private int speedWindow = 15;
        [SerializeField] private RectTransform messagePanel = default;

        private Text uiText;
        private Image img;

        private Vector2 originalPos;
        private MessagePhase phase = MessagePhase.boxOut;

        private string textForMessage = "";
        private float timeCount = 0;
        private bool startMens = false;


        public enum MessagePhase
        {
            boxOut,
            messageFilling,
            messageFill,
            bocGoingOut,
            boxExited
        }

        public int messageArrayIndex = 0;

        public void StartTextDisplay(/* bool hurry = false */)
        {
            //hurryPanel.SetActive(pressa);

            startMens = false;
            SetComponents();
            messageArrayIndex = 0;
        }

        public bool UpdateTexts(bool inputNext,bool inputFinish,string[] messageArray, Sprite foto = null)
        {
            if (messageArrayIndex < messageArray.Length)
            {
                StartShowMessage(messageArray[messageArrayIndex], foto);
            }
            else
            {
                //painelDaPressa.SetActive(false);
                return true;
            }

            if (ReadMessage() == MessagePhase.boxExited)
            {
                messageArrayIndex++;
            }


            if (inputNext)
            {
                OnInputNext();
            }

            if (inputFinish)
            {
                OffPanels();
                return true;
            }

            return false;
        }

        void SetComponents()
        {

            if (uiText == null)
            {
                uiText = messagePanel.GetComponentInChildren<Text>();
                img = messagePanel.transform.GetChild(1).GetComponent<Image>();

                originalPos = messagePanel.anchoredPosition;
            }
        }

        public void StartShowMessage(string texto, Sprite sDaFoto = null)
        {
            if (!startMens)
            {

                startMens = true;
                messagePanel.gameObject.SetActive(true);
                messagePanel.anchoredPosition = new Vector2(originalPos.x, Screen.height);
                uiText.text = "";

                if (sDaFoto != null)
                {
                    img.enabled = true;
                    img.sprite = sDaFoto;
                }
                else
                    img.enabled = false;

                phase = MessagePhase.boxOut;
                this.textForMessage = texto;

            }
        }

        public bool LendoMensagemAteOCheia(bool inputNext)
        {
            if (ReadMessage() != MessagePhase.messageFill)
            {
                if (inputNext)
                {
                    OnInputNext();
                }
                return true;
            }
            else
                return false;
        }

        public MessagePhase ReadMessage()
        {
            timeCount += Time.deltaTime;
            if (startMens)
            {
                switch (phase)
                {
                    case MessagePhase.boxOut:
                        if (Vector2.Distance(messagePanel.anchoredPosition, originalPos) > 0.1f)
                        {
                            messagePanel.anchoredPosition = Vector2.Lerp(
                                messagePanel.anchoredPosition, originalPos, Time.deltaTime * speedWindow);
                        }
                        else
                        {
                            phase = MessagePhase.messageFilling;
                            timeCount = 0;
                        }
                        break;
                    case MessagePhase.messageFilling:
                        if ((int)(timeCount * speedLetters) <= textForMessage.Length && !textForMessage.Contains("<co"))
                            uiText.text = textForMessage.Substring(0, (int)(timeCount * speedLetters));
                        else
                        {
                            phase = MessagePhase.messageFill;
                            uiText.text = textForMessage;
                        }
                        break;
                    case MessagePhase.bocGoingOut:
                        if (Mathf.Abs(messagePanel.anchoredPosition.y - Screen.height) > 0.1f)
                        {
                            messagePanel.anchoredPosition = Vector2.Lerp(messagePanel.anchoredPosition,
                                                                new Vector2(messagePanel.anchoredPosition.x, Screen.height),
                                                                Time.deltaTime * speedWindow);
                        }
                        else
                        {
                            startMens = false;
                            phase = MessagePhase.boxExited;
                        }
                        break;
                }


            }
            return phase;
        }

        public void OnInputNext()
        {
            switch (phase)
            {
                case MessagePhase.messageFilling:
                    EventAgregator.Publish(EventKey.mensagemEnchendo);
                    //EventAgregator.Publish(new StandardSendGameEvent(GameController.g.gameObject, EventKey.disparaSom, SoundEffectID.Book1.ToString()));
                    uiText.text = textForMessage;
                    phase = MessagePhase.messageFill;
                    break;

                case MessagePhase.messageFill:
                    EventAgregator.Publish(EventKey.mensgemCheia);
                    //EventAgregator.Publish(new StandardSendGameEvent(GameController.g.gameObject, EventKey.disparaSom, SoundEffectID.Book1.ToString()));
                    phase = MessagePhase.bocGoingOut;
                    timeCount = 0;
                    break;

                case MessagePhase.boxOut:
                    EventAgregator.Publish(EventKey.caixaDeTextoIndo);
                    //EventAgregator.Publish(new StandardSendGameEvent(GameController.g.gameObject, EventKey.disparaSom, SoundEffectID.Book1.ToString()));
                    messagePanel.anchoredPosition = originalPos;
                    phase = MessagePhase.messageFilling;
                    break;

                case MessagePhase.bocGoingOut:
                    startMens = false;
                    phase = MessagePhase.boxExited;
                    EventAgregator.Publish(EventKey.caixaDeTextoSaiu);
                    messageArrayIndex++;
                    break;
                    /*
                    EventAgregator.Publish(new StandardSendStringEvent(GameController.g.gameObject, SoundEffectID.Book1.ToString(), EventKey.disparaSom));
                    painelDaMens.anchoredPosition = new Vector2(painelDaMens.anchoredPosition.x, Screen.height);
                    fase = FasesDaMensagem.caixaSaiu;
                break;*/
            }
        }

        public void TurnPanels()
        {
            startMens = false;
            messageArrayIndex = 0;
            messagePanel.gameObject.SetActive(true);
        }

        public void OffPanels()
        {
            //painelDaPressa.SetActive(false);
            messagePanel.gameObject.SetActive(false);
        }
    }
}