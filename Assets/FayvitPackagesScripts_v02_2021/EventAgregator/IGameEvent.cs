using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitEventAgregator
{
    public interface IGameEvent
    {
        object[] MySendObjects { get; }
        EventKey Key { get; }
    }
}