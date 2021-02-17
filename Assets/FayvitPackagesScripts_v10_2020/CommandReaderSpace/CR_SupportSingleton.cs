using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public class CR_SupportSingleton : MonoBehaviour
    {
        //private Dictionary<string, System.Action> schelduleActions = new Dictionary<string, System.Action>();
        private static CR_SupportSingleton instance;

        public static CR_SupportSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject G = new GameObject();
                    G.name = "Fayvit_CR_SupportSingleton";
                    DontDestroyOnLoad(G);

                    instance = G.AddComponent<CR_SupportSingleton>();
                }

                return instance;

            }
        }
        // Use this for initialization
        void Start()
        {
            CR_SupportSingleton[] g = FindObjectsOfType<CR_SupportSingleton>();

            if (g.Length > 1)
                Destroy(gameObject);
        }

        public void InvokeInRealTime(System.Action acao, float time)
        {
            StartCoroutine(RealTimeCall(time, acao));
        }

        public void InvokeOnCountFrame(System.Action acao,uint count=1)
        {
            StartCoroutine(CountFrameInvoke(acao,count));
        }

        IEnumerator CountFrameInvoke(System.Action s,uint count)
        {
            for(int i=0; i<count;i++)
                yield return new WaitForEndOfFrame();

            s();
        }

        IEnumerator EndFrameInvokeWithObject(GameObject G, System.Action s)
        {
            yield return new WaitForEndOfFrame();
            if (G != null)
                s();
        }

        public void InvokeOnEndFrame(GameObject G, System.Action acao)
        {
            StartCoroutine(EndFrameInvokeWithObject(G,acao));
        }

        IEnumerator RealTimeCall(float time, System.Action s)
        {
            yield return new WaitForSecondsRealtime(time);
            s();
        }


    }
}