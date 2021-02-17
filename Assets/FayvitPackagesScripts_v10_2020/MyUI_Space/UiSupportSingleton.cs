using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FayvitUI
{
    public class UiSupportSingleton : MonoBehaviour
    {
        private Dictionary<string, System.Action> schelduleActions = new Dictionary<string, System.Action>();
        private static UiSupportSingleton instance;

        public static UiSupportSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject G = new GameObject();
                    G.name = "FayvitUiSupportSingleton";
                    DontDestroyOnLoad(G);

                    instance = G.AddComponent<UiSupportSingleton>();
                }

                return instance;

            }
        }
        // Use this for initialization
        void Start()
        {
            UiSupportSingleton[] g = FindObjectsOfType<UiSupportSingleton>();

            if (g.Length > 1)
                Destroy(gameObject);
        }

        public void InvokeInRealTime(System.Action acao,float time)
        {
            string guid = System.Guid.NewGuid().ToString();
            schelduleActions.Add(guid,acao);
            StartCoroutine(RealTimeCall(time,guid));
        }

        public void InvokeOnEndFrame(System.Action acao)
        {
            string guid = System.Guid.NewGuid().ToString();
            schelduleActions.Add(guid, acao);
            StartCoroutine(EndFrameInvoke(guid));
        }

        IEnumerator RealTimeCall(float time, string guid)
        {
            yield return new WaitForSecondsRealtime(time);
            schelduleActions[guid]();
            schelduleActions.Remove(guid);
        }

        IEnumerator EndFrameInvoke(string guid)
        {
            yield return new WaitForEndOfFrame();
            schelduleActions[guid]();
            schelduleActions.Remove(guid);
        }


    }
}