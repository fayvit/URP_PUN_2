using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FayvitUI
{
    public class AnOption : MonoBehaviour
    {
        [SerializeField] private Image spriteDoItem;

        public Image SpriteDoItem { get => spriteDoItem; set => spriteDoItem = value; }

        protected System.Action<int> ThisAction { get; set; }

        public virtual void InvokeAction()
        {
            ThisAction(transform.GetSiblingIndex() - 1);
        }
    }
}
