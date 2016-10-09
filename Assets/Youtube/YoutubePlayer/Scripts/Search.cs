using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


//AND DONT KNOW HOW TO ADD CONTACT ME: kelvinparkour@gmail.com 
//TO HELP OTHER DEVELOPERS I ADDED A JSON BASED RESPONSE ON THIS VERSION (IN OLDER VERSION I USED A POOR XML STRUCTURE).

public class vData {
	public string title;
	public string duration;
	public string thumbnail;
	public string id;
	public string desc;
	public string views;
	public string likes;
	public string dislikes;
	public string favorites;
	public string comments;
	

	public vData(string newtitle, string newduration, string newthumbnail, string newId,
	             string newDesc, string newViews, string newLikes, string newDislikes, string newFavorites, string newComments)
	{
		title = newtitle;
		duration = newduration;
		thumbnail = newthumbnail;
		id = newId;
		desc = newDesc;
		views = newViews;
		likes = newLikes;
		dislikes = newDislikes;
		favorites = newFavorites;
		comments = newComments;
	}
}


public class Search : MonoBehaviour {

	YoutubeVideo youtube;
	
	private string YourAPIKey = "AIzaSyDD-lxGLHsBIFPFPt2i31fc0tAHGeAb8mc"; //REMEMBER TO CHANGE HERE IF YOU NEED TO POINT TO YOUR GOOGLE APP. 
	/* 
	 * TO CREATE YOUR GOOGLE APP AND USE YOUR API GO TO:
	 * https://code.google.com/apis/console
	 * -Create a project.
	 * -Go to APIs -> YouTube APIs -> YouTube Data API and enable that.
	 * -then go to credentials create a new key for Public API access
	 * - now you have the API key just copy and change the variable YourAPIKey with your new API Key to monitor the use of youtube api calls.
	 * Any question? mail me: kelvinparkour@gmail.com
	 * 
	 * */

	public string searchString;
	public int maxresults;
	public bool useCategoryFilter;
	public string category;
	public Texture2D[] thumbs;

	public List<vData> videos;

	bool generated = false;

	public Texture2D kronuzBg; //remove..


    void Start()
    {
        videos = new List<vData>();
        thumbs = new Texture2D[maxresults];
        youtube = new YoutubeVideo();
    }

	void OnGUI(){

		GUI.DrawTexture (new Rect (0,0, Screen.width, Screen.height), kronuzBg);

		GUI.Label (new Rect (0, 40, 40, 20), "Search:");
		searchString = GUI.TextField (new Rect (50, 40, 100, 20), searchString, 25);
		GUI.Label (new Rect (150, 40, 60, 20), "Category:");
		category = GUI.TextField (new Rect (200, 40, 100, 20), category, 25);



		if (GUI.Button (new Rect (0, 190, 100, 60), "Filter Category:\n" + useCategoryFilter.ToString ())) {
			if (useCategoryFilter)
				useCategoryFilter = false;
			else 
				useCategoryFilter = true;
		}

		if (GUI.Button (new Rect (0, 70, 100, 100), "Search Video")) {
			YoutubeV3Call ("video");
			//SearchNow();
		}
		if (GUI.Button (new Rect (0, 270, 100, 60), "Filter By Date:\n" + filterByDate.ToString ())) {
			if (filterByDate)
				filterByDate = false;
			else 
				filterByDate = true;
		}


		if (generated) {
			int y = 70;
			for (int x = 0; x < videos.Count; x++) {
				GUI.DrawTexture (new Rect (100, y, 100, 100), thumbs [x]);
				if (GUI.Button (new Rect (100, y, 100, 100), "")) {

                    //the Handheld.PlayFullScreenMovie play the video using native unity and the first parameter are the video url
					Debug.Log("Started Video - Only run on mobile");
					StartCoroutine(PlayVideo(youtube.RequestVideo(videos[x].id,360))); //360 is the desired video quality
				}
				GUI.Label (new Rect (210, y + 2, 200, 20), videos [x].title);
				GUI.Label (new Rect (210, y + 22, 200, 20), "Duration: "+ videos [x].duration);
				GUI.Label (new Rect (210, y + 42, 200, 20), "Views: "+ videos [x].views +" Likes: "+ videos [x].likes +"");
				GUI.Label (new Rect (210, y + 62, 200, 20), "Dislikes: "+ videos [x].dislikes +" Fav: "+ videos [x].favorites);
				GUI.Label (new Rect (210, y + 82, 200, 20), "Comments: "+ videos [x].comments);

			

				y = y + 100;
			}
		}

		GUI.Label (new Rect ((Screen.width / 2), (Screen.height / 2), 100, 30), loadintStr);

	}

