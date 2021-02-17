using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRaycastPoint
{
    public static bool GetPoint(out Vector3 point)
    { 
        Ray origin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool retorno = false;
        point = Vector3.zero;

        if (Physics.Raycast(origin, out hit))
        {
            retorno = true;
            point = hit.point;
        }

        return retorno;
    }
}
