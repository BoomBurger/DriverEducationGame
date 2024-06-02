using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {

	//The wheels on the blocky car go round and round, left and right, up and down..... dont forget forward and backward

	public WheelCollider wheelCollider;
	[Range(0.5f, 2f)]
	public float wheelWidth = 1.5f;
	public bool flippedOnY;
	private Vector3 wheelPosition;
	private Quaternion wheelRotation;

	void Start () 
	{
	
	}

	void Update () 
	{
		wheelCollider.GetWorldPose (out wheelPosition, out wheelRotation);
		transform.position = wheelPosition;
		if (flippedOnY) transform.rotation = wheelRotation*Quaternion.Euler(new Vector3(0, 180));else transform.rotation = wheelRotation;
		transform.localScale = new Vector3(wheelWidth, 1, 1);
		wheelCollider.ConfigureVehicleSubsteps (5, 12, 15);
	}
}
