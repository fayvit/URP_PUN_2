using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI
{
    public class ParentingInTheHUD
    {

        public static GameObject Parenting(GameObject aContainerItem, RectTransform variableSizeContainer)
        {

            GameObject G = MonoBehaviour.Instantiate(aContainerItem);
            RectTransform T = G.GetComponent<RectTransform>();
            T.SetParent(variableSizeContainer.transform);

            T.localScale = new Vector3(1, 1, 1);
            /*
            T.offsetMax = Vector2.zero;
            T.offsetMin = Vector2.zero;
            */

            return G;

        }
    }
}
