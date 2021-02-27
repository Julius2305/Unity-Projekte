using System;

public interface ICameraController
{
    void Rotate(float x, float y, float time, bool shift = false);
    //void SetXRotation(float value);
    float GetYRotation();
}
