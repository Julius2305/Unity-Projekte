using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FurnitureTest
{
    private GameObject testObj;
    private Furniture furniture;
    private FurnitureController controller;
    private GameObject playerObj;
    private GameObject gui;

    private const float RADIUS = 2f;

    [SetUp]
    public void Setup()
    {
        //Furniture
        GameObject prefab = Resources.Load<GameObject>("Prefabs/fixedTable");
        testObj = GameObject.Instantiate(prefab);
        furniture = testObj.GetComponent<Furniture>();
        controller = furniture.Controller;
        testObj.GetComponent<SphereCollider>().radius = RADIUS;

        //Player
        prefab = Resources.Load<GameObject>("Prefabs/Player_Mock");
        playerObj = GameObject.Instantiate(prefab);
        playerObj.name = "Player";

        //GUI
        prefab = Resources.Load<GameObject>("Prefabs/UI/GUI");
        gui = GameObject.Instantiate(prefab);
        gui.name = "GUI";
    }

    [Test]
    public void FurnitureIsExistent_Test()
    {
        Assert.IsNotNull(furniture);
    }

    [Test]
    public void FurnitureHasSphereCollider_Test()
    {
        Assert.IsNotNull(testObj.GetComponent<SphereCollider>());
    }

    [Test]
    public void FurnitureColliderIsTrigger_Test()
    {
        Assert.IsTrue(testObj.GetComponent<SphereCollider>().isTrigger);
    }

    [UnityTest]
    public IEnumerator FurnitureGetsTriggerdThroughPlayer_Test()
    {
        // setup
        playerObj.transform.position = new Vector3(RADIUS - RADIUS / 2f, 0f, 0f);

        // perform
        // Wait till unity calculated the physics once
        yield return new WaitForFixedUpdate();

        // assert
        Assert.IsNotNull(playerObj.GetComponent<BoxCollider>());
        Assert.IsTrue(controller.isTriggerd);

        // cleanup
        GameObject.Destroy(playerObj);
    }

    [UnityTest]
    public IEnumerator FurnitureGetsTriggerdThroughPlayerWhenPlayerIsAtTriggerSphereEdge_Test()
    {
        // setup
        playerObj.transform.position = new Vector3(RADIUS, 0f, 0f);

        // perform
        // Wait till unity calculated the physics once
        yield return new WaitForFixedUpdate();

        // assert
        Assert.IsNotNull(playerObj.GetComponent<BoxCollider>());
        Assert.IsTrue(controller.isTriggerd);

        // cleanup
        GameObject.Destroy(playerObj);
    }

    [UnityTest]
    public IEnumerator JustOneFurnitureGetsTriggeredThroughPlayer_Test()
    {
        // setup
        playerObj.transform.position = new Vector3(RADIUS, 0f, 0f);

        // Wait till unity calculated the physics once
        yield return new WaitForFixedUpdate();

        // Instantiate a second table, in a distance near the player, so it can get triggerd
        GameObject secondTable = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/fixedTable"));
        Furniture secondFurniture = secondTable.GetComponent<Furniture>();
        FurnitureController secondController = secondFurniture.Controller;
        secondTable.transform.position = new Vector3(2f * RADIUS, 0f, 0f);

        // perform
        // Wait till unity calculated the physics once
        yield return new WaitForFixedUpdate();

        // assert
        Assert.IsNotNull(playerObj.GetComponent<BoxCollider>());
        Assert.IsTrue(controller.isTriggerd);
        Assert.IsFalse(secondController.isTriggerd);

        // cleanup
        GameObject.Destroy(secondTable);
        GameObject.Destroy(playerObj);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.Destroy(testObj);

        FurnitureTriggerInfo.DeactivateFurniture();
        GameObject.Destroy(gui);
    }
}
