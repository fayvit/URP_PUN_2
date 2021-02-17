using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FayvitCommandReader
{
    public static class FayvitCommandReaderEventAgregator
    {
        private static Dictionary<FayvitCR_EventKey, List<Action<FayvitCommandReaderEvent>>> _eventDictionary
            = new Dictionary<FayvitCR_EventKey, List<Action<FayvitCommandReaderEvent>>>();

        public static void AddListener(FayvitCR_EventKey key, Action<FayvitCommandReaderEvent> callback)
        {
            List<Action<FayvitCommandReaderEvent>> callbackList;
            if (!_eventDictionary.TryGetValue(key, out callbackList))
            {
                callbackList = new List<Action<FayvitCommandReaderEvent>>();
                _eventDictionary.Add(key, callbackList);
            }

            callbackList.Add(callback);

        }

        public static void RemoveListener(FayvitCR_EventKey key, Action<FayvitCommandReaderEvent> acao)
        {
            List<Action<FayvitCommandReaderEvent>> callbackList;
            if (_eventDictionary.TryGetValue(key, out callbackList))
            {
                callbackList.Remove(acao);
            }
        }

        public static void Publish(FayvitCR_EventKey key, FayvitCommandReaderEvent umEvento = null)
        {
            List<Action<FayvitCommandReaderEvent>> callbackList;
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

        public static void Publish(FayvitCommandReaderEvent e)
        {
            Publish(e.Key, e);
        }

        public static void ClearListeners()
        {
            _eventDictionary = new Dictionary<FayvitCR_EventKey, List<Action<FayvitCommandReaderEvent>>>();
        }
    }

    public enum FayvitCR_EventKey
    {
        nulo = -1,
        requestHideControllers,
        requestShowControllers,
        changeHardwareController,
        animationPointCheck
    }
}