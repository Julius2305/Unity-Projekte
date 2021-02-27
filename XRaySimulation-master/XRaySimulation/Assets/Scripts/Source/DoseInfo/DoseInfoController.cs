using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoseInfoController
{
    private Transform infoTransform;
    private TextMeshProUGUI avgTextMesh;
    private TextMeshProUGUI activeTextMesh;
    private Transform meshCameraTransform;
    private MeshCamera cameraController;

    private readonly string UNIT = " Gy";
    private readonly string ACTIVE = " active";
    private readonly string INACTIVE = " inactive";

    public DoseInfoController(Transform transform, Transform meshCameraTransform)
    {
        this.infoTransform = transform;

        this.avgTextMesh = infoTransform.Find("AVG Dose").GetComponent<TextMeshProUGUI>();
        this.activeTextMesh = infoTransform.Find("Active Text").GetComponent<TextMeshProUGUI>();
        this.meshCameraTransform = meshCameraTransform;

        this.cameraController = new MeshCamera(this.meshCameraTransform, 0f);
    }

    /// <summary>
    /// Sets the "AVGDose" text of the DoseInfo window to the argument.
    /// </summary>
    public void SetAVGDose(float dose)
    {
        avgTextMesh.text = dose + UNIT;
    }

    /// <summary>
    /// Sets the "Active" text of the DoseInfo window.
    /// When active "active" is shown, otherwise "inactive".
    /// </summary>
    public void SetSourceActiveText(bool isActive)
    {
        if (isActive)
            activeTextMesh.text = ACTIVE;
        else
            activeTextMesh.text = INACTIVE;
    }

    /// <summary>
    /// Rotates the MeshCamera according to the given inputs.
    /// </summary>
    public void Rotate(float x, float y, float time)
    {
        cameraController.Rotate(x, y, time);
    }
}
