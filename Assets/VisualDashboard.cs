using RVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDashboard : MonoBehaviour
{
    public double speed;
    public float rpm;
    public GasMotor engine;
    public VehicleParent vp;
    public float maxSpeed;
    public float maxRPM;

    public Transform speedNeedle; // Reference to the speedometer needle
    public Transform rpmNeedle; // Reference to the RPM meter needle

    private float speedMinRotation = 90f; // Rotation when speed is 0
    private float speedMaxRotation = -90f; // Rotation when speed is at max
    private float rpmMinRotation = 90f; // Rotation when RPM is 0
    private float rpmMaxRotation = -90f; // Rotation when RPM is at max

    // Start is called before the first frame update
    void Start()
    {
        if (engine != null)
        {
            rpm = engine.targetPitch;
            maxRPM = 1;
        }

        if (vp != null)
        {
            // Initial speed calculation
            speed = ((vp.velMag * 2.23694f) * 1.6093);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vp != null)
        {
            // Update speed based on current velocity
            speed = ((vp.velMag * 2.23694f) * 1.6093);
        }

        if (engine != null)
        {
            // Update RPM based on engine's target pitch
            rpm = engine.targetPitch;
        }

        // Update needle rotations
        UpdateNeedleRotation(speedNeedle, (float)speed, maxSpeed, speedMinRotation, speedMaxRotation);
        UpdateNeedleRotation(rpmNeedle, rpm, maxRPM, rpmMinRotation, rpmMaxRotation);
    }

    private void UpdateNeedleRotation(Transform needle, float value, float maxValue, float minRotation, float maxRotation)
    {
        if (needle == null) return;

        // Clamp the value between 0 and maxValue
        value = Mathf.Clamp(value, 0, maxValue);

        // Map value to rotation angle
        float normalizedValue = value / maxValue;
        float rotationAngle = Mathf.Lerp(minRotation, maxRotation, normalizedValue);

        // Apply the rotation to the needle
        needle.localRotation = Quaternion.Euler(0, 0, rotationAngle);
    }
}
