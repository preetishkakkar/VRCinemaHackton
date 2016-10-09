using UnityEngine;
using System.Collections;

public class ButtonsControl : MonoBehaviour {
	public static ButtonsControl Instance;

	MediaPlayerCtrl scrMedia;
	ButtonFunction[] Buttons;

	public int DefaultResolution = 360;

	public string[] VideoList;
	int CurrentIndex;
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (Instance != this) {
			Destroy(this.gameObject);
		}
		scrMedia = FindObjectOfType<MediaPlayerCtrl> ();
		Buttons = FindObjectsOfType<ButtonFunction> ();
	}

	public void PassURL(string Id){											//Pass the URL to MediaPlayerCtrl
		scrMedia.Load(YoutubeVideo.Instance.RequestVideo (Id, DefaultResolution).ToString());
	}

	void Start(){
		CurrentIndex = 0;
		PassURL (VideoList [0]);
		scrMedia.Play ();
	}
	
	public void NextVideo(){
		if (CurrentIndex == VideoList.Length - 1) {
			CurrentIndex = 0;
		} else {
			CurrentIndex++;
		}
		string Id = VideoList [CurrentIndex];
		PassURL (Id);
	}

	public void PreviousVideo(){
		if (CurrentIndex == 0) {
			CurrentIndex = VideoList.Length - 1;
		} else {
			CurrentIndex--;
		}
		string Id = VideoList [CurrentIndex];
		PassURL (Id);
	}

	public void ButtonsSwitch(bool Value){
		foreach (ButtonFunction B in Buttons) {
			B.enabled = Value;
		}
	}


}
