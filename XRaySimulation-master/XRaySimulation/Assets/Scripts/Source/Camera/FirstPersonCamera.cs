using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : ICameraController
{
    private Transform playerTransform;
    private Transform cameraTransform;

    private float xRotation;
    private float yRotation;

    private readonly float SPEED = 250f;
    private readonly float MAX_X_ROT = 60f;

    public FirstPersonCamera(Transform rotatedTransform, float yStartRotation)
    {
        this.playerTransform = rotatedTransform;
        this.cameraTransform = playerTransform.Find("FPV");

        this.xRotation = 0;
        this.yRotation = yStartRotation;
    }

    public void Rotate(float x, float y, float time, bool shift = false)
    {
        yRotation += x * time * SPEED;
        xRotation -= y * time * SPEED;

        SetXRotation(xRotation);

        playerTransform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void SetXRotation(float value)
    {
        xRotation = Mathf.Clamp(value, -MAX_X_ROT, MAX_X_ROT);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public float GetYRotation()
    {
        return yRotation;
    }
}
