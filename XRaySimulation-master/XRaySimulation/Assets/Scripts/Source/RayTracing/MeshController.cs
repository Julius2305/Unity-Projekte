using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshController
{
    public VertexData[] VerticeData;
    public List<int> RelevantVertices;

    private MeshContainer meshContainer;

    public float AverageDose { get { return CalculateAverageDose(); } private set { } }

    public MeshController(MeshContainer container)
    {
        this.meshContainer = container;

        this.VerticeData = SetVerticeData();
        this.RelevantVertices = new List<int>();

        LinkVerticesWithSamePosition();
    }

    /// <summary>
    /// Initializes the VerticeData array.
    /// </summary>
    private VertexData[] SetVerticeData()
    {
        Vector3[] vertices = meshContainer.GetVertices();
        VertexData[] data = new VertexData[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            data[i] = new VertexData(vertices[i], 0f);
        }

        return data;
    }

    /// <summary>
    /// Adds all vertices with the same position into the array
    /// of corresponding vertices. Including itself.
    /// </summary>
    private void LinkVerticesWithSamePosition()
    {
        for (int i = 0; i < VerticeData.Length; i++)
        {
            List<int> verticeIndices = new List<int>();

            for (int j = 0; j < VerticeData.Length; j++)
            {
                if (VerticeData[i].Position == VerticeData[j].Position)
                    verticeIndices.Add(j);
            }

            VerticeData[i].VerticeWithSamePos = verticeIndices.ToArray();
        }
    }

    /// <summary>
    /// Should be called on each transform position/rotation update.
    /// Updates the normals, positions and the isDirty flag.
    /// </summary>
    public void UpdateVertices(Vector3 raySource)
    {
        RelevantVertices.Clear();

        Vector3[] normals = meshContainer.GetNormals();
        Vector3[] vertices = meshContainer.GetVertices();

        for (int i = 0; i < normals.Length; i++)
        {
            VerticeData[i].Position = vertices[i];
            VerticeData[i].isDirty = false;

            if (IsNormalRelevant(normals[i], VerticeData[i].Position, raySource))
                RelevantVertices.Add(i);
        }
    }

    /// <summary>
    /// Returns true if the given normal points towards the raySource parameter.
    /// </summary>
    private bool IsNormalRelevant(Vector3 normal, Vector3 origin, Vector3 raySource)
    {
        // calculates the angle between the normals and the ray-source
        float angle = Vector3.Dot(normal, raySource - origin);

#if UNITY_EDITOR
        // draw normals of the vertices
        //Debug.DrawRay(origin, normal, Color.red, 0.1f);
#endif

        if (angle >= 0)
            return true;

        return false;
    }

    /// <summary>
    /// Returns the positions of all relevant vertices.
    /// </summary>
    public Vector3[] GetRelevantVerticePositions()
    {
        int num = RelevantVertices.Count;
        Vector3[] positions = new Vector3[num];

        for (int i = 0; i < num; i++)
        {
            positions[i] = GetRelevantVertexPosition(i);
        }

        return positions;
    }

    /// <summary>
    /// Returns the position of the relevant vertex with given index.
    /// </summary>
    private Vector3 GetRelevantVertexPosition(int index)
    {
        return VerticeData[RelevantVertices[index]].Position;
    }

    /// <summary>
    /// Sorts out all vertices form the relevantVertices list which 
    /// aren't hit directly by a ray from the IRayTracer.
    /// </summary>
    public void SortOutUnhittedVertices(IRayTracer tracer)
    {
        for (int i = RelevantVertices.Count - 1; i >= 0; i--)
        {
            if (!tracer.CreateRay(GetRelevantVertexPosition(i)))
                RelevantVertices.Remove(RelevantVertices[i]);
        }
    }

    /// <summary>
    /// Stores the doses into the mesh.
    /// Doses array should be as long as there are relevant vertices!
    /// </summary>
    public void StoreDoses(float[] doses)
    {
        for (int i = 0; i < doses.Length; i++)
        {
            StoreDose(i, doses[i]);
        }
    }

    /// <summary>
    /// Stores the dose of relevant vertex with given index.
    /// Additionally stores the dose foreach vertice which is located
    /// at the same position as the relevant vertex.
    /// </summary>
    private void StoreDose(int index, float dose)
    {
        int vertexIndex = RelevantVertices[index];
        VertexData currentData = VerticeData[vertexIndex];
        int[] correspondingVertices = currentData.VerticeWithSamePos;

        // store dose for all corresponding vertices
        // including the current one
        for (int i = 0; i < correspondingVertices.Length; i++)
        {
            StoreDoseForCorrespondingVertices(correspondingVertices[i], dose);
        }
    }

    /// <summary>
    /// Stores the dose for vertex with given index.
    /// If this vertex hasn't got already the new dose.
    /// </summary>
    private void StoreDoseForCorrespondingVertices(int index, float dose)
    {
        if (!VerticeData[index].isDirty)
        {
            VerticeData[index].Dose += dose;
            VerticeData[index].isDirty = true;
        }
    }

    /// <summary>
    /// Returns the average dose of all mesh vertices.
    /// </summary>
    private float CalculateAverageDose()
    {
        float sum = 0f;
        for (int i = 0; i < VerticeData.Length; i++)
        {
            sum += VerticeData[i].Dose;
        }

        return sum / (float)VerticeData.Length;
    }
}

