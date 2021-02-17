using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitMove
{
    public interface IFayvitMoveEvent
    {
        object[] MySendObjects { get; }
        FayvitMoveEventKey Key { get; }
    }
}