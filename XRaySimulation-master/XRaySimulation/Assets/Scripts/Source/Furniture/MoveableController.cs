using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveableController : FurnitureController
{
    private bool isMoved;

    public MoveableController(Transform conntrolledTransform) : base(conntrolledTransform)
    {
        isMoved = false;
    }

    public override void Interact(bool[] instructions, float time)
    {
        if (instructions.Length < 1 || instructions.Length > 1)
            return;

        if (instructions[0])
            MoveInsidePlayer();
    }

    /// <summary>
    /// Sets the transform parent to the interacting player.
    /// Allows movement of the furniture.
    /// </summary>
    private void MoveInsidePlayer()
    {
        GameObject player = GameObject.Find("Player");

        if (!isMoved)
        {
            isMoved = true;
            controlledTransform.SetParent(player.transform);
        }
        else
        {
            isMoved = false;
            controlledTransform.SetParent(player.transform.parent);
        }
    }

}
