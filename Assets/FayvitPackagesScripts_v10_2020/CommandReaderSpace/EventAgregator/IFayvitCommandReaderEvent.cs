using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitCommandReader
{
    public interface IFayvitCommandReaderEvent
    {
        object[] MySendObjects { get; }
        FayvitCR_EventKey Key { get; }
    }
}