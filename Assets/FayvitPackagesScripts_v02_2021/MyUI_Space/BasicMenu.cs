using UnityEngine;
using FayvitSupportSingleton;

namespace FayvitUI
{
    [System.Serializable]
    public class BasicMenu : InteractiveUiBase
    {
        private string[] opcoes;
        private System.Action<int> acao;
        private bool estadoDeAcao = false;

        protected System.Action<int> Acao
        {
            get { return acao; }
        }

        protected string[] Opcoes
        {
            get { return opcoes; }
        }

        public void StartHud(
            System.Action<int> acao,
            string[] txDeOpcoes,
            ResizeUiType tipoDeR = ResizeUiType.vertical)
        {
            this.opcoes = txDeOpcoes;

            this.acao += (int x) =>
            {
                if (!estadoDeAcao)
                {
                    estadoDeAcao = true;
                    ChangeSelectionTo(x);

                    SupportSingleton.Instance.InvokeInRealTime(() =>
                    {
                        Debug.Log("Função chamada com delay para destaque do botão");
                        acao(x);
                        estadoDeAcao = false;
                    }, .05f);
                }
            };
            StartHud(opcoes.Length, tipoDeR);
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            A_MenuOption uma = G.GetComponent<A_MenuOption>();
            uma.SetarOpcao(acao, opcoes[indice]);
        }

        protected override void AfterFinisher()
        {
            acao = null;
            //Seria preciso uma finalização especifica??
        }
    }
}