using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using System.Linq;
using NSubstitute;

public class RayRecieverWithoutSourceTest
{
    private GameObject testObj;
    private GameObject gui;
    private MeshController controller;
    private RayReciever reciever;

    [SetUp]
    public void SetUp()
    {
        // Player
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Player_Mock");
        testObj = GameObject.Instantiate(prefab);
        testObj.name = "Player";

        // RayReciever
        reciever = testObj.GetComponent<RayReciever>();

        // Gui
        prefab = Resources.Load<GameObject>("Prefabs/UI/GUI");
        gui = GameObject.Instantiate(prefab);
        gui.name = "GUI";
    }

    /// <summary>
    /// Enables to get the value of a private field with given name.
    /// Returns the value of the field.
    /// </summary>
    private T GetPrivateField<T, U>(U obj, string fieldName)
    {
        System.Reflection.FieldInfo info = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)info.GetValue(obj);
    }

    /// <summary>
    /// Enables to set the value of a private field with given name.
    /// </summary>
    private void SetPrivateField<T, U>(T obj, string fieldName, U value)
    {
        System.Reflection.FieldInfo info = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
        info.SetValue(obj, value);
    }

    [Test]
    public void PlayerIsNotNull_Test()
    {
        Assert.IsNotNull(testObj);
    }

    [Test]
    public void GuiIsNotNull_Test()
    {
        Assert.IsNotNull(gui);
    }

    [UnityTest]
    public IEnumerator AVGIsZero_Test()
    {
        // setup
        float expected = 0f;

        // till start is called
        yield return new WaitForEndOfFrame();
        // till update is called
        yield return new WaitForEndOfFrame();

        // perform
        yield return new WaitForSeconds(1f);

        // get
        controller = GetPrivateField<MeshController, RayReciever>(reciever, "controller");
        float[] doses = controller.VerticeData.Select(x => x.Dose).ToArray();
        float actual = DoseCalculator.GetAVGDose(doses);

        // assert
        Assert.AreEqual(expected, actual);
    }

    [UnityTest]
    public IEnumerator AVGIsZeroWhenSourceShouldBeActive_Test()
    {
        // setup
        float expected = 0f;

        IInputWrapper input = Substitute.For<IInputWrapper>();
        input.GetKeyDown(KeyCode.Space).Returns(true);
        SetPrivateField(UnityInput.Instance, "input", input);

        // till start is called
        yield return new WaitForEndOfFrame();
        // till update is called
        yield return new WaitForEndOfFrame();

        // perform
        input.GetKeyDown(KeyCode.Space).Returns(false);

        yield return new WaitForSeconds(1f);

        // get
        controller = GetPrivateField<MeshController, RayReciever>(reciever, "controller");
        float[] doses = controller.VerticeData.Select(x => x.Dose).ToArray();
        float actual = DoseCalculator.GetAVGDose(doses);

        // assert
        Assert.AreEqual(expected, actual);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(testObj);
        GameObject.Destroy(gui);
    }
}
