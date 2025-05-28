/// <summary>
/// Handles user interaction with the mesh via raycasting.
/// Detects clicks on mesh faces and notifies the FaceHighlighter component to toggle selection.
/// Requires a MeshCollider on the same GameObject.
/// </summary>

using Visualizer.Core;
using UnityEngine;


namespace Visualizer.Interaction
{
    [RequireComponent(typeof(MeshCollider))]
    public class MeshClickHandler : MonoBehaviour
    {
        public FaceHighlighter faceHighlighter;
        private UnityEngine.Camera _mainCam;
        private AppManager _appManager;

        private void Awake()
        {
            _appManager = FindAnyObjectByType<AppManager>();

        }

        private void Start()
        {
            _mainCam = UnityEngine.Camera.main;

            // Fallback in case faceHighlighter is not manually assigned
            if (!faceHighlighter)
                faceHighlighter = GetComponent<FaceHighlighter>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_appManager.EnableInteraction) return;
                Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        faceHighlighter?.ToggleFaceHighlight(hit.triangleIndex);
                    }
                }
            }
        }
    }
}