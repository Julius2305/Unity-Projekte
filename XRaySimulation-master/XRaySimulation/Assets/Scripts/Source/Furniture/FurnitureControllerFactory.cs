using System;
using System.Collections.Generic;
using UnityEngine;

public static class FurnitureControllerFactory
{
    /// <summary>
    /// Returns an instance of a FurnitureController according to the correct type.
    /// </summary>
    public static FurnitureController Create(FurnitureType type, Transform transform)
    {
        switch (type)
        {
            case FurnitureType.None:
                return new FurnitureController(transform);
            case FurnitureType.CArm:
                return new FurnitureController(transform);
            case FurnitureType.Closet:
                return new FurnitureController(transform);
            case FurnitureType.Door:
                return new DoorController(transform);
            case FurnitureType.PatientTable:
                return new PatientTableController(transform);
            case FurnitureType.ProtectionWall:
                return new MoveableController(transform);
            case FurnitureType.Table:
                return new MoveableController(transform);
        }

        return new FurnitureController(transform);
     }
}

