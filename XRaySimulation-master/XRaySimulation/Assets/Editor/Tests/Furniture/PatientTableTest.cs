using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PatientTableTest
{
    private GameObject testObj;
    private Furniture furniture;
    private FurnitureController controller;

    [SetUp]
    public void Setup()
    {
        // patienttable mock
        testObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PatientTable"));
        furniture = testObj.GetComponent<Furniture>();
        controller = new PatientTableController(testObj.transform);
        controller.isTriggerd = true;
        furniture.Controller = controller;
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
    [TestCase(true, false)]
    public void ArrowUpMovesTableUpWhenTableIsTriggerd_Test(bool up, bool down)
    {
        // setup
        Vector3 before = testObj.transform.position;

        // perform
        controller.Interact(new bool[] { up, down }, 1f);

        // get
        Vector3 after = testObj.transform.position;

        // assert
        Assert.Greater(after.y, before.y);
    }

    [Test]
    [TestCase(false, true)]
    public void ArrowDownMovesTableDownWhenTableIsTriggerd_Test(bool up, bool down)
    {
        // setup
        Vector3 before = testObj.transform.position;

        // perform
        controller.Interact(new bool[] { up, down }, 1f);

        // get
        Vector3 after = testObj.transform.position;

        // assert
        Assert.Less(after.y, before.y);
    }

    [Test]
    [TestCase(true, true)]
    public void ArrowUpAndArrowDownArePressedMovesTableUpWhenTableIsTriggerd_Test(bool up, bool down)
    {
        // setup
        Vector3 before = testObj.transform.position;

        // perform
        controller.Interact(new bool[] { up, down }, 1f);

        // get
        Vector3 after = testObj.transform.position;

        // assert
        Assert.Greater(after.y, before.y);
    }

    [Test]
    [TestCase(new bool[] { false, false, false })]
    [TestCase(new bool[] { true, true, true, true, true })]
    public void TheTableDontMoveWhenThereAreTooManyInstructions_Test(bool[] instructions)
    {
        // setup
        Vector3 before = testObj.transform.position;

        // perform
        controller.Interact(instructions, 1f);

        // get
        Vector3 after = testObj.transform.position;

        // assert
        Assert.AreEqual(after.y, before.y);
    }

    [Test]
    [TestCase(new bool[] { false })]
    [TestCase(new bool[] { true })]
    [TestCase(new bool[] { })]
    public void TheTableDontMoveWhenThereAreNotEnoughInstructions_Test(bool[] instructions)
    {
        // setup
        Vector3 before = testObj.transform.position;

        // perform
        controller.Interact(instructions, 1f);

        // get
        Vector3 after = testObj.transform.position;

        // assert
        Assert.AreEqual(after.y, before.y);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(testObj);
        controller = null;
    }
}
