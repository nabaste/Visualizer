/// <summary>
/// ViewportManager handles the layout and visibility of the 4 main view cameras (Top, Front, Right, Perspective).
/// It manages toggling between multi-view and maximized view modes, controls camera viewports,
/// and shows/hides overlay UI panels accordingly.
/// </summary>

using UnityEngine;

namespace Visualizer.Core
{
    public class ViewportManager : MonoBehaviour
    {
        public UnityEngine.Camera topCam, frontCam, rightCam, perspectiveCam;

        public GameObject uiOverlay1, uiOverlay2;
        private UnityEngine.Camera _currentMaximizedCamera = null;

        public bool IsMaximized => _currentMaximizedCamera;

        /// <summary>
        /// Maximizes or restores a given view by name.
        /// If the view is already maximized, it resets to the 4-view layout.
        /// Also toggles UI overlay visibility.
        /// </summary>
        public void Maximize(string view)
        {
            UnityEngine.Camera targetCam = GetCameraByName(view);

            if (targetCam == null)
            {
                Debug.LogWarning($"Camera '{view}' not found.");
                return;
            }

            if (_currentMaximizedCamera == targetCam)
            {
                // Already maximized → reset to full layout
                ResetAllViews();
                _currentMaximizedCamera = null;

                uiOverlay1.SetActive(true);
                uiOverlay2.SetActive(true);
            }
            else
            {
                // Maximize selected camera
                SetMaximizedView(targetCam);
                _currentMaximizedCamera = targetCam;

                uiOverlay1.SetActive(false);
                uiOverlay2.SetActive(false);
            }
        }

        /// <summary>
        /// Disables all cameras except the one passed in, and gives it full screen real estate.
        /// </summary>
        private void SetMaximizedView(UnityEngine.Camera cam)
        {
            ResetCameraRects();
            DisableOthers(cam);
            cam.rect = new Rect(0, 0, 1, 0.9f); // full screen, minus top bar
        }

        private void DisableOthers(UnityEngine.Camera active)
        {
            foreach (var cam in new[] { topCam, frontCam, rightCam, perspectiveCam })
                cam.enabled = (cam == active);
        }

        private UnityEngine.Camera GetCameraByName(string view)
        {
            return view.ToLower() switch
            {
                "top" => topCam,
                "front" => frontCam,
                "right" => rightCam,
                "perspective" => perspectiveCam,
                _ => null
            };
        }

        /// <summary>
        /// Restores all 4 cameras to their default 2x2 layout and enables them all.
        /// </summary>
        public void ResetAllViews()
        {
            topCam.enabled = frontCam.enabled = rightCam.enabled = perspectiveCam.enabled = true;

            topCam.rect = new Rect(0f, 0.45f, 0.5f, 0.45f);
            perspectiveCam.rect = new Rect(0.5f, 0.45f, 0.5f, 0.45f);
            frontCam.rect = new Rect(0f, 0f, 0.5f, 0.45f);
            rightCam.rect = new Rect(0.5f, 0f, 0.5f, 0.45f);
        }

        /// <summary>
        /// Clears all camera rects (sets them to 0), used before setting a maximized layout.
        /// </summary>
        private void ResetCameraRects()
        {
            foreach (var cam in new[] { topCam, frontCam, rightCam, perspectiveCam })
            {
                cam.rect = new Rect(0, 0, 0, 0);
            }
        }
    }
}