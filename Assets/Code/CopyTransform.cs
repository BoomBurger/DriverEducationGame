using UnityEngine;
using System.Collections;

public class CopyTransform : MonoBehaviour {

	//this class i use in a lot of projects, could use some improvement... or finishing, volunteers anyone

	public enum TransformType {
		Position,
		Rotation,
		Scale
	}

	public enum Transition {
		Smooth,
		Sharp
	}

	public enum Axis {
		AllAxis,
		XAndY,
		YAndZ,
		XAndZ,
		x,
		y,
		z,
	}

	public TransformType transformType;
	public Transition transition;
	public Transform target;
	public float smoothSpeed;

	void Update ()
	{
		switch (transition) {
		case Transition.Sharp:
			switch (transformType) {
			case TransformType.Position:
				transform.position = target.position;
				break;
			case TransformType.Rotation:
				transform.rotation = target.rotation;
				break;
			case TransformType.Scale:
				transform.localScale = target.localScale;
				break;
			}
			break;
		case Transition.Smooth:
			switch (transformType) {
			case TransformType.Position:
				transform.position = Vector3.Lerp (transform.position, target.position, smoothSpeed * Time.deltaTime * Time.timeScale);
				break;
			case TransformType.Rotation:
				transform.rotation = Quaternion.Lerp (transform.rotation, target.rotation, smoothSpeed * Time.deltaTime * Time.timeScale);
				break;
			case TransformType.Scale:
				transform.localScale = Vector3.Lerp (transform.localScale, target.localScale, smoothSpeed * Time.deltaTime * Time.timeScale);
				break;
			}
			break;
		}
	}

	void FixedUpdate ()
	{
	}
}
