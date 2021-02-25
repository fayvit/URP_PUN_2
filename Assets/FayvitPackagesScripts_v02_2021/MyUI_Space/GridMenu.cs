using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FayvitUI
{
    [System.Serializable]
    public class GridMenu : InteractiveUiBase
    {
        private System.Action<int> ThisAction;
        private Sprite[] spritesForGridMenu;
        private int lineCellCount = 0;
        private int rowCellCount = 0;

        public void StartHud(System.Action<int> acaoDeFora,Sprite[] sprites)
        {
            ThisAction += acaoDeFora;
            spritesForGridMenu = sprites;

            if (sprites.Length > 0)
                StartHud(sprites.Length, ResizeUiType.grid);
            else
                aContainerItem.SetActive(false);

            SetLineRowLength();

        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            AnImageOption uma = G.GetComponent<AnImageOption>();

            Sprite S = spritesForGridMenu[indice];

            uma.SetarOpcoes(S, ThisAction);
            
        }

        public void ChangeOption(int Vval,int Hval)
        {

            int quanto = -lineCellCount * Vval;

            if (quanto == 0)
                quanto = Hval;

            ChangeOptionWithVal(quanto, lineCellCount);

            
        }

        int LineCellCount()
        {
            GridLayoutGroup grid = variableSizeContainer.GetComponent<GridLayoutGroup>();

            Debug.Log("grid lengths: "+grid.cellSize + " : " + grid.spacing.x);

            return
                (int)((variableSizeContainer.rect.width-grid.padding.left-grid.padding.right) / (grid.cellSize.x + grid.spacing.x));
        }

        int RowCellCount()
        {
            GridLayoutGroup grid = variableSizeContainer.GetComponent<GridLayoutGroup>();

            return
                (int)(variableSizeContainer.rect.height / (grid.cellSize.y + grid.spacing.y));
        }

        protected override void AfterFinisher()
        {
            ThisAction = null;
        }

        public void SetLineRowLength()
        {
            lineCellCount = LineCellCount();
            rowCellCount = RowCellCount();

            Debug.Log("Ola: " + lineCellCount + " : " + rowCellCount);
        }
    }
}
