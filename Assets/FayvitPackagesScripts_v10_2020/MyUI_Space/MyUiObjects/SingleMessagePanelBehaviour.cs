using FayvitUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMessagePanelBehaviour : MonoBehaviour
{
    [SerializeField] public SingleMessagePanel baseSingle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        baseSingle.ThisUpdate(Input.GetKeyDown(KeyCode.Return));
    }
}
