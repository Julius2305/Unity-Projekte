using UnityEngine;

public interface IInputWrapper
{
    bool GetKey(KeyCode code);
    bool GetKeyDown(KeyCode code);

    float GetAxis(string axis);

}