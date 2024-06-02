using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadRules : MonoBehaviour {

	//the intersection robot system

	//I guess amber means (In my opinion) if possible you should stop, if too late, pass carefully. dont rush to pass the amber before it turns red, 
	//i have actually seen a bunch of horrible crashes because of that mistake, one of them right in front of me. sounded like a small explosion on impact, 
	//then tires screeching and both vehicles coming to a slow halt (T boned and stuck like that). slowly bumped into a traffic light. 
	//the woman driving the t-boned car died in the car because of the massive impact (after a while, few minutes)

		//DUDE, buzz kill. asshole!!! (i am talking about myself BTW)

	public PointState xState, zState;

	public enum PointState {
		Go,
		Ready,
		Stop
	}

	void Start () {
		StartCoroutine (RobotCalculation ());
	}

	void Update () {
		if (Input.GetKey(KeyCode.R)) {
			StopAllCoroutines ();
			StartCoroutine (RobotCalculation ());
		}
	}

	IEnumerator RobotCalculation () {
		xState = PointState.Go;
		zState = PointState.Stop;
		yield return new WaitForSeconds (8f);
		xState = PointState.Ready;
		zState = PointState.Stop;
		yield return new WaitForSeconds (4f);
		xState = PointState.Stop;
		zState = PointState.Go;
		yield return new WaitForSeconds (8f);
		xState = PointState.Stop;
		zState = PointState.Ready;
		yield return new WaitForSeconds (4f);
		StartCoroutine (RobotCalculation ());
	}
}
