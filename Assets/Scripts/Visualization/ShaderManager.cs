/// <summary>
/// Manages material assignment based on shading modes for mesh visualization.
/// </summary>

using System;
using UnityEngine;


namespace Visualizer.Visualization
{
    public class ShaderManager : MonoBehaviour
    {
        public Material shadedMaterial,
            shadedWireframeMaterial,
            wireframeMaterial,
            slopeHeatmapMaterial,
            faceSelectionMaterial;

        private ShadingMode _currentMode;

        /// <summary>
        /// Applies a specific shading mode to the given MeshRenderer.
        /// Optionally invoked without a renderer to only store mode state.
        /// </summary>
        public void ApplyShading(MeshRenderer meshRenderer = null, ShadingMode mode = ShadingMode.Shaded)
        {
            _currentMode = mode;

            if (!meshRenderer) return;

            switch (mode)
            {
                case ShadingMode.Shaded:
                    meshRenderer.material = shadedMaterial;
                    break;

                case ShadingMode.ShadedWireframe:
                    meshRenderer.material = shadedWireframeMaterial;
                    break;

                case ShadingMode.Wireframe:
                    meshRenderer.material = wireframeMaterial;
                    break;

                case ShadingMode.SlopeHeatmap:
                    meshRenderer.material = slopeHeatmapMaterial;
                    break;

                case ShadingMode.FaceSelection:
                    meshRenderer.material = faceSelectionMaterial;
                    break;

                default:
                    meshRenderer.material = shadedMaterial;
                    break;
            }
        }

        /// <summary>
        /// Returns the material associated with the currently selected shading mode.
        /// Useful for applying mode-consistent materials when spawning new meshes.
        /// </summary>
        public Material GetCurrentMaterial()
        {
            return _currentMode switch
            {
                ShadingMode.Shaded => shadedMaterial,
                ShadingMode.ShadedWireframe => shadedWireframeMaterial,
                ShadingMode.Wireframe => wireframeMaterial,
                ShadingMode.SlopeHeatmap => slopeHeatmapMaterial,
                ShadingMode.FaceSelection => faceSelectionMaterial,
                _ => shadedMaterial
            };
        }
    }


    public enum ShadingMode
    {
        Shaded,
        ShadedWireframe,
        Wireframe,
        SlopeHeatmap,
        FaceSelection
    }
}
