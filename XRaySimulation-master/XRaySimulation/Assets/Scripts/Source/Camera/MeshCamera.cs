using System;
using UnityEngine;

public class MeshCamera : ICameraController
{
    private Transform meshCameraTransform;

    private float xRotation;
    private float yRotation;

    private readonly float SPEED = 250f;
    private readonly float MAX_X_ROT = 60f;
    private readonly float MIN_X_ROT = -60f;

    public MeshCamera(Transform rotatedTransform, float yStartRotation)
    {
        this.meshCameraTransform = rotatedTransform;

        this.xRotation = 0f;
        this.yRotation = yStartRotation;
    }

    public void Rotate(float x, float y, float time, bool shift = false)
    {
        yRotation += x * time * SPEED;
        xRotation -= y * time * SPEED;

        xRotation = Mathf.Clamp(xRotation, MIN_X_ROT, MAX_X_ROT);

        meshCameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public float GetYRotation()
    {
        return yRotation;
    }
}
