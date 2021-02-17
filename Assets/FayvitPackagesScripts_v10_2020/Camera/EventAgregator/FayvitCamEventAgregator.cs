using UnityEngine;
using System.Collections.Generic;
using System;

namespace FayvitCam
{
    public class FayvitCamEventAgregator
    {

        private static Dictionary<FayvitCamEventKey, List<Action<FayvitCamEvent>>> _eventDictionary
            = new Dictionary<FayvitCamEventKey, List<Action<FayvitCamEvent>>>();

        public static void AddListener(FayvitCamEventKey key, Action<FayvitCamEvent> callback)
        {
            List<Action<FayvitCamEvent>> callbackList;
            if (!_eventDictionary.TryGetValue(key, out callbackList))
            {
                callbackList = new List<Action<FayvitCamEvent>>();
                _eventDictionary.Add(key, callbackList);
            }

            callbackList.Add(callback);

        }

        public static void RemoveListener(FayvitCamEventKey key, Action<FayvitCamEvent> acao)
        {
            List<Action<FayvitCamEvent>> callbackList;
            if (_eventDictionary.TryGetValue(key, out callbackList))
            {
                callbackList.Remove(acao);
            }
        }

        public static void Publish(FayvitCamEventKey key, FayvitCamEvent umEvento = null)
        {
            List<Action<FayvitCamEvent>> callbackList;
            if (_eventDictionary.TryGetValue(key, out callbackList))
            {
                //Debug.Log(callbackList.Count+" : "+umEvento.Sender+" : "+key);

                foreach (var e in callbackList)
                {
                    if (e != null)
                        e(umEvento);
                    else
                        Debug.LogWarning("Event agregator chamou uma função nula na key: " + key +
                            "\r\n Geralmente ocorre quando o objeto do evento foi destruido sem se retirar do listener");
                }
            }
        }

        public static void Publish(FayvitCamEvent e)
        {
            Publish(e.Key, e);
        }

        public static void ClearListeners()
        {
            _eventDictionary = new Dictionary<FayvitCamEventKey, List<Action<FayvitCamEvent>>>();
        }

    }

    public enum FayvitCamEventKey
    {
        nulo = -1,
        requestShakeCamera,
        controlableReached
    }
}