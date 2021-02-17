using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public interface IKeyDict
    {
        Dictionary<int, List<KeyCode>> DicKeys { get; }
        Dictionary<string, List<ValAxis>> DicAxis { get; }
    }
}