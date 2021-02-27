using System;
using UnityEngine;
using NUnit.Framework;

public class MoveControllerTest
{
    private Transform mock;
    private MoveController controller;

    [SetUp]
    public void SetUp()
    {
        // Mock gameobject
        mock = GameObject.Instantiate(new GameObject()).transform;
        mock.position = Vector3.zero;
        mock.rotation = Quaternion.identity;
        mock.gameObject.AddComponent<CharacterController>();
      
        // MoveController
        controller = new MoveController(mock);
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
    public void PlayerMovementIsExistent_Test()
    {
        // setup
        GameObject go = GameObject.Instantiate(new GameObject());
        PlayerMovement dm = go.AddComponent<PlayerMovement>();

        // assert
        Assert.IsNotNull(dm);
    }

    [Test]
    public void PlayerTransformIsNotNull_Test()
    {
        Transform transform = GetPrivateField<Transform>("playerTransform");

        Assert.IsNotNull(transform);
    }

    [Test]
    public void CharacterControllerIsNotNull_Test()
    {
        CharacterController characterController = GetPrivateField<CharacterController>("characterController");

        Assert.IsNotNull(characterController);
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(0.75639f)]
    [TestCase(1f)]
    [TestCase(0.5f)]
    public void MovesAlongPositiveZforInputUP_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(0f, value, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.Greater(after.z, before.z);
        Assert.AreEqual(before.x, after.x, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [Test]
    [TestCase(0f)]
    public void MovesNotForInputUPisZero_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(0f, value, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.AreEqual(after.z, before.z);
        Assert.AreEqual(before.x, after.x, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-0.75639f)]
    [TestCase(-1f)]
    [TestCase(-0.5f)]
    public void MovesAlongNegativeZforInputDOWN_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(0f, value, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.Less(after.z, before.z);
        Assert.AreEqual(before.x, after.x, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [Test]
    [TestCase(0)]
    public void MovesNotforInputDOWNisZero_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(0f, value, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.AreEqual(after.z, before.z);
        Assert.AreEqual(before.x, after.x, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [Test]
    [TestCase(0.1f)]
    [TestCase(0.75639f)]
    [TestCase(1f)]
    [TestCase(0.5f)]
    public void MovesAlongPositiveXforInputRIGHT_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(value, 0f, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.Greater(after.x, before.x);
        Assert.AreEqual(before.z, after.z, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [Test]
    [TestCase(0)]
    public void MovesNotforInputRIGHTisZero_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(value, 0f, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.AreEqual(after.x, before.x);
        Assert.AreEqual(before.z, after.z, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [Test]
    [TestCase(-0.1f)]
    [TestCase(-0.75639f)]
    [TestCase(-1f)]
    [TestCase(-0.5f)]
    public void MovesAlongNegativeXforInputLEFT_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(value, 0f, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.Less(after.x, before.x);
        Assert.AreEqual(before.z, after.z, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [Test]
    [TestCase(0)]
    public void MovesNotforInputLEFTisZero_Test(float value)
    {
        // setup
        Vector3 before = mock.position;

        // perform
        controller.Move(value, 0f, 1f);

        // get
        Vector3 after = mock.position;

        // assert
        Assert.AreEqual(after.x, before.x);
        Assert.AreEqual(before.z, after.z, 0.01f);
        Assert.AreEqual(before.y, after.y, 0.01f);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(mock.gameObject);
        controller = null;
    }
}