using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTestPUN_2
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private ControlledMoveForCharacter thisControl;
        [SerializeField] private float distanciaChecaMovimento = 1.2f;

        private PhotonView pv;

        // Start is called before the first frame update
        void Start()
        {
            thisControl.StartFields(transform);
            pv = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
            if (pv.IsMine)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 V;
                    if (GetRaycastPoint.GetPoint(out V))
                    {
                        thisControl.ModificarOndeChegar(V);
                    }
                }

                if (thisControl.UpdatePosition(distanciaChecaMovimento))
                {

                }
            }
        }
    } 
}
