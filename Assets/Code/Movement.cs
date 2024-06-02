using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	//moving the ass of the player, although... there is none

	public float maxSpeed;
	public int acceleration, brakeForce, jumpForce;

	private WheelCollider legs;
	private Rigidbody rigid;

	void Start () {
		legs = GetComponentInChildren<WheelCollider>();
		rigid = GetComponent<Rigidbody>();
	}

	void Update () {
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
			if (rigid.velocity.magnitude < maxSpeed && legs.isGrounded) {
				legs.motorTorque = acceleration;
				legs.brakeTorque = 0;
			}else if (legs.rpm < maxSpeed && !legs.isGrounded) {
				legs.motorTorque = acceleration;
				legs.brakeTorque = 0;
			}else {
				legs.motorTorque = 0;
				legs.brakeTorque = brakeForce;
			}
		}else {
			legs.motorTorque = 0;
			legs.brakeTorque = brakeForce;
		}

		if (legs.isGrounded) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
			}
		}

		legs.steerAngle = (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))?315:(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))?45:
			(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))?225:(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))?135:
			(Input.GetKey(KeyCode.W))?0:(Input.GetKey(KeyCode.S))?180:(Input.GetKey(KeyCode.A))?270:(Input.GetKey(KeyCode.D))?90:0;
	}
}
