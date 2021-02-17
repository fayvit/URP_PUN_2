using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitCommandReader
{
    public class N3DS_KeysDic : IKeyDict
    {
        private static N3DS_KeysDic instance;
        public static N3DS_KeysDic Instance
        {
            get
            {
                if (instance == null)
                    instance = new N3DS_KeysDic();

                return instance;
            }
        }

        private N3DS_KeysDic() { }

        public Dictionary<int, List<KeyCode>> DicKeys => dicKeys;

        public Dictionary<string, List<ValAxis>> DicAxis => dicAxis;

        public Dictionary<int, List<KeyCode>> dicKeys = new Dictionary<int, List<KeyCode>>()
        {
            { 0,new List<KeyCode>{ KeyCode.A} },
            { 1,new List<KeyCode>{KeyCode.B} },
            { 2,new List<KeyCode>{KeyCode.Y} },
            { 3,new List<KeyCode>{KeyCode.X} },
            { 4,new List<KeyCode>{KeyCode.L} },
            { 5,new List<KeyCode>{KeyCode.R} },
            { 6,new List<KeyCode>{KeyCode.Escape} },
            { 7,new List<KeyCode>{KeyCode.Return} },
            { 8,new List<KeyCode>{KeyCode.LeftAlt} },
            { 9,new List<KeyCode>{KeyCode.RightAlt} }
        };

        public Dictionary<string, List<ValAxis>> dicAxis = new Dictionary<string, List<ValAxis>>()
        {
            { "horizontal",new List<ValAxis>{new ValAxis(KeyCode.Keypad6,KeyCode.Keypad4) } },
            { "vertical",new List<ValAxis>{new ValAxis(KeyCode.Keypad8,KeyCode.Keypad2)} },
            { "Xcam",new List<ValAxis>{new ValAxis(KeyCode.Alpha4,KeyCode.Alpha3)} },
            { "Ycam",new List<ValAxis>{new ValAxis(KeyCode.Alpha2,KeyCode.Alpha1)} },
            { "HDpad",new List<ValAxis>{new ValAxis(KeyCode.RightArrow,KeyCode.LeftArrow) } },
            { "VDpad",new List<ValAxis>{new ValAxis(KeyCode.UpArrow,KeyCode.DownArrow) } }
        };
    }

    public struct ValAxis
    {
        public KeyCode pos;
        public KeyCode neg;

        public ValAxis(KeyCode pos, KeyCode neg)
        {
            this.pos = pos;
            this.neg = neg;
        }
    }
}