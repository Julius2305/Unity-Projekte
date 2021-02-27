using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class RayTracerTest
{
    private RayTracer tracer;
    private Transform source;

    private readonly Vector3 MOCK_PLACE = Vector3.right * 3f;

    [SetUp]
    public void Setup()
    {
        // create a mock gameobject
        GameObject gameObj = new GameObject("Source");
        source = GameObject.Instantiate(gameObj).transform;

        // create raytracer instance for the mock
        tracer = new RayTracer(source.transform);
    }

    [Test]
    public void RayTracerIsNotNull_Test()
    {
        Assert.IsNotNull(tracer);
    }

    [Test]
    public void GetDistances_Test()
    {
        // setup positions
        Vector3[] positions = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(0f, -1f, 0f),
            new Vector3(0f, 0f, -1f),
            new Vector3(0f, 2f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 0f, 100f)
        };

        // setup expected distances
        float[] expected = new float[]
        {
            0f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1.732f, 100f
        };

        // perform
        float[] distances = tracer.GetDistances(positions);

        // assert
        Assert.AreEqual(expected, distances);
    }

    [Test]
    public void CreateRayReturnsFalseWhenNoHitDetected_Test()
    {
        // perform
        bool hitted = tracer.CreateRay(MOCK_PLACE);

        // assert
        Assert.IsFalse(hitted);
    }

    [Test]
    [TestCase("Mock_simple")]
    [TestCase("Mock_complex")]
    public void CreateRayReturnsTrueIfDocHitDetected_Test(string mockName)
    {
        // setup
        GameObject mock = CreateMock(mockName, "Doc");

        // perform
        bool hitted = tracer.CreateRay(MOCK_PLACE);

        // assert
        Assert.IsTrue(hitted);

        // clear
        GameObject.DestroyImmediate(mock);
    }

    [Test]
    [TestCase("Mock_simple")]
    [TestCase("Mock_complex")]
    public void CreateRayReturnsFalseIfOtherHitDetected_Test(string mockName)
    {
        // setup
        GameObject mock = CreateMock(mockName, "Other");

        // perform
        bool hitted = tracer.CreateRay(MOCK_PLACE);

        // assert
        Assert.IsFalse(hitted);

        // clear
        GameObject.DestroyImmediate(mock);
    }

    [TearDown]
    public void Teardown()
    {
        // remove references for clean next test
        GameObject.DestroyImmediate(source.gameObject);
        tracer = null;
    }

    /// <summary>
    /// Creates a mock gameobject from a prefab named like "prefabName".
    /// Sets the tag and name of the gameobject to the given "tag".
    /// Returns the mock.
    /// </summary>
    private GameObject CreateMock(string prefabName, string tag)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
        GameObject mock = GameObject.Instantiate(prefab, MOCK_PLACE, Quaternion.identity);
        mock.name = tag;
        mock.tag = tag;

        return mock;
    }
}