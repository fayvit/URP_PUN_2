using UnityEngine;
using System.Collections;
using FayvitUI;

public class TextDisplayBehaviour : MonoBehaviour
{
    [SerializeField] private TextDisplay textDisplay;
    private string[] s = new string[4] {
    "bom dia ",
    "bom dia pra vc",
    "bom diaaaaa...",
    "bom dia pra vc"
    };

    // Use this for initialization
    void Start()
    {
        textDisplay.StartTextDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        bool b = Input.GetKeyDown(KeyCode.Return);

        if (textDisplay.UpdateTexts(b, false, s))
        {
            Debug.Log("Acabou");
        }
    }
}
