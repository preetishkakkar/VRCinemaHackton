using UnityEngine;
using System.Collections;

public class TiltController : MonoBehaviour {


	public GameObject SkyBlockSphere;
	public float TiltAngle = 15;
	Cardboard[] CardboardCs;
	void Start () {
		CardboardCs = FindObjectsOfType<Cardboard> ();
	}

	void Update () {


		if (transform.rotation.eulerAngles.z > TiltAngle && transform.rotation.eulerAngles.z < 100 || 
		    transform.rotation.eulerAngles.z > 260 && transform.rotation.eulerAngles.z < 360 - TiltAngle) {
			foreach(Cardboard C in CardboardCs){
				C.Recenter();
			}
			SkyBlockSphere.transform.rotation = Quaternion.Euler(0, 270, 360 - transform.rotation.eulerAngles.x);


		}
	}
}
