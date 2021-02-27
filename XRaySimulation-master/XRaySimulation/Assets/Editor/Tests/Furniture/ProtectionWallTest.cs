using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ProtectionWallTest
{
    private GameObject testObj;
    private Furniture furniture;
    private FurnitureController controller;

    [SetUp]
    public void Setup()
    {
        // protection wall mock
        GameObject prefab = Resources.Load<GameObject>("Prefabs/ProtectionWall");
        testObj = GameObject.Instantiate(prefab);
        testObj.name = "ProtectionWall";

        furniture = testObj.GetComponent<Furniture>();
        controller = new MoveableController(testObj.transform);
    }

    [Test]
    public void FirstPressFWillLockTheWallToThePlayerMovementWhenIsTriggered_Test()
    {
        // setup
        // player mock
        GameObject playerObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player_Mock"));
        playerObj.name = "Player";

        // perform
        controller.Interact(new bool[] { true }, 1f);

        // assert
        Assert.IsNotNull(playerObj.transform.Find("ProtectionWall"));

        // cleanup
        GameObject.DestroyImmediate(playerObj);
    }

    [Test]
    public void SecondPressFWillReleaseTheWallFromThePlayerMovementWhenIsTriggerd_Test()
    {
        // setup
        // player mock
        GameObject playerObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player_Mock"));
        playerObj.name = "Player";

        // perform
        controller.Interact(new bool[] { true }, 1f);

        // assert
        Assert.IsNotNull(playerObj.transform.Find("ProtectionWall"));

        // perform
        controller.Interact(new bool[] { true }, 1f);

        // assert
        Assert.IsNull(playerObj.transform.Find("ProtectionWall"));
        Assert.IsNotNull(GameObject.Find("ProtectionWall"));

        // cleanup
        GameObject.DestroyImmediate(playerObj);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(testObj);
        controller = null;
    }
}