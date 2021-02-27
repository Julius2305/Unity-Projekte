using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FurnitureInfoTest
{
    [Test]
    public void EmptyConstructorName_Test()
    {
        FurnitureInfo info = new FurnitureInfo();

        Assert.IsNotNull(info.Name);
    }

    [Test]
    public void EmptyConstructorDescription_Test()
    {
        FurnitureInfo info = new FurnitureInfo();

        Assert.IsNotNull(info.Description);
    }

    [Test]
    public void EmptyConstructorKeyCodes_Test()
    {
        FurnitureInfo info = new FurnitureInfo();

        Assert.IsNotNull(info.KeyCodes);
    }

    [Test]
    [TestCase("", "You are able to move it around!", new KeyCode[] { KeyCode.F })]
    [TestCase("Table", "You are able to move it around!", new KeyCode[] {KeyCode.F})]
    public void ConstructorName_Test(string expected, string description, KeyCode[] keys)
    {
        FurnitureInfo info = new FurnitureInfo(expected, description, keys);

        Assert.AreEqual(expected, info.Name);
    }

    [Test]
    [TestCase("Table", "", new KeyCode[] { KeyCode.F })]
    [TestCase("Table", "You are able to move it around!", new KeyCode[] { KeyCode.F })]
    public void ConstructorDescription_Test(string name, string expected, KeyCode[] keys)
    {
        FurnitureInfo info = new FurnitureInfo(name, expected, keys);

        Assert.AreEqual(expected, info.Description);
    }

    [Test]
    [TestCase("Table", "You are able to move it around!", new KeyCode[] { })]
    [TestCase("Table", "You are able to move it around!", new KeyCode[] { KeyCode.F })]
    [TestCase("Table", "You are able to move it around!", new KeyCode[] { KeyCode.F, KeyCode.A })]
    public void ConstructorKeyCodes_Test(string name, string description, KeyCode[] expected)
    {
        FurnitureInfo info = new FurnitureInfo(name, description, expected);

        Assert.AreEqual(expected, info.KeyCodes);
    }

    /// <summary>
    /// To Check if the FurnitureInfo can be set in the inspector.
    /// </summary>
    [Test]
    public void IsSerializeable_Test()
    {
        Assert.IsTrue(typeof(FurnitureInfo).IsSerializable);
    }
}