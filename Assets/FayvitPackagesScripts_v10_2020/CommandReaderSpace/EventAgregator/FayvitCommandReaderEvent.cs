using UnityEngine;
using System.Collections;

namespace FayvitCommandReader
{
    public class FayvitCommandReaderEvent : IFayvitCommandReaderEvent
    {
        public object[] MySendObjects { get; private set; }

        public FayvitCR_EventKey Key { get; private set; }

        public FayvitCommandReaderEvent(FayvitCR_EventKey key, params object[] o)
        {
            Key = key;
            MySendObjects = o;
        }
    }
}
