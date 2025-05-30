/// <summary>
/// Constructs a Unity Mesh from parsed JSON mesh data (MeshData).
/// Vertices are welded based on spatial proximity to eliminate duplication,
/// normals and slope values are averaged per welded vertex.
/// The output mesh includes:
/// - Vertex positions (converted from Z-up to Y-up)
/// - Averaged normals
/// - Encoded slope value in vertex color (R channel)
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using Visualizer.Core;

namespace Visualizer.IO
{
    public class MeshBuilder : MonoBehaviour
    {
        public static Mesh BuildMesh(MeshData data)
        {
            var mesh = new Mesh();

            // Maps rounded vertex positions to final vertex indices
            Dictionary<Vector3, int> vertexMap = new Dictionary<Vector3, int>();
            
            // Final mesh buffers
            List<Vector3> weldedVertices = new List<Vector3>();
            List<Vector3> weldedNormals = new List<Vector3>();
            List<Color> weldedColors = new List<Color>();
            List<int> triangles = new List<int>();

            // Accumulators for averaging normals and slope values
            Dictionary<int, List<Vector3>> normalAccum = new Dictionary<int, List<Vector3>>();
            Dictionary<int, List<float>> slopeAccum = new Dictionary<int, List<float>>();

            // Loop through faces and weld vertices
            for (int i = 0; i < data.Faces.Length; i++)
            {
                int[] face = data.Faces[i];
                for (int j = 0; j < 3; j++)
                {
                    int originalIndex = face[j];
                    float[] v = data.Vertices[originalIndex];

                    // Convert -Z-up to Unity's Y-up: (X, Z, -Y)
                    Vector3 position = new Vector3(v[0], v[2], v[1]);

                    // Round the position to reduce floating point noise
                    Vector3 key = new Vector3(
                        Mathf.Round(position.x * 10000f) / 10000f,
                        Mathf.Round(position.y * 10000f) / 10000f,
                        Mathf.Round(position.z * 10000f) / 10000f
                    );

                    // If this vertex position is new, register it
                    if (!vertexMap.TryGetValue(key, out int newIndex))
                    {
                        newIndex = weldedVertices.Count;
                        vertexMap[key] = newIndex;
                        weldedVertices.Add(position);
                        weldedNormals.Add(Vector3.zero); // placeholder
                        weldedColors.Add(Color.black); // placeholder
                        normalAccum[newIndex] = new List<Vector3>();
                        slopeAccum[newIndex] = new List<float>();
                    }

                    // Add normal if available
                    if (data.Vertex_Normals != null)
                    {
                        float[] n = data.Vertex_Normals[originalIndex];

                        // Rotate normals too (Z-up → Y-up)
                        Vector3 normal = new Vector3(n[0], n[2], n[1]);
                        normalAccum[newIndex].Add(normal);
                    }

                    // Add slope value for averaging
                    if (data.Slope_Angles != null)
                    {
                        slopeAccum[newIndex].Add(data.Slope_Angles[originalIndex]);
                    }

                    triangles.Add(newIndex);
                }
            }

            // Average accumulated normals
            for (int i = 0; i < weldedNormals.Count; i++)
            {
                if (normalAccum.TryGetValue(i, out var normals))
                {
                    Vector3 avg = Vector3.zero;
                    foreach (var n in normals)
                        avg += n;
                    avg.Normalize();
                    weldedNormals[i] = avg;
                }
            }

            // Average slope values and encode into vertex colors
            for (int i = 0; i < weldedColors.Count; i++)
            {
                if (slopeAccum.TryGetValue(i, out var slopes))
                {
                    float avg = 0f;
                    foreach (var s in slopes)
                        avg += s;
                    avg /= slopes.Count;

                    float t = Mathf.InverseLerp(0f, 90f, avg);
                    weldedColors[i] = new Color(t, 1f, 1f, 0f);
                }
            }

            // Assign data to mesh
            mesh.vertices = weldedVertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = weldedNormals.ToArray();
            mesh.colors = weldedColors.ToArray();
            mesh.RecalculateBounds();

            Debug.Log($"Vertex count: {mesh.vertexCount}, Triangle count: {mesh.triangles.Length / 3}");

            return mesh;
        }
    }
}