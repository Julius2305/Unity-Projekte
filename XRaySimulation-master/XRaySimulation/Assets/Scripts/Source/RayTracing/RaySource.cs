using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RaySource : MonoBehaviour
{
    [HideInInspector] public static RaySource Instance;
    [HideInInspector] public RayTracer RayTracer;

    [SerializeField] public float BaseEnergy = 120;

    private void Start()
    {
        Instance = this;

        this.RayTracer = new RayTracer(this.transform);
    }
}