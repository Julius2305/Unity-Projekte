using System;
using System.Collections.Generic;
using UnityEngine;

public interface IRayTracer
{
    bool CreateRay(Vector3 destination);

    float[] GetDistances(Vector3[] positions);
}