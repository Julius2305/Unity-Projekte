using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FurnitureInfo
{
    public string Name;
    public string Description;
    public KeyCode[] KeyCodes;

    public FurnitureInfo()
    {
        Name = "";
        Description = "";
        KeyCodes = new KeyCode[] { };
    }

    public FurnitureInfo(string name, string description, KeyCode[] keys)
    {
        Name = name;
        Description = description;
        KeyCodes = keys;
    }
}