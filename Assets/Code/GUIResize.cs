using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIResize : MonoBehaviour {

	//Another annoying thing of the legacy GUI, uh (not even my code)

	public static Rect ResizeGUI(Rect rect)
	{
		float FilScreenWidth = rect.width / 800;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = rect.height / 600;
		float rectHeight = FilScreenHeight * Screen.height;
		float rectX = (rect.x / 800) * Screen.width;
		float rectY = (rect.y / 800) * Screen.height;
		return new Rect (rectX, rectY, rectWidth, rectHeight);
	}
}