	public IEnumerator PlayVideo(string url)
	{
		yield return Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.Fill);
		Debug.Log("below this line will run when the video is finished");
		VideoFinished();
	}
	
	public void VideoFinished()
	{
		Debug.Log("Finished!");
		Application.LoadLevel(0);
	}

	public void YoutubeV3Call(string searchType){
		generated = false;
		videos = new List<vData> ();
		string newSearchString = searchString.Replace(" ", "%20");

		if (filterByDate) {
			if (useCategoryFilter)
				StartCoroutine (VideoSearchV3 ("https://www.googleapis.com/youtube/v3/search?q=" + newSearchString + "&key="+YourAPIKey+"&part=snippet,id&order=date&type=video&category="+category+"&maxResults=" + maxresults + ""));
			else
				StartCoroutine (VideoSearchV3 ("https://www.googleapis.com/youtube/v3/search?q=" + newSearchString + "&key="+YourAPIKey+"&part=snippet,id&order=date&type=video&maxResults=" + maxresults + ""));
		} else {
			if(useCategoryFilter)
				StartCoroutine (VideoSearchV3("https://www.googleapis.com/youtube/v3/search?q="+newSearchString+"&key="+YourAPIKey+"&part=snippet,id&type=video&category="+category+"&maxResults="+maxresults+""));
			else
				StartCoroutine (VideoSearchV3("https://www.googleapis.com/youtube/v3/search?q="+newSearchString+"&key="+YourAPIKey+"&part=snippet,id&type=video&maxResults="+maxresults+""));
		}
			
	}

	private string loadintStr = "";

	public bool filterByDate = true;

	IEnumerator VideoSearchV3(string url){
		WWW call = new WWW (url);
		loadintStr = "Loading...";
		yield return call;
		JSONNode youtubeReturn = JSON.Parse (call.text);
		youtubeReturn = youtubeReturn ["items"];
		if (call.text == "" || call.text == null) {
			loadintStr = "ERROR>>>";
		}
		Debug.Log (call.text);

		//counter to update thumbnails, i'll do a better thumb load on future, it's just a sample.
		getDataCounter = youtubeReturn.Count;

		if (getDataCounter <= 0) {
			loadintStr = "nothing found";
		}

		for (int i = 0; i < youtubeReturn.Count; i++) {
			StartCoroutine( GetVideoInfo(youtubeReturn[i]["id"]["videoId"],youtubeReturn[i]["snippet"]["thumbnails"]["default"]["url"],youtubeReturn[i]["snippet"]["title"],youtubeReturn[i]["snippet"]["description"]) );
		}

	}

	private int getDataCounter = 0;
	private int currentLoadCounter = 0;

	IEnumerator GetVideoInfo(string videoId,string videoThumb,string videoTitle,string videoDescription){
		WWW ncal = new WWW("https://www.googleapis.com/youtube/v3/videos?key="+YourAPIKey+"&part=contentDetails,statistics&id="+videoId);
		yield return ncal;
		JSONNode youtubeNewReturn = JSON.Parse (ncal.text);
		JSONNode data = JSON.Parse(youtubeNewReturn["items"][0].ToString());//Fixed, is getting some error to get Data

		string title = videoTitle;
		string desc = videoDescription;
		string id = videoId;
		string thumb = videoThumb;
		//Debug.Log (ncal.text);
		string duration = data["contentDetails"]["duration"];
		//fix duration string:
		duration = duration.Replace ("PT", "");
		string views = data["statistics"]["viewCount"];
		string likes = data["statistics"]["likeCount"];
		string dislikes = data["statistics"]["dislikeCount"];
		string favorites = data["statistics"]["favoriteCount"];
		string comments = data["statistics"]["commentCount"];

		//string licensed = data["statistics"]["licensedContent"];

		videos.Add (new vData(title,duration,thumb,id,desc,views,likes,dislikes,favorites,comments));
		//increment the counter, when all system get all data we can do the final steps
		currentLoadCounter++;

		if (currentLoadCounter >= getDataCounter) {
            thumbs = new Texture2D[videos.Count];
			//all data loaded, we can download thumbnails now:
			for(int x = 0; x < videos.Count; x++){
				StartCoroutine(DownloadThumbs(videos[x].thumbnail, x));
			}
			loadintStr = "";
			//Now update the scene with thumbnails:
			generated = true;
		}
	}

	public IEnumerator DownloadThumbs(string url, int idx){
		WWW www = new WWW(url);
		yield return www;
		thumbs[idx] = new Texture2D(100,100);
		www.LoadImageIntoTexture(thumbs[idx]);
	}

}
