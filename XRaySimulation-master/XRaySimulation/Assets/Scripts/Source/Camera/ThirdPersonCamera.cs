using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThirdPersonCamera : ICameraController
{
    private Transform playerTransform;
    private Transform tpvTransform;

    private float xRotation;
    private float yRotation;

    private readonly float SPEED = 250f;
    private readonly float MAX_X_ROT = 60f;
    private readonly float MIN_X_ROT = -60f;

    public ThirdPersonCamera(Transform rotatedTransform, float yStartRotation)
    {
        this.playerTransform = rotatedTransform;
        this.tpvTransform = playerTransform.Find("TPV");

        this.xRotation = 0f;
        this.yRotation = yStartRotation;
    }

    public void Rotate(float x, float y, float time, bool shift = false) 
    {
        yRotation += x * time * SPEED;
        xRotation -= y * time * SPEED;

        //cameraTransform.LookAt(tpvTransform);

        if (shift)
        {
            SetXRotation();
        }
        else 
        {
            SetXRotation();

            playerTransform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }

    public void SetXRotation()
    {
        xRotation = Mathf.Clamp(xRotation, MIN_X_ROT, MAX_X_ROT);

        tpvTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public float GetYRotation()
    {
        return yRotation;
    }
}
