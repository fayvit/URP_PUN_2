using UnityEngine;
using FayvitUI;

public class MenuReference : MonoBehaviour
{
    [SerializeField] public BasicMenu bMenu;
    /*
    [SerializeField] private string[] stringOptions;

    private void Start()
    {        
        bMenu.StartHud(MyCallback, stringOptions);
    }

    private void MyCallback(int obj)
    {
        Debug.Log(((KeyCode)obj).ToString());

        bMenu.FinishHud();
    }

    private void Update()
    {
        if (bMenu.IsActive)
        {
            int val = 0;

            if (Input.GetKeyDown(KeyCode.W))
            {
                val = -1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                val = 1;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                MyCallback(bMenu.SelectedOption);
            }

            bMenu.ChangeOption(val);
        }
    }
    */
}
