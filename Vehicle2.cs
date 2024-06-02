using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle2 : MonoBehaviour {

	[Header("PowerTrain")]
	public int maxPower, maxBrakeTorque, speedLimit;

	[Header("Wheels")]
	public WheelCollider[] frontWheels, backWheels;
	public GameObject wheel;
	public float wheelWidth;
	public int steerAngle;

	[Header("References")]
	public GameObject pointer;

	[Header("AI")]
	public float reverseWaitTime;
	public float reverseTime;

	[Header("Misc")]
	public Material[] bodyMaterialList;
	public LayerMask vehicleLayerMask;

	private GameObject tempWheel, nextPoint, futurePoint;
	private NavMeshAgent eyes;
	private Rigidbody rigid;
	private bool reverse, fix;

	public enum DriveBias {
		BackWheelDrive,
		FrontWheelDrive,
		AllWheelDrive
	}

	public enum SteerBias {
		FrontWheelSteering,
		BackWheelSteering,
		AllWheelSteering
	}

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
		eyes = GetComponentInChildren<NavMeshAgent>();
		for (int i = 0; i < frontWheels.Length; i++) {
			if (frontWheels[i].transform.position.x < transform.position.x) {
				tempWheel = Instantiate(wheel, frontWheels[i].transform.position, transform.rotation, transform) as GameObject;
				Wheel tempComp = tempWheel.AddComponent<Wheel>();
				tempComp.wheelCollider = frontWheels[i].GetComponent<WheelCollider>();
				tempComp.flippedOnY = true;
			}else if (frontWheels[i].transform.position.x > transform.position.x) {
				tempWheel = Instantiate(wheel, frontWheels[i].transform.position, transform.rotation, transform) as GameObject;
				Wheel tempComp = tempWheel.AddComponent<Wheel>();
				tempComp.wheelCollider = frontWheels[i].GetComponent<WheelCollider>();
				tempComp.flippedOnY = false;
			}
		}
		for (int i = 0; i < backWheels.Length; i++) {
			if (backWheels[i].transform.position.x < transform.position.x) {
				tempWheel = Instantiate(wheel, backWheels[i].transform.position, transform.rotation, transform) as GameObject;
				Wheel tempComp = tempWheel.AddComponent<Wheel>();
				tempComp.wheelCollider = backWheels[i].GetComponent<WheelCollider>();
				tempComp.flippedOnY = true;
			}else if (backWheels[i].transform.position.x > transform.position.x) {
				tempWheel = Instantiate(wheel, backWheels[i].transform.position, transform.rotation, transform) as GameObject;
				Wheel tempComp = tempWheel.AddComponent<Wheel>();
				tempComp.wheelCollider = backWheels[i].GetComponent<WheelCollider>();
				tempComp.flippedOnY = false;
			}
		}
		nextPoint = Point("RacingPoint");
		futurePoint = Point("RacingPoint").GetComponent<Point>().points[0];
		eyes.SetDestination(nextPoint.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		float velocity = transform.InverseTransformDirection(rigid.velocity).z;
		if (Vector3.Distance(new Vector3(pointer.transform.position.x, nextPoint.transform.position.y, pointer.transform.position.z), nextPoint.transform.position) < velocity/2) {
			nextPoint = futurePoint;
			futurePoint = Point("RacingPoint").GetComponent<Point>().points[0];
			eyes.SetDestination(nextPoint.transform.position);
		}

		for (int i = 0; i < frontWheels.Length; i++) {
			float steer = eyes.transform.localEulerAngles.y;
			if (reverse) {
				if (steer < 360 && steer > 360-steerAngle) {
					frontWheels[i].steerAngle = steerAngle;
				}else if (steer < 360-steerAngle && steer > 180) {
					frontWheels[i].steerAngle = steerAngle;
				}else if (steer < -steerAngle && steer > -180) {
					frontWheels[i].steerAngle = steerAngle;
				}
				if (steer > 0 && steer < steerAngle) {
					frontWheels[i].steerAngle = -steerAngle;
				}else if (steer > steerAngle && steer < 180) {
					frontWheels[i].steerAngle = -steerAngle;
				}
			} else {
				if (steer < 360 && steer > 360-steerAngle) {
					frontWheels[i].steerAngle = steer;
				}else if (steer < 360-steerAngle && steer > 180) {
					frontWheels[i].steerAngle = -steerAngle;
				}else if (steer < -steerAngle && steer > -180) {
					frontWheels[i].steerAngle = -steerAngle;
				}
				if (steer > 0 && steer < steerAngle) {
					frontWheels[i].steerAngle = steer;
				}else if (steer > steerAngle && steer < 180) {
					frontWheels[i].steerAngle = steerAngle;
				}
			}
		}
		for (int i = 0; i < backWheels.Length; i++) {
			velocity = transform.InverseTransformDirection(rigid.velocity).z;
			float power = 0, brake = 0;
			if (velocity > 5) {
				if (frontWheels[i].steerAngle > 0 && frontWheels[i].steerAngle < (steerAngle/2) || 
					frontWheels[i].steerAngle < 360 && frontWheels[i].steerAngle > 360-(steerAngle/2)) {
					if (power < maxPower) {
						power = (Vector3.Distance(transform.position, nextPoint.transform.position)*(maxPower/50));
					}else {
						power = maxPower;
					}
					brake = maxBrakeTorque/(Vector3.Distance(transform.position, nextPoint.transform.position));
				}else {
					if (power < (maxPower/2)) {
						power = (Vector3.Distance(transform.position, nextPoint.transform.position));
					}else {
						power = (maxPower/2);
					}
					brake = (maxBrakeTorque*2)/(Vector3.Distance(transform.position, nextPoint.transform.position));
				}
			}else {
				power = maxPower;
				brake = 0;
			}
			/*if (eyes.transform.localEulerAngles.y >= 0 && eyes.transform.localEulerAngles.y <= steerAngle) {
				//power = 800/(frontWheels[i].steerAngle);
				brake = frontWheels[i].steerAngle*(velocity*2);
			}else if (eyes.transform.localEulerAngles.y <= 360 && eyes.transform.localEulerAngles.y >= 360-steerAngle) {
				//power = 800/(Mathf.Abs(360-frontWheels[i].steerAngle));
				brake = Mathf.Abs(360-frontWheels[i].steerAngle)*(velocity*2);
			}*/
			//print (brake);
			if (reverse) {
				if (velocity < (speedLimit/4)) {
					backWheels[i].motorTorque = power;
					backWheels[i].motorTorque = 0;
				}
				if (fix) {
					StartCoroutine (StopReverse());
				}
			} else {
				if (velocity < speedLimit) {
					RaycastHit hit;
					if (Physics.SphereCast(new Ray(pointer.transform.position, pointer.transform.forward), 0.8f, out hit, (2+(velocity/2)), vehicleLayerMask)) {
						if (velocity > 0) {
							backWheels[i].motorTorque = 0;
							frontWheels[i].brakeTorque = (maxBrakeTorque/2);
						}else {
							backWheels[i].motorTorque = power;
							frontWheels[i].brakeTorque = brake;
						}
					}else {
						backWheels[i].motorTorque = power;
						frontWheels[i].brakeTorque = brake;
					}
				}else {
					backWheels[i].motorTorque = 0;
					frontWheels[i].brakeTorque = maxBrakeTorque;
				}
			}
		}

		if (velocity < 0.3f && fix) {
			StartCoroutine (CheckShouldReverse());
		}else if (velocity > 0.3f) {
			StopAllCoroutines ();
			fix = true;
		}
        
		eyes.transform.position = pointer.transform.position;
		eyes.transform.localEulerAngles -= (rigid.angularVelocity*2);
		pointer.transform.LookAt(nextPoint.transform.position);
		speedLimit = nextPoint.GetComponent<Point>().speed;
	}

	void OnGUI () {
		float velocity = transform.InverseTransformDirection(rigid.velocity).z;
		GUI.Label (new Rect(0, 0, 100, 50), maxPower.ToString());
		GUI.Label (new Rect(0, 50, 100, 50), velocity.ToString());
	}

	IEnumerator CheckShouldReverse () {
		fix = false;
		yield return new WaitForSeconds (reverseWaitTime);
		reverse = true;
		fix = true;
	}

	IEnumerator StopReverse () {
		fix = false;
		yield return new WaitForSeconds (reverseTime);
		reverse = false;
		fix = true;
	}

	public GameObject Point (string tag)
	{
		//this took me half a day remember, its about finding the closest object
		GameObject[] allCars;
		GameObject nearestObject = null;
		float distance = Mathf.Infinity;
		float closestObject = Mathf.Infinity;
		bool distanceFix = true;
		allCars = GameObject.FindGameObjectsWithTag(tag);
		if (GameObject.FindGameObjectWithTag(tag))
		{
			foreach (GameObject gObject in allCars)
			{
				distance = Vector3.Distance(gObject.transform.position, transform.position);
				if(distanceFix)
				{
					closestObject = distance;
					distanceFix = false;
				}
				if (distance < closestObject)
				{
					closestObject = distance;
				}
				if (Vector3.Distance(gObject.transform.position, transform.position) <= closestObject + 1)
				{
					nearestObject = gObject;
					//closestCar = gObject;

				}
			}
		}
		return nearestObject;
	}
}
