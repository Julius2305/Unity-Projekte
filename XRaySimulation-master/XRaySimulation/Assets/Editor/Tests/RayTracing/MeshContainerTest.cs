using System;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Reflection;

public class MeshContainerTest
{
    private MeshContainer container;
    private GameObject meshObj;

    // Can't be [SetUp] because these Functions can't have a parameter.
    private void SetUp(string prefabName)
    {
        // create a mock gameobject with a mesh
        GameObject docMeshPref = Resources.Load<GameObject>(prefabName);
        meshObj = GameObject.Instantiate(docMeshPref);
        container = new MeshContainer(meshObj.transform);
    }

    [Test]
    [TestCase("Prefabs/Mock_simple")]
    [TestCase("Prefabs/Mock_complex")]
    public void MeshContainerExists_Test(string mockName)
    {
        SetUp(mockName);
        Assert.IsNotNull(container);
    }

    [Test]
    [TestCase("Prefabs/Mock_simple")]
    [TestCase("Prefabs/Mock_complex")]
    public void GameObjectHasMeshFilterComponent_Test(string mockName)
    {
        SetUp(mockName);
        Assert.IsNotNull(meshObj.GetComponent<MeshFilter>());
    }

    [Test]
    [TestCase("Prefabs/Mock_simple")]
    [TestCase("Prefabs/Mock_complex")]
    public void MeshTransformIsNotNull_Test(string mockName)
    {
        SetUp(mockName);

        // get
        FieldInfo info = container.GetType().GetField("meshTransform", BindingFlags.NonPublic | BindingFlags.Instance);
        Transform transform = (Transform) info.GetValue(container);

        // assert
        Assert.IsNotNull(transform);
    }

    [Test]
    [TestCase("Prefabs/Mock_simple")]
    [TestCase("Prefabs/Mock_complex")]
    public void GetVerticesReturnsNotNull_Test(string mockName)
    {
        SetUp(mockName);

        Assert.IsNotNull(container.GetVertices());
    }

    [Test]
    [TestCase("Prefabs/Mock_simple", 24)]
    [TestCase("Prefabs/Mock_complex", 54)]
    public void GetVerticesReturnsCorrectNumberOfVertices_Test(string mockName, int expected)
    {
        SetUp(mockName);

        // perform
        Vector3[] vertices = container.GetVertices();

        // assert
        Assert.AreEqual(expected, vertices.Length);
    }

    [Test]
    [TestCase("Prefabs/Mock_simple")]
    [TestCase("Prefabs/Mock_complex")]
    public void GetNormalsReturnsNotNull_Test(string mockName)
    {
        SetUp(mockName);

        Assert.IsNotNull(container.GetNormals());
    }

    [Test]
    [TestCase("Prefabs/Mock_simple", 24)]
    [TestCase("Prefabs/Mock_complex", 54)]
    public void GetNormalsReturnsCorrectNumberOfNormals_Test(string mockName, int expected)
    {
        SetUp(mockName);
        
        // perform
        Vector3[] normals = container.GetNormals();

        // assert
        Assert.AreEqual(expected, normals.Length);
    }

