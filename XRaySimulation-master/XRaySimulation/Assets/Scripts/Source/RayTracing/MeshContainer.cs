using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshContainer
{
    private Transform meshTransform;
    private MeshFilter meshFilter;

    public MeshContainer(Transform transform)
    {
        this.meshTransform = transform;
        this.meshFilter = meshTransform.GetComponent<MeshFilter>();
    }

    /// <summary>
    /// Returns the world space positions of the mesh vertices.
    /// </summary>
    public Vector3[] GetVertices()
    {
        Vector3[] vertices = meshFilter.sharedMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            // rotate each vertex around pivot poitn of the transform
            Vector3 rotatedVec = meshTransform.rotation * vertices[i];
            rotatedVec.x = meshTransform.localScale.x * rotatedVec.x;
            rotatedVec.y = meshTransform.localScale.y * rotatedVec.y;
            rotatedVec.z = meshTransform.localScale.z * rotatedVec.z;

            vertices[i] = meshTransform.position + rotatedVec;
        }

        return vertices;
    }

    /// <summary>
    /// Returns the world space rotated mesh normals.
    /// </summary>
    public Vector3[] GetNormals()
    {
        Vector3[] normals = meshFilter.sharedMesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            // rotate each normals acording to the transform rotation
            normals[i] = meshTransform.rotation * normals[i];
        }

        return normals;
    }

    /// <summary>
    /// Sets the colors of the merh vertices.
    /// newColors should be as long as there are vertices in the mesh.
    /// </summary>
    public void ApplyColors(Color32[] newColors)
    {
        meshFilter.sharedMesh.colors32 = newColors;
    }

    /// <summary>
    /// Applies a given color to each vertice of the mesh.
    /// </summary>
    public void ApplyColor(Color newColor)
    {
        Color32[] newColors = new Color32[meshFilter.sharedMesh.colors32.Length];

        for (int i = 0; i < newColors.Length; i++)
        {
            newColors[i] = (Color32)newColor;
        }

        ApplyColors(newColors);
    }
}