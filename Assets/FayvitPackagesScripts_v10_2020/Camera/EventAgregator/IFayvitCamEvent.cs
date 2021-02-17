using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitCam
{
    public interface IFayvitCamEvent
    {
        object[] MySendObjects { get; }
        FayvitCamEventKey Key { get; }
    }
}