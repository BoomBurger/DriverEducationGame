using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour {

	//Manages roady stuff, haha

	public Object[] roadObject, Vehicles;
	public GameObject grid, cameraController;
	public LayerMask layerMask, layerMaskRemover;

	private ToolbarState toolbarState;
	private Camera camera;
	private Ray ray;
	private RaycastHit hit;
	private RoadObject tempObject;
	private GameObject roadParent, tempVehicle, tempFollowObject;
	private string[] toolbarStrings = {"None", "Straight", "Corner", "Intersection", "T Intersection", "VehiclePlacer", "Remover"};
	private float yRot;
	private bool mouseOnGUI;
	private int randomVehicle, toolbarSelector;

	public enum ToolbarState {
		None = 0,
		Straight = 1,
		Corner = 2,
		Intersecting = 3,
		TIntersecting = 4,
		VehiclePlacer = 5,	
		Remover = 6
	}

	void Start () {
		camera = GetComponent<Camera> ();
		roadParent = GameObject.FindGameObjectWithTag ("RoadParent");
	}

	void LateUpdate () {
		if (tempFollowObject != null) {
			cameraController.transform.position = tempFollowObject.transform.position;
		}
	}

	//The Update function updates... oh... nevermind... this is where you choose what to place, road related mumbo jumbo (i hope i said that correctly)
	void Update () {
		yRot += (Input.GetKeyDown(KeyCode.Period))?90:(Input.GetKeyDown(KeyCode.Comma))?-90:0;
		yRot = Mathf.Round (yRot/90)*90;

		ray = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast (ray, out hit, 400f)) {
			if (toolbarState == ToolbarState.None) {
				if (Input.GetKeyDown(KeyCode.Mouse0)) {
					if (hit.transform.tag == "Vehicle") {
						tempFollowObject = hit.transform.gameObject;
					}
				}
			}
		}

		if (toolbarState != ToolbarState.None) {
			tempFollowObject = null;
		}

		if (Physics.Raycast (ray, out hit, 400f, layerMask)) {
			Vector3 pos = hit.point;
			if (toolbarState == ToolbarState.VehiclePlacer) {
				pos.y = pos.y+3.2f;
			} else {
				pos.x = Mathf.Round (pos.x / 16) * 16;
				pos.z = Mathf.Round (pos.z / 16) * 16;
				pos.y = pos.y+0.1f;
			}
			toolbarState = (ToolbarState)toolbarSelector;
			switch (toolbarState) {
			case ToolbarState.None:
				CheckToDestroy (tempObject);
				tempObject = null;
				break;
			case ToolbarState.Straight:
				CheckToDestroy (tempObject);
				tempObject = (Instantiate (roadObject [0], pos, Quaternion.Euler (new Vector3 (0, yRot)), roadParent.transform) as GameObject).GetComponent<RoadObject>();
				break;
			case ToolbarState.Corner:
				CheckToDestroy (tempObject);
				tempObject = (Instantiate (roadObject [1], pos, Quaternion.Euler (new Vector3 (0, yRot)), roadParent.transform) as GameObject).GetComponent<RoadObject>();
				break;
			case ToolbarState.Intersecting:
				CheckToDestroy (tempObject);
				tempObject = (Instantiate (roadObject [2], pos, Quaternion.Euler (new Vector3 (0, yRot)), roadParent.transform) as GameObject).GetComponent<RoadObject>();
				break;
			case ToolbarState.TIntersecting:
				CheckToDestroy (tempObject);
				tempObject = (Instantiate (roadObject [3], pos, Quaternion.Euler (new Vector3 (0, yRot)), roadParent.transform) as GameObject).GetComponent<RoadObject>();
				break;
			case ToolbarState.VehiclePlacer:
				CheckToDestroy (tempObject);
				CheckToDestroy (tempVehicle);
				randomVehicle = Random.Range (0, Vehicles.Length);
				tempVehicle = Instantiate (Vehicles [randomVehicle], pos, Quaternion.Euler (new Vector3 (0, yRot))) as GameObject;
				break;
			}
			if (tempObject != null) {
				tempObject.transform.position = pos;
			}
			if (!mouseOnGUI && toolbarState != ToolbarState.Remover && toolbarState != ToolbarState.VehiclePlacer) {
				if (Input.GetKey (KeyCode.Mouse0)) {
					if (CheckForPreExistingRoad (tempObject.transform.position)) {
						tempObject.tag = "Road";
						tempObject.UpdateRoad (false);//this updates the roads connection to the one next to it
						tempObject = null;
					}
				}
			} else if (!mouseOnGUI && toolbarState == ToolbarState.VehiclePlacer) {
				if (Input.GetKeyDown (KeyCode.Mouse0)) {
					tempVehicle.transform.position = new Vector3(pos.x, pos.y - 2f, pos.z);
					tempVehicle = null;
				}
			}
			if (toolbarState == ToolbarState.None) {
				grid.SetActive (false);
			} else {
				grid.SetActive (true);
			}
			if (toolbarState != ToolbarState.VehiclePlacer) {
				CheckToDestroy (tempVehicle);
			}
		} else {
			CheckToDestroy (tempObject);
		}

		//Here we are removing roads
		if (Physics.Raycast (ray, out hit, 400f, layerMaskRemover)) {
			switch (toolbarState) {
			case ToolbarState.Remover:
				CheckToDestroy (tempObject);
				tempObject = null;
				if (!mouseOnGUI) {
					if (hit.transform.tag == "Road") {
						if (Input.GetKeyDown(KeyCode.Mouse0)) {
							hit.transform.GetComponent<RoadObject>().UpdateRoad (true);
							Destroy (hit.transform.gameObject);
						}
					}else if (hit.transform.tag == "Vehicle") {
						if (Input.GetKeyDown(KeyCode.Mouse0)) {
							Destroy (hit.transform.gameObject);
						}
					}
				}
				break;
			}
		}
	}

	//WTF!!!, coudn't i just have stored the roads in a dictionary, or with the limited world, a 3D array
	bool CheckForPreExistingRoad (Vector3 pos) {
		GameObject[] roads = GameObject.FindGameObjectsWithTag ("Road");
		for (int i = 0; i < roads.Length; i++) {
			if (Vector3.Distance(roads[i].transform.position, pos) < 2) {
				return false;
			}
		}
		return true;
	}

	//classic me, always commenting instead of removing, i am always afraid of needing the same code again, or having to revert back to it
	void OnGUI () {
		/*if (GUI.Button (new Rect (0, Screen.height - 40, Screen.width / 6, 40), "None"))
			roadPlacer = RoadPlacer.None;
		else if (GUI.Button (new Rect (Screen.width / 6, Screen.height - 40, Screen.width / 6, 40), "Straight"))
			roadPlacer = RoadPlacer.Straight;
		else if (GUI.Button (new Rect (Screen.width / 6 * 2, Screen.height - 40, Screen.width / 6, 40), "Corner"))
			roadPlacer = RoadPlacer.Corner;
		else if (GUI.Button (new Rect (Screen.width / 6 * 3, Screen.height - 40, Screen.width / 6, 40), "Intersecting"))
			roadPlacer = RoadPlacer.Intersecting;
		else if (GUI.Button (new Rect (Screen.width / 6 * 4, Screen.height - 40, Screen.width / 6, 40), "TIntersecting"))
			roadPlacer = RoadPlacer.TIntersecting;
		else if (GUI.Button (new Rect (Screen.width / 6 * 5, Screen.height - 40, Screen.width / 6, 40), "Remover"))
			roadPlacer = RoadPlacer.Remover;
		else if (GUI.Button (new Rect (0, Screen.height - 60, Screen.width, 20), "Spawn Vehicle")) {
			roadPlacer = RoadPlacer.VehiclePlacer;
		}*/
		toolbarSelector = GUI.Toolbar (new Rect(0, Screen.height-40, Screen.width, 40), toolbarSelector, toolbarStrings);
		GUI.Box (new Rect(Screen.width-400, 0, 400, 25), "Press 'R' to Reset Intersection Robots");
		GUI.Box (new Rect(Screen.width-400, 25, 400, 25), "When None selected, Click on a car to follow it");
		GUI.Box (new Rect (Screen.width - 400, 50, 400, 25), "Use '<' and '>' keys to rotate objects");

		if (GUI.Button(new Rect(240, 0, 120, 40), "Disable Follow Car")) {
			tempFollowObject = null;
		}

		Rect gui = new Rect(0, Screen.height-40, Screen.width, 40);

		Rect gui2 = new Rect (0, 0, 200, 140);

		if (gui.Contains (Event.current.mousePosition) || gui2.Contains (Event.current.mousePosition)) {
			mouseOnGUI = true;
		} else {
			mouseOnGUI = false;
		}
	}

	//Please do not ask waht this is about, please
	void CheckToDestroy (RoadObject objects) {
		if (objects != null) {
			Destroy (objects.gameObject);
		}
	}

	void CheckToDestroy (GameObject objects) {
		if (objects != null) {
			Destroy (objects.gameObject);
		}
	}
}
