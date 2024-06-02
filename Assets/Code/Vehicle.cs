using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

	//Holy shit, this is a big one. uh

		//take your time... i am NOT commenting this one, sorry... my life is really precious right now... at this moment

		//I will say this... this is the class attached to a vehicle on the road
	
	[Header("Wheels")]
	public WheelCollider[] frontWheels;
	public WheelCollider[] backWheels;
	public DriveBias driveBias;
	public SteerBias steerBias;

	[Header("Misc")]
	public GameObject steeringAim, steeringAim2;
	public MeshRenderer body;
	public Object wheel;
	[Range(20, 45)]public int steerAngle;
	public int power, brakeTorque, bodyMaterialIndex, randomID;
	public bool reverse;

	private RoadStoppingManager stoppingParent;
	private RoadPoint previousTarget, currentTarget, futureTarget, stoppingPoint, turningPoint;
	private Rigidbody rigid;
	private GameObject tempWheel;
	private Ray ray;
	private RaycastHit hit;
	private float zVelocity;
	public bool stop;

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

	void Awake () {
		body.materials [bodyMaterialIndex] = new Material (body.materials [bodyMaterialIndex]);
		body.materials [bodyMaterialIndex].color = new Color(Random.Range (0.4f, 0.8f), Random.Range (0.4f, 0.8f), Random.Range (0.4f, 0.8f));

		randomID = Random.Range (10000, 99999);
		rigid = GetComponent<Rigidbody> ();
		currentTarget = Tricks.GetClosestObjectWithTag(gameObject, "RoadPoint");
		if (currentTarget != null) {
			previousTarget = currentTarget;
			if (currentTarget.connectedPoints.Count > 0) {
				futureTarget = currentTarget.connectedPoints [Random.Range (0, currentTarget.connectedPoints.Count)];
			} else {
				futureTarget = null;
			}
		}
	}

	void Start () {
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
	}

	void Update () {
		zVelocity = transform.InverseTransformDirection (rigid.velocity).z;
		if (currentTarget != null) {
			if (Vector3.Distance (steeringAim.transform.position, currentTarget.transform.position) < 0.5f + (zVelocity / 6)) {
				//print (target.connectedPoints[0]);
				if (currentTarget.connectedPoints.Count > 0) {
					previousTarget = currentTarget;
					currentTarget = futureTarget;
					if (currentTarget != null) {
						if (currentTarget.connectedPoints.Count > 0) {
							futureTarget = currentTarget.connectedPoints [Random.Range (0, currentTarget.connectedPoints.Count)];
						} else {
							futureTarget = null;
						}
					}
				}
			}//else if (Vector3.Distance (steeringAim.transform.position, currentTarget.transform.position) < 5) {
				//steeringAim.transform.InverseTransformPoint(currentTarget.transform.position)
			if (Vector3.Distance (transform.position, currentTarget.transform.position) < Vector3.Distance (steeringAim.transform.position, transform.position)) {
				if (currentTarget.connectedPoints.Count > 0) {
					previousTarget = currentTarget;
					currentTarget = futureTarget;
					if (currentTarget != null) {
						if (currentTarget.connectedPoints.Count > 0) {
							futureTarget = currentTarget.connectedPoints [Random.Range (0, currentTarget.connectedPoints.Count)];
						} else {
							futureTarget = null;
						}
					}
				}
			}
			if (currentTarget.pointType == RoadPoint.PointType.StoppingPoint) {
				RoadObject stopper = currentTarget.transform.parent.GetComponent<RoadObject>();
				if (stoppingParent == null) {
					stoppingParent = currentTarget.transform.parent.GetComponent<RoadStoppingManager> ();
				}
				if (stoppingPoint == null) {
					stoppingPoint = currentTarget;
				}
			}
			steeringAim2.transform.LookAt (currentTarget.transform.position);
			Vector3 pos = steeringAim.transform.position;
			pos.y = currentTarget.transform.position.y;
			steeringAim.transform.position = pos;
			Vector3 rot = steeringAim2.transform.localEulerAngles;
			rot.x = 0;
			steeringAim2.transform.localEulerAngles = rot;
		} else {
			currentTarget = Tricks.GetClosestObjectWithTag(gameObject, "RoadPoint");
			if (currentTarget != null) {
				if (currentTarget.connectedPoints.Count > 0) {
					futureTarget = currentTarget.connectedPoints [Random.Range (0, currentTarget.connectedPoints.Count)];
				} else {
					futureTarget = null;
				}
			}
			//Debug.LogError ("No Target Found: Car " + randomID);
		}
		if (stoppingPoint != null) {
			stop = StoppingPointManager ();
			if (Vector3.Distance (steeringAim.transform.position, stoppingPoint.transform.position) > 8 && Vector3.Distance (steeringAim.transform.position, stoppingPoint.transform.position) > 3) {
				stoppingParent = null;
				stoppingPoint = null;
			}
		} else if (stoppingPoint == null) {
			stop = CheckForVehiclesInFront ();
		}
		if (currentTarget == null) {
			stop = true;
		}
		for (int i = 0; i < frontWheels.Length; i++) {
			float steer = steeringAim.transform.localEulerAngles.y;
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
				if (steer <= 360 && steer >= 360 - steerAngle) {
					frontWheels [i].steerAngle = steer;
				} else if (steer <= 360 - steerAngle && steer >= 180) {
					frontWheels [i].steerAngle = -steerAngle;
				} else if (steer <= -steerAngle && steer >= -180) {
					frontWheels [i].steerAngle = -steerAngle;
				} else if (steer >= 0 && steer <= steerAngle) {
					frontWheels[i].steerAngle = steer;
				}else if (steer >= steerAngle && steer <= 180) {
					frontWheels[i].steerAngle = steerAngle;
				} else {
					frontWheels [i].steerAngle = steer;
				}
			}
		}
		for (int i = 0; i < backWheels.Length; i++) {
			int currentSpeed = 0;
			if (currentTarget == null) {
				currentSpeed = 4;
			} else {
				currentSpeed = currentTarget.speed;
			}
			if (backWheels[i].rpm < (currentSpeed*30) && stop == false) {
				backWheels [i].motorTorque = power;
				backWheels [i].brakeTorque = 0;
			} else {
				backWheels [i].motorTorque = 0;
				backWheels [i].brakeTorque = brakeTorque;
			}
		}
	}

	bool StoppingPointManager () {
		if (!stoppingPoint.go && Vector3.Distance (steeringAim.transform.position, stoppingPoint.transform.position) < 2) {
			if (steeringAim.transform.InverseTransformPoint (stoppingPoint.transform.position).normalized.z > 0.2f) {
				if (Vector3.Distance (steeringAim.transform.position, stoppingPoint.transform.position) < 0.2f + (zVelocity)) {
					return true;
				} else
					return false;
			} else if (steeringAim.transform.InverseTransformPoint (stoppingPoint.transform.position).normalized.z < 0.2f) {
				return true;
			}
			return true;
		} 
		return false;
	}

	bool CheckForVehiclesInFront () {
		ray = new Ray (steeringAim.transform.position, steeringAim.transform.forward);
		if (Physics.SphereCast (ray, 1f, out hit, 1.5f + (zVelocity * 2))) {
			if (hit.transform.tag == "Vehicle") {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		} 
	}

	void OnDrawGizmos () {
		if (currentTarget != null) {
			Vector3 pos = currentTarget.transform.position;
			Vector3 pos2 = Vector3.zero;
			if (futureTarget != null) {
				pos2 = futureTarget.transform.position;
			}
			pos.y += 0.1f;
			pos2.y += 0.1f;
			Gizmos.color = Color.red;
			Gizmos.DrawLine (steeringAim.transform.position, pos);
			Gizmos.DrawLine (pos, pos2);
			Gizmos.DrawWireSphere (currentTarget.transform.position, 0.5f);
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere (transform.position, Vector3.Distance (transform.position, steeringAim.transform.position));
			Gizmos.color = Color.black;
		}
		if (futureTarget != null) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (futureTarget.transform.position, 0.5f);
		}
		Gizmos.color = Color.green;
		Gizmos.DrawLine (steeringAim.transform.position, steeringAim.transform.position+(steeringAim.transform.forward*(1 + (zVelocity * 2))));
		Gizmos.DrawWireSphere (steeringAim.transform.position+(steeringAim.transform.forward*(1 + (zVelocity * 2))), 1f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (steeringAim.transform.position, 0.3f + (zVelocity / 6));
		if (stoppingPoint != null) {
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere (stoppingPoint.transform.position, 0.2f + (zVelocity/2));
		}
	}
}
