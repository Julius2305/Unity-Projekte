using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController
{
    public bool isTriggerd;

    protected Transform controlledTransform;

    public FurnitureController(Transform controlledTransform)
    {
        this.controlledTransform = controlledTransform;
    }

    /// <summary>
    /// Performs interaction with the furniture object.
    /// </summary>
    public virtual void Interact(bool[] instructions, float time)
    {

    }

    /// <summary>
    /// Activate the furnitrue.
    /// Sets the furniture info window.
    /// </summary>
    public void Activate(FurnitureType type, FurnitureInfo info)
    {
        if (FurnitureTriggerInfo.Type == FurnitureType.None)
        {
            isTriggerd = true;
            FurnitureTriggerInfo.SetActiveFurniture(type, info);
        }
    }

    /// <summary>
    /// Dectivate the furnitrue.
    /// Disables the furniture info window.
    /// </summary>
    public void Deactivate()
    {
        if (isTriggerd)
        {
            isTriggerd = false;
            FurnitureTriggerInfo.DeactivateFurniture();
        }
    }
}