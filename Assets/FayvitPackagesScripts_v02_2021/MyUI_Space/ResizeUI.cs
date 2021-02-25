using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FayvitUI
{
    public class ResizeUI
    {
        public static void InVertical(RectTransform redimensionado, GameObject item, int num)
        {
            /*
            Debug.Log(redimensionado.rect.height+" : "+redimensionado.sizeDelta+" : "+ item.GetComponent<RectTransform>().rect.height+
                " : "+redimensionado.anchorMax+" : "+redimensionado.anchorMin);*/

            redimensionado.sizeDelta
                = new Vector2(redimensionado.sizeDelta.x, 
                num * item.GetComponent<RectTransform>().rect.height - (redimensionado.rect.height- redimensionado.sizeDelta.y));

            /*
            Debug.Log(redimensionado.rect.height + " : "+redimensionado.sizeDelta + 
                " : " + item.GetComponent<RectTransform>().rect.height + " : " + redimensionado.anchorMax + 
                " : " + redimensionado.anchorMin);*/
        }

        public static void InHorizontal(RectTransform redimensionado, GameObject item, int num)
        {
            redimensionado.sizeDelta
                = new Vector2(num * item.GetComponent<RectTransform>().rect.width 
                - (redimensionado.rect.width - redimensionado.sizeDelta.x),
                 redimensionado.sizeDelta.y );
        }

        public static void InGrid(RectTransform redimensionado, GameObject item, int num)
        {
            #region TentativaInicial

            /*
            Debug.LogWarning("Testar esse redimensionamento");
            LayoutElement lay = item.GetComponent<LayoutElement>();
            GridLayoutGroup grid = redimensionado.GetComponent<GridLayoutGroup>();

            int quantidade = Mathf.FloorToInt(
                (redimensionado.rect.width-grid.padding.left-grid.padding.right) / (grid.cellSize.x+ grid.spacing.x));

            Debug.Log("Redimensionar grade: " + num + " :" + quantidade + 
                ": " + (lay.preferredHeight + grid.spacing.y) + " : " + redimensionado.rect.width+" : "+lay.preferredWidth+
                " : "+redimensionado.sizeDelta+" : "+grid.cellSize);

            int numeroDeLinhas = Mathf.CeilToInt((float)num / quantidade);

            redimensionado.sizeDelta
                        = new Vector2(0, 
                        numeroDeLinhas * (grid.cellSize.y + grid.spacing.y) 
                        - (redimensionado.rect.height - redimensionado.sizeDelta.x-grid.padding.left-grid.padding.right));
*/
            #endregion
            //Lula Knight version


            GridLayoutGroup grid = redimensionado.GetComponent<GridLayoutGroup>();

            float outlineLength = OutlineLengthInItem(item);

            int quantidade = RowCountInGrid(redimensionado, grid, outlineLength);

            //int quantidade = Mathf.FloorToInt(
            //    (redimensionado.rect.width - grid.padding.left - grid.padding.right) / (grid.cellSize.x + grid.spacing.x + 2 * outlineLength));

            Debug.Log("Redimensionar grade: " + num + " :" + quantidade + ": " + (grid.cellSize.x + grid.spacing.y) + " : " + redimensionado.rect.width);
            int numeroDeLinhas = Mathf.CeilToInt((float)num / quantidade);
            redimensionado.sizeDelta
                        = new Vector2(redimensionado.sizeDelta.x, numeroDeLinhas * (grid.cellSize.x + grid.spacing.x + 2 * outlineLength)
                        - (redimensionado.rect.height - redimensionado.sizeDelta.y - grid.padding.top - grid.padding.bottom)
                        );
        }

        public static float OutlineLengthInItem(GameObject item)
        {
            float outlineLength = 0;
            Outline O = item.GetComponent<Outline>();

            if (O != null)
                outlineLength = O.effectDistance.x;

            return outlineLength;
        }

        public static int RowCountInGrid(RectTransform redimensionado, GridLayoutGroup grid, GameObject item)
        {
            float outlineLength = OutlineLengthInItem(item);

            int quantidade = RowCountInGrid(redimensionado, grid, outlineLength);

            Debug.Log("Redimensionar grade: " + quantidade + ": " + (grid.cellSize.x + grid.spacing.y) +
                " : " + redimensionado.rect.width + " : " + item.GetComponent<RectTransform>().rect.width);


            return quantidade;

        }

        public static int RowCountInGrid(RectTransform redimensionado, GridLayoutGroup grid, float outlineLength)
        {
            int quantidade = Mathf.FloorToInt(
                    (redimensionado.rect.width - grid.padding.left - grid.padding.right)
                    /
                    (grid.cellSize.x + grid.spacing.x + 2 * outlineLength));


            
            

            return quantidade;
        }

    }

    public enum ResizeUiType
    {
        vertical,
        grid,
        horizontal
    }
}