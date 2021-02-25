using UnityEngine;
using UnityEngine.UI;
using FayvitEventAgregator;

namespace FayvitUI
{
    public class SingleMessagePanel : MonoBehaviour
    {

        private System.Action onClose;

        [SerializeField] private Text messageText = default;
        [SerializeField] private Text messageButton = default;

        // Use this for initialization
        public void StartMessagePanel(System.Action closeAction, string messageText, string messageButton = "")
        {
            gameObject.SetActive(true);
            this.messageText.text = messageText;
            this.messageButton.text = messageButton;
            onClose = closeAction;
        }

        public void StartMessagePanel(System.Action closeAction)
        {
            gameObject.SetActive(true);
            onClose = closeAction;
        }

        public void ChangeMessageText(string s)
        {
            messageText.text = s;
        }

        public void ChangeButtonText(string s)
        {
            messageButton.text = s;
        }

        public void ChangeMessageAndButtonText(string buttonText, string messageText)
        {
            ChangeMessageText(messageText);
            ChangeButtonText(buttonText);
        }

        public void BtnCallback()
        {
            gameObject.SetActive(false);

            if (onClose != null)
            {
                onClose();
                onClose = null;
            }

        }

        public void ThisUpdate(bool input)
        {
            if (input)
            {
                EventAgregator.Publish(EventKey.closeMessagePanel);
                BtnCallback();
            }
        }
    }
}