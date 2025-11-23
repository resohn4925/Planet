using UnityEngine;

namespace CoolProceduralPlanets.CameraController
{
    public class SimpleCameraController : MonoBehaviour
    {
        class CameraState
        {
            public float yaw;
            public float pitch;
            public float roll;
            public float x;
            public float y;
            public float z;

            public void SetFromTransform(Transform t)
            {
                pitch = t.eulerAngles.x;
                yaw = t.eulerAngles.y;
                roll = t.eulerAngles.z;
                x = t.position.x;
                y = t.position.y;
                z = t.position.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

                x += rotatedTranslation.x;
                y += rotatedTranslation.y;
                z += rotatedTranslation.z;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

                x = Mathf.Lerp(x, target.x, positionLerpPct);
                y = Mathf.Lerp(y, target.y, positionLerpPct);
                z = Mathf.Lerp(z, target.z, positionLerpPct);
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(pitch, yaw, roll);
                t.position = new Vector3(x, y, z);
            }
        }

        CameraState m_TargetCameraState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Header("Movement Settings")]
        public float movementSpeed = 10f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 3f)]
        public float positionLerpTime = 0.2f;

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 3f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Sensitivity for mouse or touch rotation.")]
        public float rotationSensitivity = 0.1f;

        private float lastPinchDistance;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
        }

        Vector3 GetKeyboardTranslation()
        {
            Vector3 direction = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
            if (Input.GetKey(KeyCode.Q)) direction += Vector3.down;
            if (Input.GetKey(KeyCode.E)) direction += Vector3.up;
            return direction;
        }

        void Update()
        {
            Vector3 translation = Vector3.zero;

            // Handle pinch-to-zoom
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);

                if (lastPinchDistance != 0)
                {
                    float delta = (currentPinchDistance - lastPinchDistance) * 0.01f;
                    translation += Vector3.forward * delta * movementSpeed;
                }

                lastPinchDistance = currentPinchDistance;
            }
            else
            {
                lastPinchDistance = 0;
            }

            // Handle touch rotation
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.deltaPosition.x * rotationSensitivity;
                    float deltaY = touch.deltaPosition.y * rotationSensitivity;

                    m_TargetCameraState.yaw += deltaX;
                    m_TargetCameraState.pitch -= deltaY;
                }
            }

            // Handle keyboard and mouse input
            if (Input.touchCount == 0) // Only allow mouse and keyboard if no touches are detected
            {
                // Rotation with mouse
                if (Input.GetMouseButton(1))
                {
                    float mouseX = Input.GetAxis("Mouse X");
                    float mouseY = Input.GetAxis("Mouse Y");

                    m_TargetCameraState.yaw += mouseX * rotationSensitivity * 10f;
                    m_TargetCameraState.pitch -= mouseY * rotationSensitivity * 10f;
                }

                // Translation with keyboard
                translation += GetKeyboardTranslation() * movementSpeed * Time.deltaTime;

                // Scroll to zoom
                translation += Vector3.forward * Input.mouseScrollDelta.y * movementSpeed * 0.1f;
            }

            // Three-finger gesture for adjusting speed
            if (Input.touchCount == 3)
            {
                bool allFingersMovingDown = true;

                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetTouch(i).deltaPosition.y >= 0)
                    {
                        allFingersMovingDown = false;
                        break;
                    }
                }

                if (allFingersMovingDown)
                {
                    movementSpeed = Mathf.Max(movementSpeed - 0.1f, 1f);
                }
            }

            // Apply translation
            m_TargetCameraState.Translate(translation);

            // Framerate-independent interpolation
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }
    }
}
