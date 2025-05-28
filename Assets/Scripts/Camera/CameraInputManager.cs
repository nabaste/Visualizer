/// <summary>
/// Manages which camera view is currently focused based on user clicks.
/// Determines input routing and updates UI labels to reflect focus.
/// Prevents focus changes when a view is maximized (handled by ViewportManager).
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using Visualizer.Core;

namespace Visualizer.Camera
{
    public enum CameraView { Top, Front, Right, Perspective }
    public class CameraInputManager : MonoBehaviour
    {
        public CameraView focusedView = CameraView.Perspective;

        public ViewportManager viewportManager;

        public Image topLabel, frontLabel, rightLabel, perspectiveLabel;
        public Color focusedColor = Color.yellow;
        public Color unfocusedColor = Color.white;

        private void Start()
        {
            UpdateLabelColors();
        }
    
        private void Update()
        {
            // Respect maximized state: don't change focus
            if (viewportManager && viewportManager.IsMaximized)
                return;

            // Check any mouse click
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 screenSize = new Vector2(Screen.width, Screen.height);

                float xNorm = mousePos.x / screenSize.x;
                float yNorm = mousePos.y / screenSize.y;

                if (yNorm > 0.9f) return;
                CameraView clickedView = GetClickedView(xNorm, yNorm);
                SetFocusedCamera(clickedView);
            }
        }
        
        /// <summary>
        /// Determines which camera view was clicked based on screen coordinates.
        /// </summary>
        private CameraView GetClickedView(float x, float y)
        {
            if (y is > 0.45f )
            {
                if (x < 0.5f) return CameraView.Top;
                else return CameraView.Perspective;
            }
            else
            {
                if (x < 0.5f) return CameraView.Front;
                else return CameraView.Right;
            }
        }

        /// <summary>
        /// Changes focus to the specified camera and updates the UI.
        /// </summary>
        private void SetFocusedCamera(CameraView view)
        {
            if (focusedView == view) return;

            focusedView = view;
            UpdateLabelColors();
        }

        /// <summary>
        /// Updates the UI label colors to reflect the currently focused camera.
        /// </summary>
        private void UpdateLabelColors()
        {
            topLabel.color = (focusedView == CameraView.Top) ? focusedColor : unfocusedColor;
            frontLabel.color = (focusedView == CameraView.Front) ? focusedColor : unfocusedColor;
            rightLabel.color = (focusedView == CameraView.Right) ? focusedColor : unfocusedColor;
            perspectiveLabel.color = (focusedView == CameraView.Perspective) ? focusedColor : unfocusedColor;
        }

        public bool IsFocused(CameraView view)
        {
            return focusedView == view;
        }
    }
}