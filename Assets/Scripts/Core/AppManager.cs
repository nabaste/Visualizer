/// <summary>
/// AppManager is the main coordinator of the application lifecycle.
/// It manages loading mesh data from disk, instantiating and assigning the mesh,
/// handling viewport layout, switching shading modes, and dispatching interaction logic.
/// </summary>


#if UNITY_STANDALONE_WIN
#endif

using System.IO;
using System.Windows.Forms;
using UnityEditor;
using UnityEngine;
using Visualizer.Interaction;
using Visualizer.IO;
using Visualizer.Visualization;

namespace Visualizer.Core
{
    public class AppManager : MonoBehaviour
    {
        [Header("References")]
        public ShaderManager shaderManager;
        public ViewportManager viewportManager;

        private GameObject _currentMeshGo;

        public delegate void ShadingModeChangedEvent(ShadingMode mode);
        public event ShadingModeChangedEvent OnShadingModeChanged;

        public bool EnableInteraction { get; private set; } = false;

        void Start()
        {
            Debug.Log("AppManager initialized.");
            viewportManager.ResetAllViews();
        }

        /// <summary>
        /// Opens a file dialog and loads mesh data from a JSON file.
        /// The dialog implementation depends on platform (Editor or Windows).
        /// </summary>
        public void LoadMeshFromFile()
        {
#if UNITY_EDITOR
        
            string editorPath = EditorUtility.OpenFilePanel("Open JSON File", "", "json");
            if (string.IsNullOrEmpty(editorPath)) return;
            string json = File.ReadAllText(editorPath);

#elif UNITY_STANDALONE_WIN
        
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Title = "Select a JSON file";
        ofd.Filter = "JSON files (*.json)|*.json";
        if (ofd.ShowDialog() != DialogResult.OK) return;
        string buildPath = ofd.FileName;
        string json = File.ReadAllText(buildPath);

#endif
        
            MeshData meshData = JsonLoader.ParseJson(json);

            if (meshData == null)
            {
                Debug.LogError("Failed to parse mesh data.");
                return;
            }

            GenerateMesh(meshData);
        }

        /// <summary>
        /// Instantiates a new GameObject containing the loaded mesh, its renderer,
        /// a collider, and interaction components. Previous mesh (if any) is destroyed.
        /// </summary>
        private void GenerateMesh(MeshData data)
        {
            if (_currentMeshGo != null)
                Destroy(_currentMeshGo);

            Mesh mesh = MeshBuilder.BuildMesh(data);

            _currentMeshGo = new GameObject("GeneratedMesh", typeof(MeshFilter), typeof(MeshRenderer));
            _currentMeshGo.GetComponent<MeshFilter>().mesh = mesh;
            _currentMeshGo.GetComponent<MeshRenderer>().material = shaderManager.GetCurrentMaterial();

            _currentMeshGo.AddComponent<MeshClickHandler>();
            _currentMeshGo.AddComponent<FaceHighlighter>();
            _currentMeshGo.AddComponent<MeshCollider>().sharedMesh = mesh;
        

            // Could notify other systems here (update bounding boxes, focus camera)
            Debug.Log("Mesh generated and assigned.");
        }

        /// <summary>
        /// Updates the active shading mode. Notifies listeners and updates the mesh material.
        /// Also enables interaction features if FaceSelection mode is chosen.
        /// </summary>
        public void SetShadingMode(ShadingMode mode)
        {
            OnShadingModeChanged?.Invoke(mode);
        
            shaderManager.ApplyShading(_currentMeshGo ? _currentMeshGo.GetComponent<MeshRenderer>() : null, mode);
        
            EnableInteraction = mode == ShadingMode.FaceSelection;  
        }

        public void MaximizeView(string viewName)
        {
            viewportManager.Maximize(viewName);
        }
    
        public void QuitApplication()
        {
#if UNITY_STANDALONE_WIN
            DialogResult result = MessageBox.Show("Are you sure you want to quit?", "Exit Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UnityEngine.Application.Quit();
            }
#endif
        }

        /// <summary>
        /// Resets the app to its initial state after confirmation.
        /// Clears the current mesh and resets viewport and shading mode.
        /// </summary>
        public void ResetApp()
        {
#if UNITY_STANDALONE_WIN
            DialogResult result = MessageBox.Show("Are you sure you want to start a new file? All progress will be lost", "New File",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;
            if (_currentMeshGo)
            {
                Destroy(_currentMeshGo);
                _currentMeshGo = null;
            }

            viewportManager.ResetAllViews();
        
            SetShadingMode(ShadingMode.Shaded);
#endif
        }
    }
}