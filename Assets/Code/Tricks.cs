using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tricks {

	//More like tools, haaaoooo... haha

	public static RoadPoint GetClosestObjectWithTag (GameObject gameObject, string tag) {
		GameObject[] objects = GameObject.FindGameObjectsWithTag (tag);
		GameObject tempObject = null;
		if (objects.Length > 1) {
			float dist = 0;
			dist = Vector3.Distance (gameObject.transform.position, objects [0].transform.position);
			tempObject = objects [0];
			for (int i = 1; i < objects.Length; i++) {
				if (Vector3.Distance (gameObject.transform.position, objects [i].transform.position) < dist) {
					dist = Vector3.Distance (gameObject.transform.position, objects [i].transform.position);
					tempObject = objects [i];
				}
			}
		} else
			return null;
		return tempObject.GetComponent<RoadPoint> ();
	}
}
