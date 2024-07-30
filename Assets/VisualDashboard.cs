using RVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDashboard : MonoBehaviour
{
    public double speed;
    public float rpm;
    public GasMotor engine;
    private VehicleParent vp;
    public float maxSpeed;
    public float maxRPM;

    // Start is called before the first frame update
    void Start()
    {
        rpm = engine.targetPitch;
        vp = GetComponent<VehicleParent>();
        speed = ((vp.velMag * 2.23694f) * 1.6093);
        maxRPM = engine.maxRPM;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
