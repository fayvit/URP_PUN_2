
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI
{
    public class AnImageOption : AnOption
    {
        [SerializeField] private Image optionImage;

        public Image OptionImage { get { return optionImage; } set { optionImage = value; } }

        public void SetarOpcoes(Sprite S, System.Action<int> A)
        {
            ThisAction += A;
            optionImage.sprite = S;
        }
    }
}
