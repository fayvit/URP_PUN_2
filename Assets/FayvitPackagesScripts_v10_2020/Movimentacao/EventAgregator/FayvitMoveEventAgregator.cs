using UnityEngine;
using System.Collections.Generic;
using System;

namespace FayvitMove
{
    public class FayvitMoveEventAgregator
    {

        private static Dictionary<FayvitMoveEventKey, List<Action<FayvitMoveEvent>>> _eventDictionary
            = new Dictionary<FayvitMoveEventKey, List<Action<FayvitMoveEvent>>>();

        public static void AddListener(FayvitMoveEventKey key, Action<FayvitMoveEvent> callback)
        {
            List<Action<FayvitMoveEvent>> callbackList;
            if (!_eventDictionary.TryGetValue(key, out callbackList))
            {
                callbackList = new List<Action<FayvitMoveEvent>>();
                _eventDictionary.Add(key, callbackList);
            }

            callbackList.Add(callback);

        }

        public static void RemoveListener(FayvitMoveEventKey key, Action<FayvitMoveEvent> acao)
        {
            List<Action<FayvitMoveEvent>> callbackList;
            if (_eventDictionary.TryGetValue(key, out callbackList))
            {
                callbackList.Remove(acao);
            }
        }

        public static void Publish(FayvitMoveEventKey key, FayvitMoveEvent umEvento = null)
        {
            List<Action<FayvitMoveEvent>> callbackList;
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

        public static void Publish(FayvitMoveEvent e)
        {
            Publish(e.Key, e);
        }

        public static void ClearListeners()
        {
            _eventDictionary = new Dictionary<FayvitMoveEventKey, List<Action<FayvitMoveEvent>>>();
        }

    }

    public enum FayvitMoveEventKey
    {
        nulo = -1,
        changeMoveSpeed = 0,
        animateDownJump = 1,
        animateStartJump = 2,
        animateFall = 3,
        startRoll = 4,
        endRoll = 5,
        posRollAttack = 6,
        atkTrigger = 7,
        damageAnimate = 8,
        deathAnimate = 9,
        endAttack = 10,
        enterInBlock = 11,
        exitInBlock = 12,
        blockHit = 13,
    }
}