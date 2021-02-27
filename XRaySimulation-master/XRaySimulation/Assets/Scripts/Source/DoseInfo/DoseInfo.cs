using System;
using System.Collections.Generic;
using UnityEngine;

public class DoseInfo : MonoBehaviour
{
    public static DoseInfo Instance;
    public DoseInfoController Controller;

    private void Start()
    {
        Instance = this;

        Transform meshCameraTransform = GameObject.Find("Player/MeshCamera").transform;

        Controller = new DoseInfoController(this.transform, meshCameraTransform);
    }

    private void Update()
    {
        RotateMeshCamera();    
    }

    /// <summary>
    /// Rotates the MeshCamera according to the pressed keys this frame.
    /// </summary>
    private void RotateMeshCamera()
    {
        float x = 0f;
        float y = 0f;

        if (UnityInput.Instance.GetKey(KeyCode.UpArrow))
            y += 1;
        if (UnityInput.Instance.GetKey(KeyCode.DownArrow))
            y -= 1;
        if (UnityInput.Instance.GetKey(KeyCode.RightArrow))
            x += 1;
        if (UnityInput.Instance.GetKey(KeyCode.LeftArrow))
            x -= 1;

        Controller.Rotate(x, y, Time.deltaTime);
    }
}