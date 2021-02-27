using System;
using UnityEngine;
using NUnit.Framework;

public class FirstPersonCameraTest
{
    public Transform Mock;
    public Transform CameraTransform;
    public FirstPersonCamera Controller;

    private const float SPEED = 250f;

    [SetUp]
    public void SetUp()
    {
        // create player and camera mock
        Mock = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player_Mock")).transform;
        CameraTransform = Mock.Find("FPV");

        // First-Person-Camera
        Controller = new FirstPersonCamera(Mock, 0f);

        // set SPEED
        System.Reflection.FieldInfo info = Controller.GetType().GetField("SPEED", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        info.SetValue(Controller, SPEED);
    }

    [Test]
    public void PlayerCameraIsExistent_Test()
    {
        PlayerCamera pc = Mock.GetComponent<PlayerCamera>();

        Assert.IsNotNull(pc);
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(0.75639f)]
    [TestCase(1f)]
    [TestCase(0.5f)]
    public void MouseInputUPRotatesCameraUp_Test(float value)
    {
        // settup
        Vector3 before = CameraTransform.rotation.eulerAngles;
        
        // perform
        Controller.Rotate(0f, value, 1f);
        
        // get
        Vector3 after = CameraTransform.rotation.eulerAngles;

        // Move to test range.
        // Since the Unity rotation is presented from 0 to 360 degrees.
        // Which means a rotation is always greater than 0, or 0.
        float afterX = after.x - 360f;

        // assert
        Assert.Less(afterX, before.x);
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-0.75639f)]
    [TestCase(-1f)]
    [TestCase(-0.5f)]
    public void MouseInputDOWNRotatesCameraDown_Test(float value)
    {
        // setup
        Vector3 before = CameraTransform.rotation.eulerAngles;
        
        // perform
        Controller.Rotate(0f, value, 1f);
        
        // get
        Vector3 after = CameraTransform.rotation.eulerAngles;

        // assert
        Assert.Greater(after.x, before.x);
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(0.75639f)]
    [TestCase(1f)]
    [TestCase(0.5f)]
    public void MouseInputRIGHTRotatesPlayerRight_Test(float value)
    {
        // setup
        Vector3 before = Mock.rotation.eulerAngles;
        
        // perform
        Controller.Rotate(value, 0f, 1f);
        
        // get
        Vector3 after = Mock.rotation.eulerAngles;

        // assert
        Assert.Greater(after.y, before.y);
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-0.75639f)]
    [TestCase(-1f)]
    [TestCase(-0.5f)]
    public void MouseInputLEFTRotatesPlayerLeft_Test(float value)
    {
        // setup
        Vector3 before = Mock.rotation.eulerAngles;
        
        // perform
        Controller.Rotate(value, 0f, 1f);
        
        // get
        Vector3 after = Mock.rotation.eulerAngles;

        // Move to test range.
        // Since the Unity rotation is presented from 0 to 360 degrees.
        // Which means a rotation is always greater than 0, or 0.
        float afterY = after.y - 360f;

        // assert
        Assert.Less(afterY, before.y);
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-1f)]
    [TestCase(0f)]
    public void ClampUpRotationForm60To60_Test(float value)
    {
        // setup
        Controller.SetXRotation(60f);
        Vector3 before = CameraTransform.localEulerAngles;
        
        // perform
        Controller.Rotate(0f, value, 1f);
        
        // get
        Vector3 after = CameraTransform.localEulerAngles;

        after.x = (float)Math.Round(after.x, 10);

        // assert
        Assert.AreEqual(after.x, before.x);
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-1f)]
    [TestCase(0f)]
    public void ClampUpRotationFormLess60To60_Test(float value)
    {
        // setup
        Controller.SetXRotation(59f);
        Vector3 before = CameraTransform.localEulerAngles;
        
        // perform
        Controller.Rotate(0f, value, 1f);
        
        // get
        Vector3 after = CameraTransform.localEulerAngles;

        after.x = (float) Math.Round(after.x, 3);

        // assert
        Assert.GreaterOrEqual(after.x, 59f);
        Assert.LessOrEqual(after.x, 60f);
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(1f)]
    [TestCase(0f)]
    public void ClampDownRotationFormMinus60ToMinus60_Test(float value)
    {
        // setup
        Controller.SetXRotation(-60f);
        Vector3 before = CameraTransform.localEulerAngles;
        
        // perform
        Controller.Rotate(0f, value, 1f);

        // get
        Vector3 after = CameraTransform.localEulerAngles;

        after.x = (float)Math.Round(after.x, 10);

        // assert
        Assert.AreEqual(after.x, before.x);
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(1f)]
    [TestCase(0f)]
    public void ClampDownRotationFormLessMinus60ToMinus60_Test(float value)
    {
        // setup
        Controller.SetXRotation(-59f);
        Vector3 before = CameraTransform.localEulerAngles;
        
        // perform
        Controller.Rotate(0f, value, 1f);
        
        // get
        Vector3 after = CameraTransform.localEulerAngles;

        // Move to test range.
        // Since the Unity rotation is presented from 0 to 360 degrees.
        // Which means a rotation is always greater than 0, or 0.
        float afterX = after.x - 360f;

        // assert
        Assert.LessOrEqual(afterX, -59f);
        Assert.GreaterOrEqual(afterX, -60f);
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
        Controller.Rotate(value, 0f, 1f);
        
        // get
        float after = Controller.GetYRotation();

        // assert
        // value * 250, since the rotationspeed is set to 250
        Assert.AreEqual(value * SPEED, after);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(Mock.gameObject);
    }
}