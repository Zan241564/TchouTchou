using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class BiomeData : ScriptableObject
{
    public Texture2D train;
    public Texture2D background;
    public Texture2D ground;
    public Texture2D sky;
    public Texture2D clouds;
    public int ID;
    public string biomeName;
}
