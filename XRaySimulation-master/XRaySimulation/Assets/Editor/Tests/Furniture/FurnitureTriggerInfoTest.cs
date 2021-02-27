using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class FurnitureTriggerInfoTest
{
    private GameObject gui;

    [SetUp]
    public void Setup()
    {
        // create a GUI object
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/GUI");
        gui = GameObject.Instantiate(prefab);
        gui.name = "GUI";
    }

    [Test]
    [TestCase(FurnitureType.CArm)]
    [TestCase(FurnitureType.Closet)]
    [TestCase(FurnitureType.Door)]
    [TestCase(FurnitureType.PatientTable)]
    [TestCase(FurnitureType.ProtectionWall)]
    [TestCase(FurnitureType.Table)]
    [TestCase(FurnitureType.None)]
    public void SetActiveFurnitureSetsCorrectFurnitureType_Test(FurnitureType expected)
    {
        // setup
        FurnitureInfo info = new FurnitureInfo(expected.ToString(), expected.ToString(), new KeyCode[] { });

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(expected, info);

        // assert
        Assert.AreEqual(expected, FurnitureTriggerInfo.Type);
    }

    [Test]
    public void DeactivateFunitureSetsTypeToNone_Test()
    {
        // setup
        FurnitureTriggerInfo.Type = FurnitureType.CArm;

        // perform
        FurnitureTriggerInfo.DeactivateFurniture();

        // assert
        Assert.AreEqual(FurnitureType.None, FurnitureTriggerInfo.Type);
    }

    [Test]
    public void SetActiveFurnitureActivatesFurnitureInfoUI_Test()
    {
        // setup
        FurnitureInfo info = new FurnitureInfo("example", "example", new KeyCode[] { });

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(FurnitureType.CArm, info);

        // get
        bool active = GameObject.Find("GUI/FurnitureInfo").activeSelf;

        // assert
        Assert.IsTrue(active);
    }

    [Test]
    public void DeactivateFurnitureDeactivatesFurnitureInfoUI_Test()
    {
        // perform
        FurnitureTriggerInfo.DeactivateFurniture();

        // get
        bool active = GameObject.Find("GUI").transform.Find("FurnitureInfo").gameObject.activeSelf;

        // assert
        Assert.IsFalse(active);
    }

    [Test]
    [TestCase(FurnitureType.CArm)]
    public void SetActiveFurnitureLoadsCorrectFurnitureNameIntoUI_Test(FurnitureType type)
    {
        // setup
        string expected = type.ToString();
        FurnitureInfo info = new FurnitureInfo(expected, expected, new KeyCode[] { });

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(type, info);

        // get
        string actual = GameObject.Find("GUI/FurnitureInfo/Name").GetComponent<TextMeshProUGUI>().text;

        // assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    [TestCase(FurnitureType.CArm)]
    public void SetActiveFurnitureLoadsCorrectFurnitureDescriptionIntoUI_Test(FurnitureType type)
    {
        // setup
        string expected = type.ToString();
        FurnitureInfo info = new FurnitureInfo(expected, expected, new KeyCode[] { });

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(type, info);

        //get
        string actual = GameObject.Find("GUI/FurnitureInfo/Description").GetComponent<TextMeshProUGUI>().text;

        // assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    [TestCase(FurnitureType.CArm, new KeyCode[] { KeyCode.A, KeyCode.F })]
    [TestCase(FurnitureType.CArm, new KeyCode[] { KeyCode.B, KeyCode.DownArrow })]
    public void SetActiveFurnitureLoadsCorrectKeyCodesIntoUIForTwoKeys_Test(FurnitureType type, KeyCode[] keys)
    {
        // setup
        string expected = type.ToString();
        FurnitureInfo info = new FurnitureInfo(expected, expected, keys);

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(type, info);

        // get
        string actual0 = GameObject.Find("GUI/FurnitureInfo/Keys/1/KeyName").GetComponent<TextMeshProUGUI>().text;
        string actual1 = GameObject.Find("GUI/FurnitureInfo/Keys/2/KeyName").GetComponent<TextMeshProUGUI>().text;

        // assert
        Assert.AreEqual(keys[0].ToString(), actual0);
        Assert.AreEqual(keys[1].ToString(), actual1);
    }

    [Test]
    [TestCase(FurnitureType.CArm, KeyCode.A)]
    [TestCase(FurnitureType.Table, KeyCode.DownArrow)]
    public void SetActiveFurnitureLoadsCorrectKeyCodesIntoUIForOneKey_Test(FurnitureType type, KeyCode keyCode)
    {
        // setup
        string expected = keyCode.ToString();
        string name = type.ToString();
        FurnitureInfo info = new FurnitureInfo(name, name, new KeyCode[] { keyCode });

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(FurnitureType.CArm, info);

        // get
        string actual0 = GameObject.Find("GUI/FurnitureInfo/Keys/1/KeyName").GetComponent<TextMeshProUGUI>().text;
        string actual1 = GameObject.Find("GUI/FurnitureInfo/Keys/2/KeyName").GetComponent<TextMeshProUGUI>().text;

        // assert
        Assert.AreEqual(expected, actual0);
        Assert.AreEqual("", actual1);
    }

    [Test]
    [TestCase(FurnitureType.CArm)]
    [TestCase(FurnitureType.Table)]
    public void SetActiveFurnitureLoadsCorrectKeyCodesIntoUIForZeroKeys_Test(FurnitureType type)
    {
        // setup
        string expected = "";
        string name = type.ToString();
        FurnitureInfo info = new FurnitureInfo(name, name, new KeyCode[] {  });

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(FurnitureType.CArm, info);

        // get
        string actual0 = GameObject.Find("GUI/FurnitureInfo/Keys/1/KeyName").GetComponent<TextMeshProUGUI>().text;
        string actual1 = GameObject.Find("GUI/FurnitureInfo/Keys/2/KeyName").GetComponent<TextMeshProUGUI>().text;

        // assert
        Assert.AreEqual(expected, actual0);
        Assert.AreEqual(expected, actual1);
    }

    [Test]
    public void SetActiveFurnitureThrowsWarningMessageWhenFurnitureInfoHasMoreThanTwoKeyCodes_Test()
    {
        // setup
        FurnitureInfo info = new FurnitureInfo("example", "example", new KeyCode[] { KeyCode.A, KeyCode.B, KeyCode.C });

        // perform
        FurnitureTriggerInfo.SetActiveFurniture(FurnitureType.CArm, info);

        // assert
        LogAssert.Expect(LogType.Warning, "The GUI can only handle two different keys!");
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(gui);
    }
}