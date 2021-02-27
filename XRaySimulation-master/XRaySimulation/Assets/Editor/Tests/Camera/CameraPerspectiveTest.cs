using System;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class CameraPerspectiveTest
{
    private GameObject mockObj;

    [SetUp]
    public void Setup()
    {
        mockObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player_Mock"));
    }

    [Test]
    public void FPVisExistent_Test()
    {
        Transform fpvTransform = mockObj.transform.Find("FPV");

        Assert.IsNotNull(fpvTransform);
    }

    [Test]
    public void TPVisExistent_Test()
    {
        Transform tpvTransform = mockObj.transform.Find("TPV");

        Assert.IsNotNull(tpvTransform);
    }

    [Test]
    public void PlayerCameraScriptIsExistent_Test()
    {
        PlayerCamera pr = mockObj.GetComponent<PlayerCamera>();

        Assert.IsNotNull(pr);
    }

    [Test]
    public void WhenFPVisActiveInputSetsTPVActive_Test()
    {
        // setup
        PlayerCamera pr = mockObj.GetComponent<PlayerCamera>();

        // set the private CameraController field of PlayerCamera, since it is set in the Start-Method
        System.Reflection.FieldInfo info = pr.GetType().GetField("cameraController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        info.SetValue(pr, new FirstPersonCamera(pr.transform, 0f));

        // perform
        pr.SwitchCamera();

        // get
        bool FPV_isActive = mockObj.transform.Find("FPV").gameObject.activeSelf;
        bool TPV_isActive = mockObj.transform.Find("TPV").gameObject.activeSelf;

        // assert
        Assert.IsTrue(TPV_isActive);
        Assert.IsFalse(FPV_isActive);
    }

    [Test]
    public void WhenTPVisActiveInputSetsFPVActive_Test()
    {
        // setup
        PlayerCamera pr = mockObj.GetComponent<PlayerCamera>();
        mockObj.transform.Find("FPV").gameObject.SetActive(false);
        mockObj.transform.Find("TPV").gameObject.SetActive(true);

        // set the CameraController field of PlayerCamera, since it is set in the Start-Method
        System.Reflection.FieldInfo info = pr.GetType().GetField("cameraController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        info.SetValue(pr, new ThirdPersonCamera(pr.transform, 0f));

        // perform
        pr.SwitchCamera();

        // get
        bool FPV_isActive = mockObj.transform.Find("FPV").gameObject.activeSelf;
        bool TPV_isActive = mockObj.transform.Find("TPV").gameObject.activeSelf;

        // assert
        Assert.IsFalse(TPV_isActive);
        Assert.IsTrue(FPV_isActive);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(mockObj);
    }
}
