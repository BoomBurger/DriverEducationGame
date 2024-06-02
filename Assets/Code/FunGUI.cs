using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class FunGUI : MonoBehaviour {

	//The time i created this project, i hated the new GUI system that unity added cause i thought it was so much more complecated to use, now i love it and use it all the time.

	//I dont really see the fun in this class, where's the bouncy castle??????????

	public Texture2D[] timeControls;
	//public PostProcessVolume volume;

	private TimeToolBar timeToolbar;
	private bool SSAO, BLOOM, DOF, SSR;
	private int timeSelector = 1;

	public enum TimeToolBar {
		Pause = 0,
		Play = 1,
		FastForward = 2,
		FastForward2 = 3
	}

	//this is how you enable and disable visual effects of what i know of
	void Update () {
		//volume.profile.settings [0].enabled.value = SSAO;
		//volume.profile.settings [1].enabled.value = BLOOM;
		//volume.profile.settings [2].enabled.value = SSR;
		//volume.profile.settings [3].enabled.value = DOF;

		timeToolbar = (TimeToolBar)timeSelector;

		switch (timeToolbar) {
		case TimeToolBar.Pause:
			Time.timeScale = 0f;
			break;
		case TimeToolBar.Play:
			Time.timeScale = 1f;
			break;
		case TimeToolBar.FastForward:
			Time.timeScale = 5f;
			break;
		case TimeToolBar.FastForward2:
			Time.timeScale = 10f;
			break;
		}
	}

	//I didnt really care for quality as much as function (this GUI seems so much more annoying to work with to me now)
	void OnGUI () {

		timeSelector = GUI.Toolbar (new Rect (0, 0, 160, 40), timeSelector, timeControls);

		if (GUI.Button (new Rect (160, 0, 80, 40), "Remove All")) {
			GameObject[] roads = GameObject.FindGameObjectsWithTag ("Road");
			for (int i = 0; i < roads.Length; i++) {
				Destroy (roads[i]);
			}
			GameObject[] vehicles = GameObject.FindGameObjectsWithTag ("Vehicle");
			for (int i = 0; i < vehicles.Length; i++) {
				Destroy (vehicles[i]);
			}
		}

		SSAO = GUI.Toggle (new Rect (0, 40, 200, 25), SSAO, "Screen Space Ambient Occlusion");
		SSR = GUI.Toggle (new Rect (0, 65, 200, 25), SSR, "Screen Space Reflections");
		DOF = GUI.Toggle (new Rect (0, 90, 200, 25), DOF, "Depth of Field");
		BLOOM = GUI.Toggle (new Rect (0, 115, 200, 25), BLOOM, "Bloom");
	}
}
