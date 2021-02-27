using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DoorControllerTest
{
    private GameObject testObj;
    private Furniture furniture;
    private FurnitureController controller;

    [SetUp]
    public void SetUp()
    {
        // door mock
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Door");
        testObj = GameObject.Instantiate(prefab);
        testObj.name = "Door";

        furniture = testObj.GetComponent<Furniture>();
        controller = new DoorController(testObj.transform);
    }

    /// <summary>
    /// Enables to get the value of a private field with given name.
    /// Returns the value of the field.
    /// </summary>
    private T GetPrivateField<T>(string fieldName)
    {
        System.Reflection.FieldInfo info = controller.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)info.GetValue(controller);
    }

    [Test]
    public void TestObjectIsNotNull_Test()
    {
        Assert.IsNotNull(testObj);
    }

    [Test]
    public void FurnitrueIsNotNull_Test()
    {
        Assert.IsNotNull(furniture);
    }

    [Test]
    public void ControllerIsNotNull_Test()
    {
        Assert.IsNotNull(controller);
    }

    [Test]
    public void LeftDoorIsNotNull_Test()
    {
        Transform left = GetPrivateField<Transform>("leftDoor");

        Assert.IsNotNull(left);
    }

    [Test]
    public void RightDoorIsNotNull_Test()
    {
        Transform right = GetPrivateField<Transform>("rightDoor");

        Assert.IsNotNull(right);
    }

    [Test]
    [TestCase(new bool[] { })]
    [TestCase(new bool[] { true, true })]
    [TestCase(new bool[] { false, false })]
    public void NothingWillHappenWhenWrongAmountOfInstructionsGiven_Test(bool[] instructions)
    {
        // perform
        controller.Interact(instructions, 1f);

        // get
        bool isOpen = GetPrivateField<bool>("isOpen");

        // assert
        Assert.IsFalse(isOpen);
    }

    [Test]
    public void FirstPressOfFWillSetIsOpenToTrue_Test()
    {
        // perform
        controller.Interact(new bool[] { true }, 1f);

        // get
        bool isOpen = GetPrivateField<bool>("isOpen");

        // assert
        Assert.IsTrue(isOpen);
    }

    [Test]
    public void SecondPressOfFWillSetIsOpenFalse_Test()
    {
        // perform
        controller.Interact(new bool[] { true }, 1f);
        controller.Interact(new bool[] { true }, 1f);

        // get
        bool isOpen = GetPrivateField<bool>("isOpen");

        // assert
        Assert.IsFalse(isOpen);
    }

    [Test]
    public void FirstPressOfFWillOpenDoor_Test()
    {
        // setup
        Transform left = GetPrivateField<Transform>("leftDoor");
        Transform right = GetPrivateField<Transform>("rightDoor");

        Vector3 leftBefore = left.localPosition;
        Vector3 rightBefore = right.localPosition;

        // perform
        controller.Interact(new bool[] { true }, 1);

        // get
        Vector3 leftAfter = left.localPosition;
        Vector3 rightAfter = right.localPosition;

        // assert
        Assert.Less(leftAfter.z, leftBefore.z);
        Assert.Greater(rightAfter.z, rightBefore.z);
    }

    [Test]
    public void SecondPressOfFWillCloseDoor_Test()
    {
        // setup
        Transform left = GetPrivateField<Transform>("leftDoor");
        Transform right = GetPrivateField<Transform>("rightDoor");

        Vector3 leftBefore = left.localPosition;
        Vector3 rightBefore = right.localPosition;

        // perform
        controller.Interact(new bool[] { true }, 1);
        controller.Interact(new bool[] { true }, 1);

        // get
        Vector3 leftAfter = left.localPosition;
        Vector3 rightAfter = right.localPosition;

        // assert
        Assert.AreEqual(leftAfter.z, leftBefore.z);
        Assert.AreEqual(rightAfter.z, rightBefore.z);
    }


    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(testObj);
        controller = null;
    }
}