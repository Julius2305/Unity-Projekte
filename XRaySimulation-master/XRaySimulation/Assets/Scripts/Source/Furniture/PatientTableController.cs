using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatientTableController : FurnitureController
{
    public PatientTableController(Transform controlledTransform) : base(controlledTransform)
    {
        
    }

    /// <summary>
    /// Moves the table up, or down accoring to the given input instructions.
    /// </summary>
    /// <param name="instructions"></param>
    /// <param name="time"></param>
    override public void Interact(bool[] instructions, float time)
    {
        if (instructions.Length < 2 || instructions.Length > 3)
            return;

        if (instructions[0])
            controlledTransform.Translate(Vector3.up * time);
        else if (instructions[1])
            controlledTransform.Translate(Vector3.down * time);
    }
}