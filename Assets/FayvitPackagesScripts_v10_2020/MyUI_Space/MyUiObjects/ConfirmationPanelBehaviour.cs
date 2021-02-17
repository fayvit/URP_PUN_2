using FayvitCommandReader;
using FayvitUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationPanelBehaviour : MonoBehaviour
{
    [SerializeField] public ConfirmationPanel confirmation;
    // Start is called before the first frame update
    void Start()
    {
        confirmation.StartConfirmationPanel(
            () => { Debug.Log("Yes pressed"); },
            () => { Debug.Log("No pressed"); }, "Ola");
    }

    // Update is called once per frame
    void Update()
    {
        bool change = false;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            change = true;

        confirmation.ThisUpdate(change, Input.GetKeyDown(KeyCode.Return), false);
    }
}
