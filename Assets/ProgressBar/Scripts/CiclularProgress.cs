using UnityEngine;
using System.Collections;

public class CiclularProgress : MonoBehaviour {	
	bool IsRunning;
	void Awake(){
		gameObject.GetComponent<Renderer>().material.SetFloat("_Progress", -1);
	}

	public void RunProgress (float timeToComplete, ButtonFunction CurrentFunction) {
		if (!IsRunning) {
			StartCoroutine (RadialProgress (timeToComplete, CurrentFunction));
		}
	}
	public void ResetProgress(){
		StopAllCoroutines ();
		IsRunning = false;
		gameObject.GetComponent<Renderer>().material.SetFloat("_Progress", -1);
	}

	IEnumerator RadialProgress(float time, ButtonFunction CurrentFunction)
	{
		IsRunning = true;
		float rate = 1 / time;
		float i = 0;
		while (i < 1)
		{
			i += Time.deltaTime * rate;
			gameObject.GetComponent<Renderer>().material.SetFloat("_Progress", i);
			yield return 0;
		}
		IsRunning = false;
		CurrentFunction.Function ();
	}
}