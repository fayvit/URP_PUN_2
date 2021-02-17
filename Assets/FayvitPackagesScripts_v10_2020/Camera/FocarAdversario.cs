using UnityEngine;
using System.Collections.Generic;

namespace FayvitCam
{
    [System.Serializable]
    public class FocarAdversario
    {
        [SerializeField] private GameObject alvoFoco;
        [SerializeField] private RectTransform UI_Element;
        [SerializeField] private RectTransform CanvasRect;


        //private float x = 0;
        //private float y = 0;


        private Camera Cam;

        //public List<GameObject> OsPerto { get; set; }


        void MovimentaMiraNoAdversario(Vector3 WorldObject)
        {
            Vector2 ViewportPosition = Cam.WorldToViewportPoint(WorldObject + 0.5f * Vector3.up);

            Vector2 WorldObject_ScreenPosition = new Vector2(
            (ViewportPosition.x * CanvasRect.sizeDelta.x * Cam.rect.width) - (CanvasRect.sizeDelta.x * (0.5f - Cam.rect.x)),
            (ViewportPosition.y * CanvasRect.sizeDelta.y * Cam.rect.height) - (CanvasRect.sizeDelta.y * (0.5f - Cam.rect.y)));

            float dist = Vector3.Distance(Cam.transform.position, WorldObject);
            UI_Element.localScale = Vector3.Lerp(Vector3.one, 0.5f * Vector3.one, (dist - 10) / 20);
            UI_Element.anchoredPosition = WorldObject_ScreenPosition;
        }

        void AplicaAlvoFoco(Transform alvoDaCamera)
        {
            if (!alvoFoco)
            {
                Debug.LogError("alvoFoco não setado no inspector");
                return;

                #region suprimido
                //alvoFoco = MonoBehaviour.Instantiate<GameObject>(elementosDoJogo.el.retorna(DoJogo.AlvoFoco));
                //UI_Element = alvoFoco.transform.GetChild(0).GetComponentInChildren<RectTransform>();
                //CanvasRect = alvoFoco.GetComponent<RectTransform>();

                // GerenciadorDeMultiplayer.MinhaHUD(numControl).AcionaHudVidaInimigo(OsPerto[focado].GetComponent<GerenciadorDeCriature>().MeuCriatureBase);
                #endregion
            }
            else if (!alvoFoco.activeSelf)
            {
                alvoFoco.SetActive(true);
                // GerenciadorDeMultiplayer.MinhaHUD(numControl).AcionaHudVidaInimigo(OsPerto[focado].GetComponent<GerenciadorDeCriature>().MeuCriatureBase);
            }


            Vector3 posDoInimigo = alvoDaCamera.position + Vector3.up;
            // Vector3 dirInimigoPersonagem = (alvoDaCamera.position - posDoInimigo).normalized;

            if (alvoFoco.activeSelf)
            {
                MovimentaMiraNoAdversario(posDoInimigo);
                UI_Element.Rotate(Vector3.forward, 1.5f);
            }

        }

        //void AtualizeOsPerto()
        //{
        //    List<GameObject> GG = new List<GameObject>();
        //    #region suprimido_bomRever
        //    //foreach (GameObject G in osPerto)
        //    //{
        //    //    if (G != null)
        //    //        if (G.GetComponent<GerenciadorDeCriature>().MeuCriatureBase.CaracCriature.meusAtributos.PV.Corrente > 0)
        //    //            GG.Add(G);
        //    //}
        //    #endregion

        //    if (OsPerto.Count > focado)
        //        focado = (OsPerto[focado] != null) ? GG.IndexOf(OsPerto[focado]) : 0;

        //    OsPerto = GG;
        //}

        public void RemoveMira()//(Controlador controlador)
        {
            if (alvoFoco)
                alvoFoco.SetActive(false);

            //  GerenciadorDeMultiplayer.MinhaHUD(controlador).DesacionaHudVidaInimigo();

        }

        //void FocoScrool(float alteradorDeAlvo)
        //{
        //    #region suprimido
        //    //int focadoAntigo = focado;
        //    // float alt2 = cAlt.alternador2("joy " + numControl + " Xcam");

        //    //if (numControl == -1)
        //    //    alt2 = 0;

        //    //if (alt2 == 0 /*&& numControl == -1*/)
        //    //{
        //    //    alt2 = cAlt.alternador3("joy -1 triggers");
        //    //}
        //    #endregion


        //    if (alteradorDeAlvo != 0)
        //    {
        //        AtualizeOsPerto();
        //        // int focadoAntigo = focado;
        //        if ( alteradorDeAlvo < 0)
        //        {
        //            if (focado > 0)
        //                focado--;
        //            else
        //                focado = Mathf.Max(0, OsPerto.Count - 1);
        //        }
        //        else if (alteradorDeAlvo > 0)
        //        {

        //            if (focado < OsPerto.Count - 1)
        //                focado++;
        //            else
        //                focado = 0;
        //        }


        //    }

        //    #region suprimido_2
        //    //if (focado != focadoAntigo)
        //    //    GerenciadorDeMultiplayer
        //    //        .MinhaHUD(numControl)
        //    //        .TrocaInimigoFocado(OsPerto[focado].GetComponent<GerenciadorDeCriature>().MeuCriatureBase);
        //    #endregion

        //}

        public void Focar(Transform camera, Transform alvo, float alteradorDeAlvo)
        {
            if (!Cam)
                Cam = camera.GetComponent<Camera>();
            if (!Cam)
                Cam = camera.GetComponentInChildren<Camera>();

            #region suprimido
            //float escalA = caracteristicas.AlturaDoPersonagem;
            //float velocidadeMaxFoco = caracteristicas.velocidadeMaxFoco;

            //Transform personagem = caracteristicas.alvo;
            #endregion

            AplicaAlvoFoco(alvo);

            #region Suprimido_2
            //if (OsPerto.Count>focado)
            //    if (!OsPerto[focado])
            //    {
            //        AtualizeOsPerto();
            //    }

            //FocoScrool(alteradorDeAlvo);

            //if (OsPerto.Count > 0)
            //{
            //    AplicaAlvoFoco(alvo);

            //    //Vector3 direcaoDaVisao = OsPerto[focado].transform.position - personagem.position;

            //    //Quaternion alvoQ = Quaternion.LookRotation(direcaoDaVisao +
            //    //                                       escalA * Vector3.down);

            //    //x = Mathf.LerpAngle(x, alvoQ.eulerAngles.y, velocidadeMaxFoco * Time.deltaTime);
            //    //y = Mathf.LerpAngle(y, alvoQ.eulerAngles.x, velocidadeMaxFoco * Time.deltaTime);

            //}

            //return new Vector2(x, y);
            #endregion
        }

    }
}