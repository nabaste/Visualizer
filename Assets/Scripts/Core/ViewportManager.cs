using UnityEngine;

namespace Visualizer.Core
{
    public class ViewportManager : MonoBehaviour
    {
        public UnityEngine.Camera topCam, frontCam, rightCam, perspectiveCam;

        public GameObject uiOverlay1, uiOverlay2;
        private UnityEngine.Camera _currentMaximizedCamera = null;

        public bool IsMaximized => _currentMaximizedCamera;


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

        public void ResetAllViews()
        {
            topCam.enabled = frontCam.enabled = rightCam.enabled = perspectiveCam.enabled = true;

            topCam.rect = new Rect(0f, 0.45f, 0.5f, 0.45f);
            perspectiveCam.rect = new Rect(0.5f, 0.45f, 0.5f, 0.45f);
            frontCam.rect = new Rect(0f, 0f, 0.5f, 0.45f);
            rightCam.rect = new Rect(0.5f, 0f, 0.5f, 0.45f);
        }

        private void ResetCameraRects()
        {
            foreach (var cam in new[] { topCam, frontCam, rightCam, perspectiveCam })
            {
                cam.rect = new Rect(0, 0, 0, 0);
            }
        }
    }
}