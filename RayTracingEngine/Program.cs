﻿using System.Numerics;
using RayTracingEngine.Primitives;
using RayTracingEngine.Rendering;

namespace RayTracingEngine;

public class Program
{
    public static void Main(string[] args)
    {
        // Create a ray from the origin to the point (1, 1, 1)
        var ray = new Ray(new Vector3(0, 5, 1.0f), new Vector3(0, -0.5f, -0.1f));

        var cube = new Cube(1, Vector3.Zero);
        
        // Check if the ray intersects with the sphere
        var hitInfo = cube.Hit(ray);
        
        if (hitInfo != null)
        {
            Console.WriteLine($"Hits: {hitInfo.Hits.Length}");
        }
        else
        {
            Console.WriteLine("No hit");
        }
    }
}