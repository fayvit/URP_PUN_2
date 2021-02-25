using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyStringDict
{

    private static readonly Dictionary<string, int> dictStringForButtons = new Dictionary<string, int>()
    { 
        {"Action",0},
        {"Jump",1 },
        {"Run",2},
        {"Roll",3},
        {"LeftHandLight",4},
        {"RighthandLight",5},
        {"Select",6 },
        {"Start",7},
        {"LS",8},
        {"LockTargetCam",9}
    };

    private static readonly Dictionary<string, string> dictStringForAxis = new Dictionary<string, string>()
    {
        {"LightAttack","triggerR"},
        /*nomes padrão*/
        { "horizontal", "horizontal"},
        { "vertical","vertical"},
        { "Xcam","Xcam"},
        { "Ycam","Ycam"},
        { "HDpad","HDpad"},
        { "VDpad","VDpad"},
        { "triggerL","triggerL"},
        { "triggerR","triggerR"},
        { "triggers","triggers" }
    };

    public static string GetStringForAxis(string s)
    {
        if (dictStringForAxis.ContainsKey(s))
        {
            return dictStringForAxis[s];
        }
        else
        {
            Debug.Log("Chave não encontrada no dicionario de Strings");
            return s;
        }
    }

    public static int GetIntForString(string s)
    {
        if (dictStringForButtons.ContainsKey(s))
        {
            return dictStringForButtons[s];
        }
        else
        {
            Debug.Log("Chave não encontrada no dicionario de Strings");
            return 0;
        }
    }
}

public enum CommandKey
{ 
        Action,
        Jump,
        Run,
        Roll,
        LeftHandLight,
        RighthandLight,
        Select,
        Start,
        LS,
        LockTargetCam
}