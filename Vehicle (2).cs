using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

	public WheelCollider FL, FR, RL, RR;
	public VehicleController vehicle_controller;
	public DriveBias drive_bias;
	public SteeringBias steering_bias;
	public float power, brake_force, steer_angle;
	public float checkpoint_changing_distance, checkpoint_stopping_distance, vehicle_stopping_distance;
	public GameObject steering_aim, steering_aim2;
	public MeshRenderer body_material;

	//public float brake;

	private GameObject next_checkpoint, previous_checkpoint, future_checkpoint;
	private Material body;
	private Vector3 rot;
	private Rigidbody rigid;
	private bool stop, no_vehicle, reverse, fix = true;
	private RaycastHit hit, hit3;

	public enum VehicleController {
		AITraffic,
		AIRacing,
		Player
	}

	public enum DriveBias {
		FourWheelDrive,
		FrontWheelDrive,
		RearWheelDrive
	}

	public enum SteeringBias {
		AllWheels,
		FrontWheels,
		BackWheels
	}

	void Start ()
	{
		next_checkpoint = Point(true, "VCP");
		rigid = this.GetComponent<Rigidbody>();
		//body = new Material(body_shader);
		//body.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		for (int i = 0; i < body_material.materials.Length; i++)
		{
			if (body_material.materials[i].color == Color.white)
			{
				body_material.materials[i].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			}
			break;
		}
		//body_material.materials[0].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}

	void Update ()
	{
		//brake = RR.brakeTorque;
		steering_aim2.transform.LookAt(next_checkpoint.transform, Vector3.up);
		steering_aim2.transform.position = new Vector3(transform.position.x, next_checkpoint.transform.position.y, transform.position.z);
		switch (vehicle_controller) {
		case VehicleController.AITraffic:
			float zvelocity = transform.InverseTransformDirection(rigid.velocity).z;

			checkpoint_changing_distance = (zvelocity / 2) + 0.5f;
			checkpoint_stopping_distance = (zvelocity * 2) + 1.5f;
			vehicle_stopping_distance = (zvelocity * 1.5f) + 1.5f;
			if (Vector3.Distance(steering_aim.transform.position, next_checkpoint.transform.position) < checkpoint_changing_distance)
			{
				previous_checkpoint = next_checkpoint;
				if (future_checkpoint == null)
				{
					next_checkpoint = next_checkpoint.GetComponent<VehiclePoints>().connected_points[Random.Range(0, next_checkpoint.GetComponent<VehiclePoints>().connected_points.Count)];
					future_checkpoint = next_checkpoint.GetComponent<VehiclePoints>().connected_points[Random.Range(0, next_checkpoint.GetComponent<VehiclePoints>().connected_points.Count)];
				}else {
					next_checkpoint = future_checkpoint;
					future_checkpoint = next_checkpoint.GetComponent<VehiclePoints>().connected_points[Random.Range(0, next_checkpoint.GetComponent<VehiclePoints>().connected_points.Count)];
				}
				previous_checkpoint.GetComponent<VehiclePoints>().vehicle.Remove(gameObject);
				if (!next_checkpoint.GetComponent<VehiclePoints>().vehicle.Contains(gameObject))
				{
					next_checkpoint.GetComponent<VehiclePoints>().vehicle.Add(gameObject);
				}
			}
			if (Physics.SphereCast(steering_aim.transform.position - transform.forward, 1.4f, steering_aim.transform.forward, out hit, vehicle_stopping_distance))
			{
				if (hit.transform.gameObject.tag == "Vehicle")
				{
					stop = true;
					no_vehicle = false;
				}else if (hit.transform.gameObject.tag == "Obsticle")
				{
					stop = true;
					no_vehicle = false;
				}else {
					if (previous_checkpoint.transform.tag == "VCPWR")
					{
						SphereCastingCheckpointDetection (hit3);
					}else{// tag check
						stop = false;
						no_vehicle = true;

					}
				}
			}else if (previous_checkpoint != null)// main sphere cast
			{
				if (previous_checkpoint.transform.tag == "VCPWR")
				{
					SphereCastingCheckpointDetection (hit3);
				}else{
					stop = false;
					no_vehicle = true;

				}
			}else{
				stop = false;
				no_vehicle = true;

			}
			if (no_vehicle)
			{
				if (next_checkpoint.transform.tag == "VCPWR")
				{
					if (next_checkpoint.GetComponent<VehiclePoints>().axis == VehiclePoints.Axis.X)
					{
						if (next_checkpoint.transform.parent.GetComponent<TrafficPointsParent>().robotX != TrafficPointsParent.Robot.Go)
						{
							if (Vector3.Distance(steering_aim.transform.position, next_checkpoint.transform.position) < checkpoint_stopping_distance)
							{
								stop = true;
							}else {
								stop = false;
							}
						}else {
								stop = false;
						}
					}else if (next_checkpoint.GetComponent<VehiclePoints>().axis == VehiclePoints.Axis.Z)
					{
						if (next_checkpoint.transform.parent.GetComponent<TrafficPointsParent>().robotZ != TrafficPointsParent.Robot.Go)
						{
							if (Vector3.Distance(steering_aim.transform.position, next_checkpoint.transform.position) < checkpoint_stopping_distance)
							{
								stop = true;
							}else {
								stop = false;
							}
						}else {
								stop = false;
						}
					}
				}else {
					stop = false;
				}
			}
			switch (drive_bias) {
			case DriveBias.RearWheelDrive:
				if (zvelocity < next_checkpoint.GetComponent<VehiclePoints>().speed / 15)
				{
					if (stop)
					{
						RL.motorTorque = 0;
						RR.motorTorque = 0;
						RL.brakeTorque = brake_force;
						RR.brakeTorque = brake_force;
					}else {
						if (reverse)
						{
							RL.motorTorque = -power;
							RR.motorTorque = -power;
							RL.brakeTorque = 0;
							RR.brakeTorque = 0;
						}else {
							RL.motorTorque = power;
							RR.motorTorque = power;
							RL.brakeTorque = 0;
							RR.brakeTorque = 0;
						}
					}
				}else
				{
					RL.motorTorque = 0;
					RR.motorTorque = 0;
					RL.brakeTorque = brake_force;
					RR.brakeTorque = brake_force;
				}
				break;
			}
			switch (steering_bias) {
			case SteeringBias.FrontWheels:
				float steer = steering_aim.transform.localEulerAngles.y;
				if (steering_aim.transform.localEulerAngles.y < 360 && steering_aim.transform.localEulerAngles.y > 360 - steer_angle)
				{
					FR.steerAngle = steer;
					FL.steerAngle = steer;
					//FR.steerAngle = AI.gameObject.transform.localEulerAngles.y;
					//FL.steerAngle = AI.gameObject.transform.localEulerAngles.y;
				}
				if (steering_aim.transform.localEulerAngles.y < steer_angle && steering_aim.transform.localEulerAngles.y > 0)
				{
					FR.steerAngle = steer;
					FL.steerAngle = steer;
				}
				break;
			}
			break;
		case VehicleController.AIRacing://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			zvelocity = transform.InverseTransformDirection(rigid.velocity).z;
			if (zvelocity < 0.5f && fix)
			{
				StartCoroutine(Reverse());
				fix = false;
			}else if (zvelocity >= 0.5f && !fix)
			{
				StopAllCoroutines();
				reverse = false;
				fix = true;
			}
			checkpoint_changing_distance = (zvelocity / 2) + 0.5f;
			checkpoint_stopping_distance = (zvelocity * 2) + 1.5f;
			vehicle_stopping_distance = (zvelocity * 1.5f) + 1.5f;
			if (Vector3.Distance(steering_aim.transform.position, next_checkpoint.transform.position) < checkpoint_changing_distance)
			{
				previous_checkpoint = next_checkpoint;
				if (future_checkpoint == null)
				{
					next_checkpoint = next_checkpoint.GetComponent<VehiclePoints>().connected_points[Random.Range(0, next_checkpoint.GetComponent<VehiclePoints>().connected_points.Count)];
					future_checkpoint = next_checkpoint.GetComponent<VehiclePoints>().connected_points[Random.Range(0, next_checkpoint.GetComponent<VehiclePoints>().connected_points.Count)];
				}else {
					next_checkpoint = future_checkpoint;
					future_checkpoint = next_checkpoint.GetComponent<VehiclePoints>().connected_points[Random.Range(0, next_checkpoint.GetComponent<VehiclePoints>().connected_points.Count)];
				}
				previous_checkpoint.GetComponent<VehiclePoints>().vehicle.Remove(gameObject);
				if (!next_checkpoint.GetComponent<VehiclePoints>().vehicle.Contains(gameObject))
				{
					next_checkpoint.GetComponent<VehiclePoints>().vehicle.Add(gameObject);
				}
			}
			if (Physics.SphereCast(steering_aim.transform.position - transform.forward, 1.4f, steering_aim.transform.forward, out hit, vehicle_stopping_distance))
			{
				float dodge_speed = 10f;
				if (hit.transform.gameObject.tag == "Vehicle")
				{
					if (hit.transform.position.x > transform.InverseTransformPoint(transform.position).x)
					{
						steering_aim2.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + dodge_speed, transform.localEulerAngles.z);
					}else if (hit.transform.position.x < transform.InverseTransformPoint(transform.position).x)
					{
						steering_aim2.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - dodge_speed, transform.localEulerAngles.z);
					}
					stop = true;
					no_vehicle = false;
				}else if (hit.transform.gameObject.tag == "Obsticle")
				{
					/*if (hit.transform.InverseTransformPoint(transform.position).x > transform.position.x)
					{
						steering_aim2.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + dodge_speed, transform.localEulerAngles.z);
					}else if (hit.transform.InverseTransformPoint(transform.position).x < transform.position.x)
					{
						steering_aim2.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - dodge_speed, transform.localEulerAngles.z);
					}*/if (hit.transform.TransformPoint(transform.position).x > transform.position.x)
					{
						steering_aim2.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + dodge_speed, transform.localEulerAngles.z);
					}else if (hit.transform.TransformPoint(transform.position).x < transform.position.x)
					{
						steering_aim2.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - dodge_speed, transform.localEulerAngles.z);
					}
					stop = true;
					no_vehicle = false;
				}else {
					if (previous_checkpoint.transform.tag == "VCPWR")
					{
						SphereCastingCheckpointDetection (hit3);
					}else{// tag check
						stop = false;
						no_vehicle = true;

					}
				}
			}else if (previous_checkpoint != null)// main sphere cast
			{
				if (previous_checkpoint.transform.tag == "VCPWR")
				{
					SphereCastingCheckpointDetection (hit3);
				}else{
					stop = false;
					no_vehicle = true;

				}
			}else{
				stop = false;
				no_vehicle = true;

			}
			if (no_vehicle)
			{
				if (next_checkpoint.transform.tag == "VCPWR")
				{
					if (next_checkpoint.GetComponent<VehiclePoints>().axis == VehiclePoints.Axis.X)
					{
						if (next_checkpoint.transform.parent.GetComponent<TrafficPointsParent>().robotX != TrafficPointsParent.Robot.Go)
						{
							if (Vector3.Distance(steering_aim.transform.position, next_checkpoint.transform.position) < checkpoint_stopping_distance)
							{
								stop = true;
							}else {
								stop = false;
							}
						}else {
							stop = false;
						}
					}else if (next_checkpoint.GetComponent<VehiclePoints>().axis == VehiclePoints.Axis.Z)
					{
						if (next_checkpoint.transform.parent.GetComponent<TrafficPointsParent>().robotZ != TrafficPointsParent.Robot.Go)
						{
							if (Vector3.Distance(steering_aim.transform.position, next_checkpoint.transform.position) < checkpoint_stopping_distance)
							{
								stop = true;
							}else {
								stop = false;
							}
						}else {
							stop = false;
						}
					}
				}else {
					stop = false;
				}
			}
			switch (drive_bias) {
			case DriveBias.RearWheelDrive:
				if (zvelocity < next_checkpoint.GetComponent<VehiclePoints>().speed / 15)
				{
					//if (stop)
					//{
					//RL.motorTorque = power;
					//RR.motorTorque = power;
					//RL.brakeTorque = 0;
					//RR.brakeTorque = 0;
					//RL.motorTorque = 0;
					//RR.motorTorque = 0;
					//RL.brakeTorque = brake_force;
					//RR.brakeTorque = brake_force;
					//}else {
					if (reverse)
					{
						RL.motorTorque = -power;
						RR.motorTorque = -power;
						RL.brakeTorque = 0;
						RR.brakeTorque = 0;
					}else {
						RL.motorTorque = power;
						RR.motorTorque = power;
						RL.brakeTorque = 0;
						RR.brakeTorque = 0;
					}
					//}
				}else
				{
					RL.motorTorque = 0;
					RR.motorTorque = 0;
					RL.brakeTorque = brake_force;
					RR.brakeTorque = brake_force;
				}
				break;
			}
			switch (steering_bias) {
			case SteeringBias.FrontWheels:
				float steer = steering_aim.transform.localEulerAngles.y;
				if (reverse	)
				{
					if (steering_aim.transform.localEulerAngles.y < 360 && steering_aim.transform.localEulerAngles.y > 360 - steer_angle)
					{
						FR.steerAngle = -steer;
						FL.steerAngle = -steer;
						//FR.steerAngle = AI.gameObject.transform.localEulerAngles.y;
						//FL.steerAngle = AI.gameObject.transform.localEulerAngles.y;
					}
					if (steering_aim.transform.localEulerAngles.y < steer_angle && steering_aim.transform.localEulerAngles.y > 0)
					{
						FR.steerAngle = -steer;
						FL.steerAngle = -steer;
					}
				}else
				{
					if (steering_aim.transform.localEulerAngles.y < 360 && steering_aim.transform.localEulerAngles.y > 360 - steer_angle)
					{
						FR.steerAngle = steer;
						FL.steerAngle = steer;
						//FR.steerAngle = AI.gameObject.transform.localEulerAngles.y;
						//FL.steerAngle = AI.gameObject.transform.localEulerAngles.y;
					}
					if (steering_aim.transform.localEulerAngles.y < steer_angle && steering_aim.transform.localEulerAngles.y > 0)
					{
						FR.steerAngle = steer;
						FL.steerAngle = steer;
					}
				}
				break;
			}
			break;
		}
	}

	void SphereCastingCheckpointDetection (RaycastHit hit2)
	{
		if (Physics.SphereCast(next_checkpoint.transform.position - (next_checkpoint.transform.forward * 2), 1f, next_checkpoint.transform.forward, out hit2, next_checkpoint.GetComponent<VehiclePoints>().ray_lenght))
		{
			if (hit2.transform.gameObject.tag == "Vehicle")
			{
				if (hit2.transform.gameObject == gameObject)
				{
					stop = false;
					no_vehicle = true;
				}else{
					stop = true;
					no_vehicle = false;
				}
			}else{
				stop = false;
				no_vehicle = true;

			}
		}else{
			stop = false;
			no_vehicle = true;

		}
	}

	IEnumerator Reverse ()
	{
		yield return new WaitForSeconds(2f);
		reverse = true;
		yield return new WaitForSeconds(2f);
		reverse = false;
		fix = true;
	}

	public GameObject Point (bool distanceFix, string tag)
	{
		//this took me half a day remember, its about finding the closest object
		GameObject[] allCars;
		GameObject nearestObject = null;
		float distance = Mathf.Infinity;
		float closestObject = Mathf.Infinity;
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

/*if (next_checkpoint.GetComponent<VehiclePoints>().detection_points.Count > 0)
					{
						for (int i = 0; i < next_checkpoint.GetComponent<VehiclePoints>().detection_points.Count; i++)
						{
							if (next_checkpoint.GetComponent<VehiclePoints>().detection_points[i].GetComponent<VehiclePoints>().vehicle.Count > 0)
							{
							}
						}
					}*/