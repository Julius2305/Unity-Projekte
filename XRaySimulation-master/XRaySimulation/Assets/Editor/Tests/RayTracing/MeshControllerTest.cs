using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Linq;
using NSubstitute;
using System.Linq.Expressions;

public class MeshControllerTest
{
    private GameObject docMeshObj;
    private MeshController controller;

    // Can't use normal [SetUp], because one test use different mocks.
    // The default SetUp-Method can't have parameters.
    public void SetUp(string mockName = "Mock_simple")
    {
        GameObject docMeshPref = Resources.Load<GameObject>("Prefabs/" + mockName);
        docMeshObj = GameObject.Instantiate(docMeshPref);

        MeshContainer container = new MeshContainer(docMeshPref.transform);
        controller = new MeshController(container);
    }

    /// <summary>
    /// Enables to get the value of a private field with given name.
    /// Returns the value of the field.
    /// </summary>
    private T GetPrivateField<T>(string fieldName)
    {
        System.Reflection.FieldInfo info = controller.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T) info.GetValue(controller);
    }

    [Test]
    public void MeshContainerIsNotNull_Test()
    {
        SetUp();

        MeshContainer mc = GetPrivateField<MeshContainer>("meshContainer");

        Assert.IsNotNull(mc);
    }

    [Test]
    public void VerticeDataIsNotNull_Test()
    {
        SetUp();

        Assert.IsNotNull(controller.VerticeData);
    }

    [Test]
    public void RelevantVerticesIsNotNull_Test()
    {
        SetUp();

        Assert.IsNotNull(controller.RelevantVertices);
    }

    [Test]
    public void RelevantVerticesIsNotNullAfterUpdate_Test()
    {
        SetUp();

        // perform
        controller.UpdateVertices(new Vector3());

        // assert
        Assert.IsNotNull(controller.RelevantVertices);
    }

    [Test]
    [TestCase("Mock_simple", 4)]
    [TestCase("Mock_complex", 16)]
    public void RelevantVerticesHasCorrectNumberAfterUpdateWithSourceOnXAxis_Test(string mockName, int expected)
    {
        SetUp(mockName);

        // perform
        controller.UpdateVertices(new Vector3(5f, 0f, 0f));

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    [TestCase("Mock_simple", 4)]
    [TestCase("Mock_complex", 9)]
    public void RelevantVerticesHasCorrectNumberAfterUpdateWithSourceOnYAxis_Test(string mockName, int expected)
    {
        SetUp(mockName);

        // perform
        controller.UpdateVertices(new Vector3(0f, 5f, 0f));

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    [TestCase("Mock_simple", 4)]
    [TestCase("Mock_complex", 12)]
    public void RelevantVerticesHasCorrectNumberAfterUpdateWithSourceOnZAxis_Test(string mockName, int expected)
    {
        SetUp(mockName);

        // perform
        controller.UpdateVertices(new Vector3(0f, 0f, 5f));

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    [TestCase("Mock_simple", 4)]
    [TestCase("Mock_complex", 16)]
    public void RelevantVerticesHasCorrectNumberAfterUpdateWithSourceOnNegativeXAxis_Test(string mockName, int expected)
    {
        SetUp(mockName);

        // perform
        controller.UpdateVertices(new Vector3(-5f, 0f, 0f));

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    [TestCase("Mock_simple", 4)]
    [TestCase("Mock_complex", 9)]
    public void RelevantVerticesHasCorrectNumberAfterUpdateWithSourceOnnegativeYAxis_Test(string mockName, int expected)
    {
        SetUp(mockName);

        // perform
        controller.UpdateVertices(new Vector3(0f, -5f, 0f));

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    [TestCase("Mock_simple", 4)]
    [TestCase("Mock_complex", 16)]
    public void RelevantVerticesHasCorrectNumberAfterUpdateWithSourceOnNegativeZAxis_Test(string mockName, int expected)
    {
        SetUp(mockName);

        // perform
        controller.UpdateVertices(new Vector3(0f, 0f, -5f));

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    public void GetRelevantVerticePositionsReturnsCorrectNumberOfPositions_Test()
    {
        // setup
        SetUp();
        controller.RelevantVertices = new List<int> { 0, 1, 2 };
        controller.VerticeData = new VertexData[]
        {
            new VertexData(new Vector3(1f, 0f, 0f), 0f),
            new VertexData(new Vector3(0f, 1f, 0f), 0f),
            new VertexData(new Vector3(0f, 0f, 1f), 0f),
        };

        // perform
        Vector3[] positions = controller.GetRelevantVerticePositions();

        // assert
        Assert.AreEqual(3, positions.Length);
    }

    [Test]
    public void GetRelevantVerticePositionsReturnsCorrectPositions_Test()
    {
        // setup
        SetUp();

        VertexData[] expected = new VertexData[]
        {
            new VertexData(new Vector3(1f, 0f, 0f), 0f),
            new VertexData(new Vector3(0f, 1f, 0f), 0f),
            new VertexData(new Vector3(0f, 0f, 1f), 0f),
        };

        controller.RelevantVertices = new List<int> { 0, 1, 2 };
        controller.VerticeData = expected;

        // perform
        Vector3[] positions = controller.GetRelevantVerticePositions();

        // assert
        Assert.AreEqual(expected[0].Position, positions[0]);
        Assert.AreEqual(expected[1].Position, positions[1]);
        Assert.AreEqual(expected[2].Position, positions[2]);
    }

    [Test]
    public void SortOutUnhittedVerticesForAllVerticesHitted_Test()
    {
        // setup
        SetUp();

        controller.RelevantVertices = new List<int>() { 0, 1, 2 };
        int expected = 3;

        IRayTracer rt = Substitute.For<IRayTracer>();
        rt.CreateRay(controller.GetRelevantVerticePositions()[0]).Returns(true);
        rt.CreateRay(controller.GetRelevantVerticePositions()[1]).Returns(true);
        rt.CreateRay(controller.GetRelevantVerticePositions()[2]).Returns(true);

        // perform
        controller.SortOutUnhittedVertices(rt);

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    public void SortOutUnhittedVerticesForZeroVerticesHitted_Test()
    {
        // setup
        SetUp();

        controller.RelevantVertices = new List<int>() { 0, 1, 2 };
        int expected = 0;

        IRayTracer rt = Substitute.For<IRayTracer>();
        rt.CreateRay(controller.GetRelevantVerticePositions()[0]).Returns(false);
        rt.CreateRay(controller.GetRelevantVerticePositions()[1]).Returns(false);
        rt.CreateRay(controller.GetRelevantVerticePositions()[2]).Returns(false);

        // perform
        controller.SortOutUnhittedVertices(rt);

        // assert
        Assert.AreEqual(expected, controller.RelevantVertices.Count);
    }

    [Test]
    public void StoreDosesForNoCorrespondingVertices_Test()
    {
        // setup
        SetUp();

        controller.VerticeData = new VertexData[3]
        {
            new VertexData(new Vector3(10f, 10f, 10f), 0f ),
            new VertexData(new Vector3(8f, 1f, -1f), 2f ),
            new VertexData(new Vector3(2f, 1f, 1.5f), 10f )
        };

        controller.RelevantVertices = new List<int>()
        {
            0, 1, 2
        };

        // disable linking of vertices with same position
        // linking will result in error or needs a more complicated setup
        System.Reflection.MethodInfo info = controller.GetType().GetMethod("LinkVerticesWithSamePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        info.Invoke(controller, null);

        float[] doses = new float[] { 3f, 2f, 1.256987f };
        float[] expected = new float[] { 3f, 4f, 11.256987f };

        // perform
        controller.StoreDoses(doses);

        // get
        float[] actual = controller.VerticeData.Select(x => x.Dose).ToArray();

        // assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void StoreDosesForCorrespondingVertices_Test()
    {
        // setup
        SetUp();

        controller.VerticeData = new VertexData[4]
        {
            new VertexData(new Vector3(1f, 10f, 10f), 0f ),
            new VertexData(new Vector3(1f, 10f, 10f), 0f ),
            new VertexData(new Vector3(8f, 1f, 1f), 2f ),
            new VertexData(new Vector3(8f, 1f, 1f), 2f )
        };

        controller.RelevantVertices = new List<int>()
        {
            0, 2
        };

        // disable linking of vertices with same position
        // linking will result in error or needs a more complicated setup
        System.Reflection.MethodInfo info = controller.GetType().GetMethod("LinkVerticesWithSamePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        info.Invoke(controller, null);

        float[] doses = new float[] { 3f, 2f };
        float[] expected = new float[] { 3f, 3f, 4f, 4f};

        // perform
        controller.StoreDoses(doses);

        // get
        float[] actual = controller.VerticeData.Select(x => x.Dose).ToArray();

        // assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void AverageDose_Test()
    {
        // setup
        SetUp();

        controller.VerticeData = new VertexData[3]
        {
            new VertexData(new Vector3(10f, 10f, 10f), 0f ),
            new VertexData(new Vector3(8f, 1f, -1f), 2f ),
            new VertexData(new Vector3(2f, 1f, 1.5f), 10f )
        };

        float avg = (0f + 2f + 10f) / 3f;

        // perform
        float actual = controller.AverageDose;

        // assert
        Assert.AreEqual(avg, actual);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(docMeshObj);
        controller = null;
    }
}