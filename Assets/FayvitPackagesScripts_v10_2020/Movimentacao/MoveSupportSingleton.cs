using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FayvitMove
{
    public class MoveSupportSingleton : MonoBehaviour
    {

        private Dictionary<string, System.Action> schelduleActions = new Dictionary<string, System.Action>();
        private static MoveSupportSingleton instance;

        public static MoveSupportSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject G = new GameObject();
                    G.name = "Fayvit_Move_SupportSingleton";
                    DontDestroyOnLoad(G);

                    instance = G.AddComponent<MoveSupportSingleton>();
                }

                return instance;

            }
        }
        // Use this for initialization
        void Start()
        {
            MoveSupportSingleton[] g = FindObjectsOfType<MoveSupportSingleton>();

            if (g.Length > 1)
                Destroy(gameObject);
        }

        public void InvokeInRealTime(System.Action acao, float time)
        {
            StartCoroutine(RealTimeCall(time, acao));
        }

        public void InvokeOnEndFrame(System.Action acao)
        {
            StartCoroutine(EndFrameInvoke(acao));
        }

        IEnumerator EndFrameInvoke(System.Action s)
        {
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
            StartCoroutine(EndFrameInvokeWithObject(G, acao));
        }

        IEnumerator RealTimeCall(float time, System.Action s)
        {
            yield return new WaitForSecondsRealtime(time);
            s();
        }
    }
}