using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTestPUN_2
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private ControlledMoveForCharacter thisControl;
        [SerializeField] private float distanciaChecaMovimento = 1.2f;

        // Start is called before the first frame update
        void Start()
        {
            thisControl.StartFields(transform);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Meu input");
                Vector3 V;
                if (GetRaycastPoint.GetPoint(out V))
                {
                    Debug.Log("meu V: " + V);
                    thisControl.ModificarOndeChegar(V);
                }
            }

            if (thisControl.UpdatePosition(distanciaChecaMovimento))
            { 
            
            }
        }
    }

    
}
