using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BlocNarratif
{
    public string ID;
    public string Text;

    public BlocNarratif(string id, string text)
    {
        ID = id;
        Text = text;
    }
}
