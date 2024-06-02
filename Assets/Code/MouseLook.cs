using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	//mouselooking... blah blah blah... wait... no player... WTF??????, this class is useless... or is it?

	public CamType camType;
	[Range(0, 6)]
	public int sensitivity;

	private GameObject camera;
	private bool mouseActive = false;
	private float xInput, yInput;
	private string GameController = "GameController", mouseX = "Mouse X", mouseY = "Mouse Y";
	//private GameManager gameManager;

	public enum CamType {
		Character
	}

	//GOD, more commenting, hope no one is allergic
	void Start () {
		camera = GetComponentInChildren<Camera>().gameObject;
		//gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}

	void Update () {
		if (camera == null) {
			camera = GetComponentInChildren<Camera>().gameObject;
		}

		xInput += Input.GetAxis(mouseX) * Time.deltaTime * (sensitivity * 50);
		yInput += -Input.GetAxis(mouseY) * Time.deltaTime * (sensitivity * 50);
		yInput = Mathf.Clamp(yInput, -90, 90);

		switch (camType) {
		case CamType.Character:
			transform.localEulerAngles = new Vector3(0, xInput);
			camera.transform.localEulerAngles = new Vector3(yInput, 0);
			break;
		}

		if (GameObject.FindGameObjectWithTag (GameController))
			//mouseActive = gameManager.paused;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = mouseActive;
	}

	void OnGUI () {
		GUI.Box(new Rect(Screen.width/2-2, Screen.height/4*3-25, 4, 4), "");
	}
}
