using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : FurnitureController
{
    private Transform leftDoor;
    private Transform rightDoor;
    private bool isOpen;

    private Vector3 LEFT_CLOSED;
    private Vector3 RIGHT_CLOSED;
    private Vector3 openAmount = new Vector3(0f, 0f, 0.75f);


    public DoorController(Transform controlledTransform) : base(controlledTransform)
    {
        this.leftDoor = this.controlledTransform.Find("Left");
        this.rightDoor = this.controlledTransform.Find("Right");

        LEFT_CLOSED = this.leftDoor.localPosition;
        RIGHT_CLOSED = this.rightDoor.localPosition;

        isOpen = false;
    }

    public override void Interact(bool[] instructions, float time)
    {
        if (instructions.Length < 1 || instructions.Length > 1)
            return;

        if (instructions[0])
            OpenCloseDoor();
    }

    /// <summary>
    /// Opens the door when closed.
    /// Closes the door when open.
    /// </summary>
    private void OpenCloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            leftDoor.localPosition = LEFT_CLOSED;
            rightDoor.localPosition = RIGHT_CLOSED;
        }
        else
        {
            isOpen = true;
            leftDoor.localPosition = LEFT_CLOSED - openAmount;
            rightDoor.localPosition = RIGHT_CLOSED + openAmount;
        }
    }
}