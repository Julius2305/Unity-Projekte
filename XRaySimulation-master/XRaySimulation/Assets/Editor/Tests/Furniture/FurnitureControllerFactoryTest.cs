using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FurnitureControllerFactoryTest
{
    [Test]
    public void FactoryReturnIsNotNull_Test()
    { 
        // setup
        GameObject obj = new GameObject();

        // perform
        FurnitureController controller = FurnitureControllerFactory.Create(FurnitureType.None, obj.transform);

        // assert
        Assert.IsNotNull(controller);
    }

    [Test]
    [TestCase(typeof(PatientTableController), FurnitureType.PatientTable)]
    [TestCase(typeof(MoveableController), FurnitureType.ProtectionWall)]
    [TestCase(typeof(MoveableController), FurnitureType.Table)]
    public void ReturnedControllerIsOfCorrectType_Test(Type type, FurnitureType furnitureType)
    {
        // setup
        GameObject obj = new GameObject();
        
        // perform
        FurnitureController controller = FurnitureControllerFactory.Create(furnitureType, obj.transform);

        // assert
        Assert.AreEqual(type, controller.GetType());
    }
}