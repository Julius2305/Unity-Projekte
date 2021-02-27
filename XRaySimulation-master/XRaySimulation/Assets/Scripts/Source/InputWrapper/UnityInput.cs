using System;
using UnityEngine;

public class UnityInput : IInputWrapper
{
    private static IInputWrapper input;
    public static IInputWrapper Instance
    {
        get
        {
            if (input == null)
                input = new UnityInput();

            return input;
        }
    }

    /// <summary>
    /// Returns true, while the user holds down the key identified by the KeyCode.
    /// </summary>
    public bool GetKey(KeyCode code)
    {
        return Input.GetKey(code);
    }

    /// <summary>
    /// Returns true during the frame the user starts pressing this key identified by the KeyCode.
    /// </summary>
    public bool GetKeyDown(KeyCode code)
    {
        return Input.GetKeyDown(code);
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by the axisname.
    /// </summary>
    public float GetAxis(string axis)
    {
        return Input.GetAxis(axis);
    }
}
