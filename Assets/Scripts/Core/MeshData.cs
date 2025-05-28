using UnityEngine;

namespace Visualizer.Core
{
    [System.Serializable]
    public class MeshData
    {
        public float[][] Vertices;
        public int[][] Faces;

        [SerializeField] //do I need this?
        public float[][] Vertex_Normals;

        [SerializeField] public float[] Slope_Angles;
    }
}