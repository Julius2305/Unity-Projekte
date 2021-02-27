using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class FurnitureTriggerInfo
{
    public static FurnitureType Type = FurnitureType.None;

    /// <summary>
    /// Activates the furniture info window.
    /// Sets the ifno content to the content of the given info object.
    /// </summary>
    public static void SetActiveFurniture(FurnitureType type, FurnitureInfo info)
    {
        Type = type;

        SetUI(info);
    }

    /// <summary>
    /// Deactivates the furniture info window.
    /// </summary>
    public static void DeactivateFurniture()
    {
        Type = FurnitureType.None;
        GameObject.Find("GUI").transform.Find("FurnitureInfo").gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the content of the info window to the correct infos of the FurnitureInfo object.
    /// Hides the windwo if the current funiture type is NONE.
    /// </summary>
    private static void SetUI(FurnitureInfo info)
    {
        GameObject infoGameObject = GameObject.Find("GUI").transform.Find("FurnitureInfo").gameObject;

        if (Type != FurnitureType.None)
        {
            infoGameObject.SetActive(true);

            TextMeshProUGUI name = infoGameObject.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI description = infoGameObject.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();

            name.text = info.Name;
            description.text = info.Description;

            SetKeys(info.KeyCodes);
        }
        else
        {
            infoGameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the key interaction texts to the names of the given keys.
    /// There should be maximum two KeyCodes.
    /// </summary>
    private static void SetKeys(KeyCode[] keys)
    {
        GameObject keysObject = GameObject.Find("GUI/FurnitureInfo/Keys");

        if (keys.Length > 2)
            Debug.LogWarning("The GUI can only handle two different keys!");
        
        for (int i = 0; i < 2; i++)
        {
            Transform keyTransform = keysObject.transform.GetChild(i);
            TextMeshProUGUI keyText = keyTransform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            if (keys.Length > i)
                keyText.text = keys[i].ToString();
            else
                keyText.text = "";
        }
    }
}
