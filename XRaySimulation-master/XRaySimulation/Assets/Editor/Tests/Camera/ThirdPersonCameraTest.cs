using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ThirdPersonCameraTest
{
    private Transform mock;
    private Transform cameraTransform;
    private ThirdPersonCamera controller;

    private const float SPEED = 250f; 

    [SetUp]
    public void SetUp()
    {
        // create player and camera mock
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Player_Mock");
        mock = GameObject.Instantiate(prefab).transform;
        cameraTransform = mock.Find("TPV");

        // Third-Person-Controller
        controller = new ThirdPersonCamera(mock, 0f);

        //Set SPEED const
        System.Reflection.FieldInfo info = controller.GetType().GetField("SPEED", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        info.SetValue(controller, SPEED);
    }

    [Test]
    public void PlayerCameraIsExistent_Test()
    {
        PlayerCamera pc = mock.GetComponent<PlayerCamera>();
        
        Assert.IsNotNull(pc);
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(0.75639f)]
    [TestCase(1f)]
    [TestCase(0.5f)]
    public void MouseInputUPRotatesCameraDown_Test(float value)
    {
        // setup
        float expected = value * 1f * SPEED;

        // since the desired rotation is clamped
        expected = Mathf.Clamp(expected, -60, 60);
        // because Unity returns rotations from 0 to 360 degrees
        expected = 360f - expected;

        // perform
        controller.Rotate(0f, value, 1f);
        
        // get
        Vector3 after = cameraTransform.rotation.eulerAngles;

        // assert
        Assert.AreEqual(expected, (float)Math.Round(after.x, 3));
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-0.75639f)]
    [TestCase(-1f)]
    [TestCase(-0.5f)]
    public void MouseInputDOWNRotatesCameraUp_Test(float value)
    {
        // setup
        float expected = value * 1f * SPEED;

        // since the desired rotation is clamped
        expected = Mathf.Clamp(expected, -60, 60);
        // because Unity returns rotations from 0 to 360 degrees
        expected = -expected;

        // perform
        controller.Rotate(0f, value, 1f);
        
        // get
        Vector3 after = cameraTransform.rotation.eulerAngles;

        // assert
        Assert.AreEqual(expected, (float) Math.Round(after.x, 3));
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(0.75639f)]
    [TestCase(1f)]
    [TestCase(0.5f)]
    public void MouseInputRIGHTRotatesPlayerRight_Test(float value)
    {
        // setup
        float expected = value * 1f * SPEED;

        // perform
        controller.Rotate(value, 0f, 1f);
        
        // get
        Vector3 after = mock.transform.rotation.eulerAngles;

        // assert
        Assert.AreEqual(expected, after.y);
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-0.75639f)]
    [TestCase(-1f)]
    [TestCase(-0.5f)]
    public void MouseInputLEFTRotatesPlayerLeft_Test(float value)
    {
        // setup
        float expected = value * 1f * SPEED;
        // since Unity returns rotations form 0 to 360 degrees
        expected = 360f + expected;

        // perform
        controller.Rotate(value, 0f, 1f);
        
        // get
        Vector3 after = mock.transform.rotation.eulerAngles;

        // assert
        Assert.AreEqual(expected, after.y);
    }

    [Test]
    [TestCase(0f)]
    [TestCase(10f)]
    [TestCase(-130f)]
    [TestCase(1f)]
    [TestCase(-0.0010f)]
    public void ReturnsCorrectYRotation_Test(float value)
    {
        // perform
        controller.Rotate(value, 0f, 1f);
        
        // get
        float after = controller.GetYRotation();

        // assert
        Assert.AreEqual(value * SPEED, after);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(mock.gameObject);
        controller = null;
    }
}
