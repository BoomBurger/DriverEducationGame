using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadPoint : MonoBehaviour {

	//This is the actual point the vehicle follow

		//NEXT CLASS!!!!

	public List<RoadPoint> connectedPoints = new List<RoadPoint>();
	public StateAxis robotAxis;
	public PointType pointType;
	public State state;
	public bool go;
	public int speed;

	private RoadRules.PointState roadState;
	private RoadRules roadRules;

	public enum State {
		In,
		Out,
		Middle
	}

	public enum StateAxis {
		None,
		XRobot,
		ZRobot
	}

	public enum PointType {
		None,
		StoppingPoint,
	}

	void Awake () {
		roadRules = GameObject.FindGameObjectWithTag ("RoadRules").GetComponent<RoadRules>();
	}

	void Update () {
		if (transform.parent.localEulerAngles.y == 0 || transform.parent.localEulerAngles.y == 180) {
			if (robotAxis == StateAxis.XRobot) {
				roadState = roadRules.xState;
			} else if (robotAxis == StateAxis.ZRobot) {
				roadState = roadRules.zState;
			}
		} else {
			if (robotAxis == StateAxis.XRobot) {
				roadState = roadRules.zState;
			} else if (robotAxis == StateAxis.ZRobot) {
				roadState = roadRules.xState;
			}
		}
		if (roadState == RoadRules.PointState.Go) {
			go = true;
		} else {
			go = false;
		}
		connectedPoints = connectedPoints.Distinct ().ToList ();
	}

	void OnDrawGizmos () {
		if (pointType == PointType.StoppingPoint) {
			if (roadState == RoadRules.PointState.Go) 
				Gizmos.color = Color.green;
			else if (roadState == RoadRules.PointState.Ready) 
				Gizmos.color = Color.yellow;
			else if (roadState == RoadRules.PointState.Stop) 
				Gizmos.color = Color.red;
			if (connectedPoints.Count > 0) {
				for (int i = 0; i < connectedPoints.Count; i++) {
					if (connectedPoints[i] != null) {
						Gizmos.DrawLine (transform.position, connectedPoints[i].transform.position);
					}
				}
			}
			Gizmos.DrawWireSphere (transform.position, 0.4f);
		} else if (pointType == PointType.None) {
			Gizmos.color = Color.cyan;
			if (connectedPoints.Count > 0) {
				for (int i = 0; i < connectedPoints.Count; i++) {
					if (connectedPoints[i] != null) {
						Gizmos.DrawLine (transform.position, connectedPoints[i].transform.position);
					}
				}
			}
			Gizmos.DrawWireSphere (transform.position, 0.4f);
		}
	}
}