    private Vector3[] normals = new Vector3[]
    {
        new Vector3(0f, 1f, 0f),
        new Vector3(0f, 0f, -1f),
        new Vector3(0f, 0f, -1f),
        new Vector3(0f, 0f, 1f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(1f, 0f, 0f),
    };

    [Test]
    [TestCase("Prefabs/Mock_simple", 0)]
    [TestCase("Prefabs/Mock_simple", 4)]
    [TestCase("Prefabs/Mock_simple", 8)]
    [TestCase("Prefabs/Mock_simple", 12)]
    [TestCase("Prefabs/Mock_simple", 16)]
    [TestCase("Prefabs/Mock_simple", 20)]
    public void GetNormalsReturnsCorrectNormals_Test(string mockName, int index)
    {
        // setup
        SetUp(mockName);
        Vector3 expected = normals[index / 4];
        
        // perform
        Vector3 normal = container.GetNormals()[index];

        // assert
        Assert.AreEqual(expected.x, normal.x, delta: 0.001f);
        Assert.AreEqual(expected.y, normal.y, delta: 0.001f);
        Assert.AreEqual(expected.z, normal.z, delta: 0.001f);
    }

    private Vector3[] rotatedNormals = new Vector3[]
    {
        new Vector3(-0.0087f, 0.4999f, 0.8660f),
        new Vector3(-0.7261f, 0.5922f, -0.3492f),
        new Vector3(-0.7261f, 0.5922f, -0.3492f),
        new Vector3(0.7261f, -0.5922f, 0.3492f),
        new Vector3(-0.6875f, -0.6319f, 0.3578f),
        new Vector3(0.6875f, 0.6319f, -0.3578f),
    };

    [Test]
    [TestCase("Prefabs/Mock_simple", 0)]
    [TestCase("Prefabs/Mock_simple", 4)]
    [TestCase("Prefabs/Mock_simple", 8)]
    [TestCase("Prefabs/Mock_simple", 12)]
    [TestCase("Prefabs/Mock_simple", 16)]
    [TestCase("Prefabs/Mock_simple", 20)]
    public void GetNormalsReturnsCorrectNormalsAfterRotation_Test(string mockName, int index)
    {
        // setup
        SetUp(mockName);
        meshObj.transform.Rotate(new Vector3(60f, -1f, 45.7f));
        Vector3 expected = rotatedNormals[index / 4];

        // perform
        Vector3 normal = container.GetNormals()[index];

        // assert
        Assert.AreEqual(expected.x, normal.x, delta: 0.001f);
        Assert.AreEqual(expected.y, normal.y, delta: 0.001f);
        Assert.AreEqual(expected.z, normal.z, delta: 0.001f);
    }

    private Vector3[] vertices = new Vector3[]
    {
        new Vector3(0.5f, 0.5f, 0.5f),
        new Vector3(-0.5f, 0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),
    };

    [Test]
    [TestCase("Prefabs/Mock_simple", 0)]
    [TestCase("Prefabs/Mock_simple", 1)]
    [TestCase("Prefabs/Mock_simple", 2)]
    [TestCase("Prefabs/Mock_simple", 3)]
    [TestCase("Prefabs/Mock_simple", 4)]
    [TestCase("Prefabs/Mock_simple", 5)]
    [TestCase("Prefabs/Mock_simple", 6)]
    [TestCase("Prefabs/Mock_simple", 7)]
    public void GetVerticesRetrunsCorrectVertices_Test(string mockName, int index)
    {
        // setup
        SetUp(mockName);
        Vector3 expected = vertices[index];

        // perform
        Vector3 vertex = container.GetVertices()[index];

        // assert
        Assert.AreEqual(expected.x, vertex.x, delta: 0.001f);
        Assert.AreEqual(expected.y, vertex.y, delta: 0.001f);
        Assert.AreEqual(expected.z, vertex.z, delta: 0.001f);
    }

    
    [Test]
    [TestCase("Prefabs/Mock_simple", 0)]
    [TestCase("Prefabs/Mock_simple", 1)]
    [TestCase("Prefabs/Mock_simple", 2)]
    [TestCase("Prefabs/Mock_simple", 3)]
    [TestCase("Prefabs/Mock_simple", 4)]
    [TestCase("Prefabs/Mock_simple", 5)]
    [TestCase("Prefabs/Mock_simple", 6)]
    [TestCase("Prefabs/Mock_simple", 7)]
    public void GetVerticesRetrunsCorrectVerticesAfterTranslation_Test(string mockName, int index)
    {
        // Array of expected vertice positions after translation.
        Vector3[] translatedVertices = new Vector3[]
        {
            new Vector3(1.5f, 2.5f, 3.5f),
            new Vector3(0.5f, 2.5f, 3.5f),
            new Vector3(1.5f, 2.5f, 2.5f),
            new Vector3(0.5f, 2.5f, 2.5f),
            new Vector3(1.5f, 1.5f, 2.5f),
            new Vector3(0.5f, 1.5f, 2.5f),
            new Vector3(1.5f, 1.5f, 3.5f),
            new Vector3(0.5f, 1.5f, 3.5f),
        };

        // setup
        SetUp(mockName);
        Vector3 expected = translatedVertices[index];

        // perform
        meshObj.transform.position = new Vector3(1f, 2f, 3f);
        Vector3 vertex = container.GetVertices()[index];

        // assert
        Assert.AreEqual(expected.x, vertex.x, delta: 0.001f);
        Assert.AreEqual(expected.y, vertex.y, delta: 0.001f);
        Assert.AreEqual(expected.z, vertex.z, delta: 0.001f);
    }

    

    [Test]
    [TestCase("Prefabs/Mock_simple", 0)]
    [TestCase("Prefabs/Mock_simple", 1)]
    [TestCase("Prefabs/Mock_simple", 2)]
    [TestCase("Prefabs/Mock_simple", 3)]
    [TestCase("Prefabs/Mock_simple", 4)]
    [TestCase("Prefabs/Mock_simple", 5)]
    [TestCase("Prefabs/Mock_simple", 6)]
    [TestCase("Prefabs/Mock_simple", 7)]
    public void GetVerticesRetrunsCorrectVerticesAfterRotation_Test(string mockName, int index)
    {
        // Array of expected vertice positions after a rotation.
        Vector3[] rotatedVertices = new Vector3[]
        {
            new Vector3(0.7024f, 0.2697f, 0.4287f),
            new Vector3(0.0149f, -0.3621f, 0.7865f),
            new Vector3(-0.0237f, 0.8620f, 0.0794f),
            new Vector3(-0.7112f, 0.2301f, 0.4373f),
            new Vector3(-0.0149f, 0.3621f, -0.7865f),
            new Vector3(-0.7024f, -0.2697f, -0.4287f),
            new Vector3(0.7112f, -0.2301f, -0.4373f),
            new Vector3(0.0237f, -0.8620f, -0.0794f),
        };

        // setup
        SetUp(mockName);
        Vector3 expected = rotatedVertices[index];

        // perform
        meshObj.transform.Rotate(new Vector3(60f, -1f, 45.7f));
        Vector3 vertex = container.GetVertices()[index];

        // assert
        Assert.AreEqual(expected.x, vertex.x, delta: 0.001f);
        Assert.AreEqual(expected.y, vertex.y, delta: 0.001f);
        Assert.AreEqual(expected.z, vertex.z, delta: 0.001f);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(meshObj);
    }
}
