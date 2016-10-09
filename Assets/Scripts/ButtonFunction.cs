using UnityEngine;
using System.Collections;

public class ButtonFunction : MonoBehaviour {

	public bool ChangeColorOnHighlight = true;
	public Color ColorOfHighlightedButton = Color.green;
	private Color NeutralButtonColor;
	public bool PreviousVideo;
	public bool NextVideo;

	ButtonsControl Controller;
	MediaPlayerCtrl scrMedia;


	private Material Mat;
	void Start(){
		scrMedia = FindObjectOfType<MediaPlayerCtrl> ();
		Controller = GetComponentInParent<ButtonsControl> ();
		Mat = GetComponent<Renderer> ().material;
		NeutralButtonColor = Mat.color;
	}

	public void SwitchHighlight(bool Value){
		if (ChangeColorOnHighlight) {
			if (Value) {
				Mat.color = ColorOfHighlightedButton;
			} else {
				Mat.color = NeutralButtonColor;
			}
		}
	}
	public void Function(){
		if (PreviousVideo) {
			Controller.PreviousVideo();
		}
		if (NextVideo) {
			Controller.NextVideo();
		}
	}	
}
