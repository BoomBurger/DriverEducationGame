using RVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCarCamera : MonoBehaviour
{
    public Transform target;
    public Transform cameraLook;

    public Vector3 positionOffset;

    private float xInput;
    private float yInput;
    private Quaternion rotationOffset;


    Vector3 lookDir;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += positionOffset;
        
    }

    void Update()
    {


        if (xInput != 0f || yInput != 0f)
        {
            rotationOffset.eulerAngles = new Vector3(0 + target.rotation.x, (Mathf.Atan2(xInput, yInput) * 180 / Mathf.PI), 0 + target.rotation.z);

            transform.rotation = rotationOffset;

        }
        else
        {
            // Look at the cameraLook target
            transform.LookAt(cameraLook);

        }
    }


    // function for setting the rotation input of the camera
    public void SetInput(float x, float y)
    {
        xInput = x;
        yInput = y;
    }
}
