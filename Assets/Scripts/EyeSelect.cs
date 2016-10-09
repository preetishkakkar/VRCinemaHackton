using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class EyeSelect : ButtonFunction {


	public override void Function(){
		Debug.Log ("Eye Select Called");
		var leftCamObj = GameObject.Find("Left Eye").gameObject;
		var leftCam = leftCamObj.GetComponent<Camera>();
		var leftBlur = (BlurOptimized)leftCam.GetComponent(typeof(BlurOptimized));
		leftBlur.enabled = !leftBlur.enabled;

		var rightCamObj = GameObject.Find ("Right Eye").gameObject;
		var rightCam = rightCamObj.GetComponent<Camera>();
		var rightBlur = (BlurOptimized)rightCam.GetComponent(typeof(BlurOptimized));
		rightBlur.enabled = !rightBlur.enabled;
	}
}
