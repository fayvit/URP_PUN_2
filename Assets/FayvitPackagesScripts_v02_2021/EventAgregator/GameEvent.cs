using UnityEngine;
using System.Collections;

namespace FayvitEventAgregator
{
    public class GameEvent : IGameEvent
    {
        public object[] MySendObjects { get; private set; }

        public EventKey Key { get; private set; }

        public GameEvent(EventKey key, params object[] o)
        {
            Key = key;
            MySendObjects = o;
        }
    }
}
