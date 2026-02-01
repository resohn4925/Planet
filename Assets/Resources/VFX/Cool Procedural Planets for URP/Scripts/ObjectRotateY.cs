using UnityEngine;

namespace CoolProceduralPlanets.ObjectRotateY{

public class ObjectRotateY : MonoBehaviour
{
    // Public variable with tooltip
    [Tooltip("Rotation speed around Y axis (degrees per frame). Use +/- keys to adjust.")]
    public float rotateSpeed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around Y axis using the current rotateSpeed
        transform.Rotate(0f, rotateSpeed, 0f);

        // Check for input to increase rotation speed
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            rotateSpeed += 0.005f;
            Debug.Log($"Rotation speed increased to: {rotateSpeed}");
        }

        // Check for input to decrease rotation speed
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            rotateSpeed -= 0.005f;
            Debug.Log($"Rotation speed decreased to: {rotateSpeed}");
        }
    }
}
}