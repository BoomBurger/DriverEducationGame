using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour {

	//This class is on the road object
	
	public RoadType roadType;
	public RoadPoint[] roadPoints;

	public enum RoadType {
		Straight,
		Corner,
		Intersection,
		TIntersection
	}

	//Connecting the roads to its neighbors, this is getting REALLY boring now
	public void UpdateRoad (bool remove) {
		roadPoints = GetComponentsInChildren<RoadPoint>();
		List<RoadObject> roadObject = new List<RoadObject> ();
		switch (roadType) {
		case RoadType.Straight:
			roadObject.Add(CheckForNeighborRoad (transform.forward));
			roadObject.Add(CheckForNeighborRoad (-transform.forward));
			break;
		case RoadType.Corner:
			roadObject.Add(CheckForNeighborRoad (transform.forward));
			roadObject.Add(CheckForNeighborRoad (-transform.right));
			break;
		case RoadType.Intersection:
			roadObject.Add(CheckForNeighborRoad (transform.forward));
			roadObject.Add(CheckForNeighborRoad (-transform.forward));
			roadObject.Add(CheckForNeighborRoad (-transform.right));
			roadObject.Add(CheckForNeighborRoad (transform.right));
			break;
		case RoadType.TIntersection:
			roadObject.Add(CheckForNeighborRoad (transform.forward));
			roadObject.Add(CheckForNeighborRoad (-transform.right));
			roadObject.Add(CheckForNeighborRoad (transform.right));
			break;
		}
		List<RoadPoint> plusConnection = new List<RoadPoint> (), minusConnection = new List<RoadPoint> ();
		for (int k = 0; k < roadObject.Count; k++) {
			if (roadObject[k] != null) {

				RoadPoint neighborPoint = GetClosestRoadPoint (roadObject[k].roadPoints, gameObject, RoadPoint.State.Out), 
						point = GetClosestRoadPoint (roadPoints, roadObject[k].gameObject, RoadPoint.State.In);

				if (remove) neighborPoint.connectedPoints.Remove(point);
				else neighborPoint.connectedPoints.Add(point);

				neighborPoint = GetClosestRoadPoint (roadObject [k].roadPoints, gameObject, RoadPoint.State.In); 
				point = GetClosestRoadPoint (roadPoints, roadObject[k].gameObject, RoadPoint.State.Out);

				if (remove) point.connectedPoints.Remove (neighborPoint);
				else point.connectedPoints.Add (neighborPoint);

			}
			plusConnection.Clear();
			minusConnection.Clear();
		}
	}

	//I guess the method name makes sense here
	RoadObject CheckForNeighborRoad (Vector3 side) {
		Vector3 pos = transform.position + (side * 16);
		GameObject[] roadObjects = GameObject.FindGameObjectsWithTag ("Road");
		for (int i = 0; i < roadObjects.Length; i++) {
			if (Vector3.Distance(roadObjects[i].transform.position, pos) < 2) {
				return roadObjects [i].GetComponent<RoadObject>();
			}
		}
		return null;
	}

	//The roads have points that the vehicles follow, this method gets the closest point to connect to, stupid solution i think
	RoadPoint GetClosestRoadPoint (RoadPoint[] roadPoints, GameObject closestTo, RoadPoint.State state) {
		float dist = 0;
		bool fix = true;
		RoadPoint tempPoint = null;
		for (int i = 0; i < roadPoints.Length; i++) {
			if (roadPoints[i].state == state && fix) {
				dist = Vector3.Distance (roadPoints [i].transform.position, closestTo.transform.position);
				tempPoint = roadPoints [i];
				fix = false;
			} else if (Vector3.Distance(roadPoints[i].transform.position, closestTo.transform.position) < dist && roadPoints[i].state == state) {
				dist = Vector3.Distance (roadPoints [i].transform.position, closestTo.transform.position);
				tempPoint = roadPoints [i];
			}
		}
		return tempPoint;
	}
}
