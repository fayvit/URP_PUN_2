//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//namespace FayvitUI
//{
//    public static class UiEventAgregator
//    {
//        private static Dictionary<UIEventKey, List<Action<IFayvitUiEvent>>> _eventDictionary
//            = new Dictionary<UIEventKey, List<Action<IFayvitUiEvent>>>();

//        public static void AddListener(UIEventKey key, Action<IFayvitUiEvent> callback)
//        {
//            List<Action<IFayvitUiEvent>> callbackList;
//            if (!_eventDictionary.TryGetValue(key, out callbackList))
//            {
//                callbackList = new List<Action<IFayvitUiEvent>>();
//                _eventDictionary.Add(key, callbackList);
//            }

//            callbackList.Add(callback);

//        }

//        public static void RemoveListener(UIEventKey key, Action<IFayvitUiEvent> acao)
//        {
//            List<Action<IFayvitUiEvent>> callbackList;
//            if (_eventDictionary.TryGetValue(key, out callbackList))
//            {
//                callbackList.Remove(acao);
//            }
//        }

//        public static void Publish(UIEventKey key, IFayvitUiEvent umEvento = null)
//        {
//            List<Action<IFayvitUiEvent>> callbackList;
//            if (_eventDictionary.TryGetValue(key, out callbackList))
//            {
//                //Debug.Log(callbackList.Count+" : "+umEvento.Sender+" : "+key);

//                foreach (var e in callbackList)
//                {
//                    if (e != null)
//                        e(umEvento);
//                    else
//                        Debug.LogWarning("Event agregator chamou uma função nula na key: " + key +
//                            "\r\n Geralmente ocorre quando o objeto do evento foi destruido sem se retirar do listener");
//                }
//            }
//        }

//        public static void Publish(IFayvitUiEvent e)
//        {
//            Publish(e.Key, e);
//        }

//        public static void ClearListeners()
//        {
//            _eventDictionary = new Dictionary<UIEventKey, List<Action<IFayvitUiEvent>>>();
//        }
//    }

//    public enum UIEventKey
//    {
//        nulo = -1,
//        UiDeOpcoesChange = 0,
//        closeMessagePanel = 1,
//        confirmationPanelBtnYes = 2,
//        confirmationPanelBtnNo = 3,
//        mensagemEnchendo = 4,
//        caixaDeTextoIndo = 5,
//        mensgemCheia = 6,
//        caixaDeTextoSaiu = 7,
//        changeColorPicker = 8
//    }
//}