using System;
using UnityEngine;

public class RayTracer : IRayTracer
{
    private Transform source;

    public RayTracer(Transform transform)
    {
        this.source = transform;
    }

    /// <summary>
    /// Creates a unity Raycast to the given destination.
    /// The starting point is determined by the field "Transform source".
    /// Returns true if the ray hits an object with tag "Doc".
    /// </summary>
    public bool CreateRay(Vector3 destination)
    {
        return CreateRay(source.position, destination);
    }

    /// <summary>
    /// Creates a unity Raycast from the start point to the end point.
    /// Returns true if the ray hits an object with tag "Doc".
    /// </summary>
    private bool CreateRay(Vector3 start, Vector3 end)
    {
        bool hittedDoc = false;

        Vector3 direction = end - start;

        RaycastHit hit;
        Ray ray = new Ray(start, direction.normalized);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Doc")
            {
                hittedDoc = true;
            }
        }

#if UNITY_EDITOR
        DrawDebugRay(start, hit.point, hittedDoc);
#endif

        return hittedDoc;
    }

    /// <summary>
    /// Returns the distances between the source Transform and the given positions.
    /// </summary>
    public float[] GetDistances(Vector3[] positions)
    {
        int num = positions.Length;
        float[] distances = new float[num];

        for (int i = 0; i < num; i++)
        {
            distances[i] = GetDistance(positions[i]);
        }

        return distances;
    }

    /// <summary>
    /// Returns the distances between the source Transform and the given position.
    /// </summary>
    private float GetDistance(Vector3 position)
    {
        return (float)Math.Round(Vector3.Distance(source.position, position), 3);
    }

    /// <summary>
    /// Helper function.
    /// Draws a line in the editor scene view between start and end position.
    /// Hit determines the color of the line.
    /// True is green, false is red.
    /// Should only be called in UNITY_EDITOR mode.
    /// Increases the performance overhead!
    /// </summary>
    private void DrawDebugRay(Vector3 start, Vector3 end, bool hit)
    {
        if (hit)
            Debug.DrawLine(start, end, Color.green, 0.1f);
        else
            Debug.DrawLine(start, end, Color.red, 0.1f);
    }
}