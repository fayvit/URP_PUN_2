using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public class KeyboardKeysDict : IKeyDict
    {
        private static KeyboardKeysDict instance;
        public static KeyboardKeysDict Instance
        {
            get
            {
                if (instance == null)
                    instance = new KeyboardKeysDict();

                return instance;
            }
        }

        private KeyboardKeysDict() { }

        public static Dictionary<int, List<KeyCode>> dicKeys = new Dictionary<int, List<KeyCode>>
        {
            { 0,new List<KeyCode>{KeyCode.E,KeyCode.RightArrow } },
            { 1,new List<KeyCode>{KeyCode.L,KeyCode.DownArrow} },
            { 2,new List<KeyCode>{KeyCode.LeftShift,KeyCode.LeftArrow} },
            { 3,new List<KeyCode>{KeyCode.Space} },
            { 4,new List<KeyCode>{KeyCode.I} },
            { 5,new List<KeyCode>{KeyCode.O,KeyCode.UpArrow} },
            { 6,new List<KeyCode>{KeyCode.Escape} },
            { 7,new List<KeyCode>{KeyCode.Return} },
            { 8,new List<KeyCode>{KeyCode.R} },
            { 9,new List<KeyCode>{KeyCode.Q,KeyCode.LeftAlt} }
        };

        public static readonly Dictionary<string, List<ValAxis>> dicAxis = new Dictionary<string, List<ValAxis>>
        {
            { "horizontal", new List <ValAxis>{new ValAxis(KeyCode.D,KeyCode.A) } },
            { "vertical",new List <ValAxis>{new ValAxis(KeyCode.W,KeyCode.S)} },
            { "Xcam",new List <ValAxis>{new ValAxis(KeyCode.K,KeyCode.H)} },
            { "Ycam",new List <ValAxis>{new ValAxis(KeyCode.J,KeyCode.U)} },
            { "HDpad",new List <ValAxis>{new ValAxis(KeyCode.Alpha2,KeyCode.Alpha1) } },
            { "VDpad",new List <ValAxis>{new ValAxis(KeyCode.Alpha4,KeyCode.Alpha3) } },
            { "triggerL",new List <ValAxis>{new ValAxis(KeyCode.Y,KeyCode.None) } },
            { "triggerR",new List <ValAxis>{new ValAxis(KeyCode.P,KeyCode.None) } },
            { "triggers",new List <ValAxis>{new ValAxis(KeyCode.P,KeyCode.Y) } }
        };


        public Dictionary<int, List<KeyCode>> DicKeys => dicKeys;

        public Dictionary<string, List<ValAxis>> DicAxis => dicAxis;
    }
}
