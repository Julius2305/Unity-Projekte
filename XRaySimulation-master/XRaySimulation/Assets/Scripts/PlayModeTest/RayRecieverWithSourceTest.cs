using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using System.Linq;
using NSubstitute;

public class RayRecieverWithSourceTest
{
    private GameObject testObj;
    private GameObject source;
    private GameObject gui;
    private MeshController controller;
    private RayReciever reciever;

    [SetUp]
    public void SetUp()
    {
        //RaySource
        GameObject prefab = Resources.Load<GameObject>("Prefabs/XRaySource");
        source = GameObject.Instantiate(prefab);
        source.name = "XRaySource";

        // Player / RayReciever
        prefab = Resources.Load<GameObject>("Prefabs/Player_Mock");
        testObj = GameObject.Instantiate(prefab, new Vector3(3f, 0f, 0f), Quaternion.identity);
        testObj.name = "Player";

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
        System.Reflection.FieldInfo info = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
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
    public IEnumerator AVGIsZeroWhenSourceIsNotActive_Test()
    {
        // setup
        float expected = 0f;

        // wait till start is called
        yield return new WaitForEndOfFrame();
        // wait till first update is called
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
    public IEnumerator AVGIsGreaterZeroWhenSourceIsActive_Test()
    {
        // setup
        float expected = 0f;

        SetPrivateField(reciever, "isActive", true);

        // wait till start is called
        yield return new WaitForEndOfFrame();
        // wait till first update is called
        yield return new WaitForEndOfFrame();

        // perform
        yield return new WaitForSeconds(1f);

        // get
        controller = GetPrivateField<MeshController, RayReciever>(reciever, "controller");
        float[] doses = controller.VerticeData.Select(x => x.Dose).ToArray();
        float actual = DoseCalculator.GetAVGDose(doses);

        // assert
        Assert.Greater(actual, expected);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(source);
        GameObject.Destroy(testObj);
        GameObject.Destroy(gui);
    }
}
