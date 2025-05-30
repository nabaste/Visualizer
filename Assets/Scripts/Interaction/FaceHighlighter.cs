/// <summary>
/// Controls highlighting of individual mesh faces by modifying vertex colors' alpha channel.
/// Allows toggling highlight visibility per triangle on the attached MeshFilter's mesh.
/// </summary>

using System.Collections.Generic;
using UnityEngine;

namespace Visualizer.Interaction
{
    [RequireComponent(typeof(MeshFilter))]
    public class FaceHighlighter : MonoBehaviour
    {
        [Tooltip("Set alpha = 1 to show highlight")]
        public float highlightAlpha = 1f;

        public float baseAlpha = 0f;

        private Mesh _mesh;
        private HashSet<int> _selectedTriangles = new HashSet<int>(); // Tracks currently highlighted triangles

        private Color[] _colors; // Per-vertex colors of the mesh
        private int[] _triangles; // Indices defining mesh triangles

        private void Awake()
        {
            _mesh = GetComponent<MeshFilter>().mesh;
            _triangles = _mesh.triangles;
            _colors = _mesh.colors;

            if (_colors == null || _colors.Length != _mesh.vertexCount)
            {
                _colors = new Color[_mesh.vertexCount];
                for (int i = 0; i < _colors.Length; i++)
                    _colors[i] = new Color(0, 0, 0, baseAlpha);
            }
        }

        /// <summary>
        /// Toggles the highlight state of the face specified by the triangle index.
        /// </summary>
        public void ToggleFaceHighlight(int triangleIndex)
        {
            if (triangleIndex * 3 + 2 >= _triangles.Length)
                return;

            if (_selectedTriangles.Contains(triangleIndex))
            {
                _selectedTriangles.Remove(triangleIndex);
                SetFaceAlpha(triangleIndex, baseAlpha);
            }
            else
            {
                _selectedTriangles.Add(triangleIndex);
                SetFaceAlpha(triangleIndex, highlightAlpha);
            }

            _mesh.colors = _colors;
        }

        private void SetFaceAlpha(int triangleIndex, float alpha)
        {
            int i0 = _triangles[triangleIndex * 3 + 0];
            int i1 = _triangles[triangleIndex * 3 + 1];
            int i2 = _triangles[triangleIndex * 3 + 2];

            _colors[i0].a = alpha;
            _colors[i1].a = alpha;
            _colors[i2].a = alpha;

        }
    }
}
