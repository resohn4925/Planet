using UnityEngine;

namespace StarSystem
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlanetOrbiter : MonoBehaviour
    {
        [Header("Orbital Parameters")]

        [Tooltip("The parent object the planet orbits around.")]
        public Transform parentObject;

        [Tooltip("The base speed at which the planet moves along its orbit.")]
        public float baseOrbitalSpeed = 1f;

        [Tooltip("The distance of the planet from the parent object. If 0, it will be calculated from the scene.")]
        public float distance = 0f;

        [Tooltip("The horizontal stretch factor of the orbit.")]
        public float offsetSin = 1f;

        [Tooltip("The vertical stretch factor of the orbit.")]
        public float offsetCos = 1f;

        [Tooltip("The inclination angle of the orbit relative to the horizontal plane.")]
        public float eclipticAngle = 0f;

        [Header("Planet Rotation")]

        [Tooltip("The speed at which the planet rotates around its own axis.")]
        public float selfRotationSpeed = 50f;

        [Tooltip("The axis around which the planet rotates. Default is the Y axis.")]
        public Vector3 rotationAxis = Vector3.up;

        [Header("Orbit Visualization")]

        [Tooltip("The material used to render the planet's orbit.")]
        public Material orbitMaterial;

        [Tooltip("The number of segments used to draw the orbit line.")]
        public int orbitSegments = 100;

        private float currentAngle; // The current angle of the planet in its orbit
        private LineRenderer lineRenderer;
        private float orbitalSpeedMultiplier = 1f; // Multiplier for orbital speed
        private bool drawOrbit = true; // Orbit drawing state

        private void Start()
        {
            if (parentObject == null)
            {
                Debug.LogError("Parent object is not assigned!");
                return;
            }

            // If distance is 0, calculate it from the scene
            if (distance == 0f)
            {
                distance = Vector3.Distance(transform.position, parentObject.position);
                Debug.Log($"Distance automatically set to {distance} for {gameObject.name}");
            }

            currentAngle = 0f;

            // Configure the LineRenderer for orbit visualization
            lineRenderer = GetComponent<LineRenderer>();
            if (orbitMaterial != null)
            {
                lineRenderer.material = orbitMaterial;
                lineRenderer.widthMultiplier = 0.02f;
                lineRenderer.loop = true;
            }

            DrawOrbit(); // Initial orbit drawing
        }

        private void Update()
        {
            if (parentObject == null) return;

            // Handle input for speed adjustment and toggling orbit drawing
            HandleInput();

            // Update the planet's position along the orbit
            Vector3 newPosition = GetPosition(parentObject.position, distance, currentAngle, offsetSin, offsetCos, eclipticAngle);
            transform.position = newPosition;

            // Rotate the planet around its own axis
            transform.Rotate(rotationAxis, selfRotationSpeed * Time.deltaTime, Space.Self);

            // Increment the angle for orbital movement
            currentAngle += baseOrbitalSpeed * orbitalSpeedMultiplier * Time.deltaTime;

            // Redraw the orbit to follow the parent object's position (if enabled)
            if (drawOrbit)
                DrawOrbit();
        }

        private void HandleInput()
        {
            // Adjust orbital speed
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                orbitalSpeedMultiplier = Mathf.Max(0f, orbitalSpeedMultiplier - 0.1f);
                Debug.Log($"Orbital speed multiplier decreased to {orbitalSpeedMultiplier}");
            }
            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                orbitalSpeedMultiplier += 0.1f;
                Debug.Log($"Orbital speed multiplier increased to {orbitalSpeedMultiplier}");
            }

            // Toggle orbit drawing
            if (Input.GetKeyDown(KeyCode.F1))
            {
                drawOrbit = !drawOrbit;
                if (lineRenderer != null)
                    lineRenderer.enabled = drawOrbit;

                Debug.Log($"Orbit drawing toggled: {(drawOrbit ? "ON" : "OFF")}");
            }
        }

        private void DrawOrbit()
        {
            if (lineRenderer == null) return;

            lineRenderer.positionCount = orbitSegments + 1;

            Vector3[] orbitPoints = new Vector3[orbitSegments + 1];
            Quaternion rotation = Quaternion.Euler(0, 0, eclipticAngle);

            for (int i = 0; i <= orbitSegments; i++)
            {
                float angle = (float)i / orbitSegments * 2 * Mathf.PI;
                Vector3 orbitPoint = new Vector3(
                    Mathf.Sin(angle) * distance * offsetSin,
                    0,
                    Mathf.Cos(angle) * distance * offsetCos
                );
                orbitPoint = rotation * orbitPoint + parentObject.position;
                orbitPoints[i] = orbitPoint;
            }

            lineRenderer.SetPositions(orbitPoints);
        }

        private Vector3 GetPosition(Vector3 around, float dist, float angle, float sin, float cos, float eclipticAngle)
        {
            // Calculate the position of the planet along its orbit
            Quaternion rotation = Quaternion.Euler(0, 0, eclipticAngle);
            Vector3 pos = new Vector3(
                Mathf.Sin(angle) * dist * sin,
                0,
                Mathf.Cos(angle) * dist * cos
            );
            return rotation * pos + around;
        }
    }
}
