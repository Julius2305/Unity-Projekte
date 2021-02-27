using System;
using UnityEngine;

public struct VertexData
{
    public Vector3 Position;
    public float Dose;
    public bool isDirty;
    public int[] VerticeWithSamePos;

    public VertexData(Vector3 pos, float dose)
    {
        Position = pos;
        Dose = dose;
        isDirty = false;
        VerticeWithSamePos = new int[0];
    }
}
