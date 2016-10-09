using UnityEngine;
using System.Collections;

public class YoutubeEasyMovieTexture : MonoBehaviour
{
	ButtonsControl Controller;
	public int Resolution = 360;
    public string youtubeVideoIdOrUrl;

	void Awake(){
		Controller = FindObjectOfType<ButtonsControl> ();
		youtubeVideoIdOrUrl = Controller.VideoList [0];
	}

	void Start () {
		LoadYoutubeInTexture(youtubeVideoIdOrUrl, Resolution);
	}

    public void LoadYoutubeInTexture(string Id, int Res)
    {
		if(Res == 0){
			Res = Resolution;
		}
        //ALERT If you are using EasyMovieTexture uncomment the line..
        this.gameObject.GetComponent<MediaPlayerCtrl>().m_strFileName = YoutubeVideo.Instance.RequestVideo(Id,Res);
        
    }
}
