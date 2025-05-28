using UnityEngine;

namespace Visualizer.Camera
{
    public class CameraController : MonoBehaviour
    {
        public CameraView cameraView = CameraView.Perspective;
        public Transform target;
        public float orbitDistance = 130f;
        public float orbitSpeed = 30f;
        public float panSpeed = 0.1f;
        public float zoomSpeed = 5f;
        public float minZoom = 2f;
        public float maxZoom = 100f;

        private Vector3 _lastMousePosition;
        private UnityEngine.Camera _cam;

        public CameraInputManager inputManager;

        private void Start()
        {
            _cam = GetComponent<UnityEngine.Camera>();

            if (cameraView == CameraView.Perspective && target)
            {
                // Position the camera for orbit
                transform.position = target.position - transform.forward * orbitDistance;
            }
        }

        private void Update()
        {
            if (!inputManager.IsFocused(cameraView))
                return;

            HandleZoom();

            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                _lastMousePosition = Input.mousePosition;

            if (cameraView == CameraView.Perspective)
            {
                HandleOrbit();
            }

            HandlePan();
        }

        private void HandleZoom()
        {
            float scroll = Input.mouseScrollDelta.y;

            if (scroll != 0)
            {
                if (cameraView == CameraView.Perspective)
                {
                    transform.position += transform.forward * (scroll * zoomSpeed);
                    orbitDistance = Vector3.Distance(transform.position, target ? target.position : Vector3.zero);
                }
                else
                {
                    _cam.orthographicSize -= scroll * zoomSpeed * 0.5f;
                    _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, minZoom, maxZoom);
                }
            }
        }

        private void HandleOrbit()
        {
            if (!Input.GetMouseButton(1)) return; // Right mouse
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            _lastMousePosition = Input.mousePosition;

            float yaw = delta.x * orbitSpeed * Time.deltaTime;
            float pitch = -delta.y * orbitSpeed * Time.deltaTime;

            // Rotate around the target
            if (!target) return;
            transform.RotateAround(target.position, Vector3.up, yaw);
            transform.RotateAround(target.position, transform.right, pitch);
            transform.LookAt(target);
        }

        private void HandlePan()
        {
            if (!Input.GetMouseButton(2)) return; // Middle mouse
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            _lastMousePosition = Input.mousePosition;

            Vector3 pan = new Vector3(-delta.x * panSpeed, -delta.y * panSpeed, 0f);

            if (cameraView == CameraView.Perspective)
            {
                // Pan in camera space
                transform.Translate(pan, Space.Self);
                if (target) target.Translate(pan, Space.Self);
            }
            else
            {
                // Pan orthographic camera
                Vector3 move = _cam.transform.right * pan.x + _cam.transform.up * pan.y;
                transform.position += move;
            }
        }
    }
}