using UnityEngine;
using System.Collections;

public class RayControl : MonoBehaviour {
	MediaPlayerCtrl scrMedia;

	public float TimeBeteweenIteration = 0.24f;
	public float TimerToCallButton = 2.5f;
	public LayerMask ScreenLayer;
	public LayerMask ButtonLayer;
	Camera Cam;
	ButtonFunction[] Buttons;

	public GameObject Reticule;
	CiclularProgress ReticuleLoding;

	void Start () {
		ReticuleLoding = Reticule.GetComponentInChildren<CiclularProgress> ();
		Buttons = FindObjectsOfType<ButtonFunction> ();
		scrMedia = FindObjectOfType<MediaPlayerCtrl> ();
		Cam = GetComponent<Camera> ();
	}

	bool IsPlaying;
	Ray Check;
	RaycastHit Hit;

	void Update () {
		Check = Cam.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0));
		if (Physics.Raycast (Check, out Hit, Mathf.Infinity, ScreenLayer)) {
			if(!IsPlaying){
				Reticule.SetActive(false);
				scrMedia.Play();
				IsPlaying = true;
				StopAllCoroutines();
				foreach(ButtonFunction B in Buttons){
					B.gameObject.SetActive(false);
				}
			}
		} else {
			if(IsPlaying){
				scrMedia.Pause();
				Reticule.SetActive(true);
				StartCoroutine(CheckButtons());
				IsPlaying = false;
				foreach(ButtonFunction B in Buttons){
					B.gameObject.SetActive(true);
				}
			}
		}
	}


	private ButtonFunction CurrentFunction = null;
	IEnumerator CheckButtons(){
		while (true) {
			if(Physics.Raycast(Check, out Hit, Mathf.Infinity, ButtonLayer)){
				CurrentFunction = Hit.collider.GetComponent<ButtonFunction>();
				CurrentFunction.SwitchHighlight(true);
				ReticuleLoding.RunProgress(TimerToCallButton, CurrentFunction);

			} else{
				if(CurrentFunction != null){
					CurrentFunction.SwitchHighlight(false);
					ReticuleLoding.ResetProgress();
				}
			}
			yield return new WaitForSeconds (TimeBeteweenIteration);
		}
	}

}
