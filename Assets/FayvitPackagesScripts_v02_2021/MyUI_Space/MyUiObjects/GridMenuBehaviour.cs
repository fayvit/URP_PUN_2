using UnityEngine;
using FayvitUI;
using FayvitSupportSingleton;

public class GridMenuBehaviour : MonoBehaviour
{
    [SerializeField] public GridMenu gMenu;
    [SerializeField] private Sprite[] imgOptions;

    // Use this for initialization
    void Start()
    {
        gMenu.StartHud(MyCallback, imgOptions);
    }

    private void MyCallback(int obj)
    {
        
    }

    public void RestartGridHud()
    {
        SupportSingleton.Instance.InvokeInRealTime(() =>
        {
            gMenu.FinishHud();
            gMenu.StartHud(MyCallback, imgOptions);
        },.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gMenu.IsActive)
        {
            int Vval = 0;
            int Hval = 0;

            if (Input.GetKeyDown(KeyCode.W))
            {
                Vval = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Vval = -1;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Hval = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Hval = 1;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                MyCallback(gMenu.SelectedOption);
            }

            gMenu.ChangeOption(Vval,Hval);
        }
    }
}