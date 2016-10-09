using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;



public class vSData {
	public string title;
	public string thumbnail;
	public string id;
	

	public vSData(string newtitle, string newthumbnail, string newId)
	{
		title = newtitle;
		thumbnail = newthumbnail;
		id = newId;
	}
}

public class ChannelSearch : MonoBehaviour {
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

    public List<vSData> videos;
    public List<vData> videoList;

    bool generated = false;
    bool generatedChannels = false;

    void Start()
    {

        videos = new List<vSData>();
        videoList = new List<vData>();
        thumbs = new Texture2D[maxresults];
        youtube = new YoutubeVideo();

    }

    public Texture2D kronuzBg; //remove..

    void OnGUI()
    {

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), kronuzBg);

        GUI.Label(new Rect(0, 40, 40, 20), "Search:");
        searchString = GUI.TextField(new Rect(50, 40, 100, 20), searchString, 25);
        GUI.Label(new Rect(150, 40, 60, 20), "Category:");
        category = GUI.TextField(new Rect(200, 40, 100, 20), category, 25);



        if (GUI.Button(new Rect(0, 190, 100, 60), "Filter Category:\n" + useCategoryFilter.ToString()))
        {
            if (useCategoryFilter)
                useCategoryFilter = false;
            else
                useCategoryFilter = true;
        }

        if (GUI.Button(new Rect(0, 70, 100, 100), "Search Channel"))
        {
            YoutubeV3Call("video");
        }
        if (GUI.Button(new Rect(0, 270, 100, 60), "Filter By Date:\n" + filterByDate.ToString()))
        {
            if (filterByDate)
                filterByDate = false;
            else
                filterByDate = true;
        }


        if (generatedChannels)
        {
            int y = 70;
            for (int x = 0; x < videos.Count; x++)
            {
                GUI.DrawTexture(new Rect(100, y, 100, 100), thumbs[x]);
                if (GUI.Button(new Rect(100, y, 100, 100), ""))
                {
                    LoadChannelV3Videos(videos[x].id);
                    Debug.Log("Loading channel videos");
                }
                GUI.Label(new Rect(210, y + 2, 200, 20), videos[x].title);
                GUI.Label(new Rect(210, y + 22, 200, 40), "Click on thumbnail to \n open channel videos");

                y = y + 100;
            }
        }

        if (generated)
        {
            int y = 70;
            for (int x = 0; x < videoList.Count; x++)
            {
                GUI.DrawTexture(new Rect(100, y, 100, 100), thumbs[x]);
                if (GUI.Button(new Rect(100, y, 100, 100), ""))
                {
                    Debug.Log("Started Video - Only run on mobile");
                    StartCoroutine(PlayVideo(youtube.RequestVideo(videoList[x].id,360))); //360 is the desired video Quallity
                }

                GUI.Label(new Rect(210, y + 2, 200, 20), videoList[x].title);
                GUI.Label(new Rect(210, y + 22, 200, 20), "Duration: " + videoList[x].duration);
                GUI.Label(new Rect(210, y + 42, 200, 20), "Views: " + videoList[x].views + " Likes: " + videoList[x].likes + "");
                GUI.Label(new Rect(210, y + 62, 200, 20), "Dislikes: " + videoList[x].dislikes + " Fav: " + videoList[x].favorites);
                GUI.Label(new Rect(210, y + 82, 200, 20), "Comments: " + videoList[x].comments);



                y = y + 100;
            }
        }

        GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2), 100, 30), loadintStr);

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

    public void LoadChannelV3Videos(string channelId) {  //load videos of the desired channel
        videoList = new List<vData>();
        videos = new List<vSData>();
       

        if (filterByDate)
        {
            StartCoroutine(VideoSearchV3(channelId + "&part=snippet,id&order=date&type=videos&maxResults=" + maxresults + ""));
        }
        else
        {
            StartCoroutine(VideoSearchV3(channelId + "&part=snippet,id&type=videos&maxResults=" + maxresults + ""));
        }

        
    }

    public void YoutubeV3Call(string searchType)
    {
        generated = false;
        generatedChannels = false;
        videos = new List<vSData>();
        string newSearchString = searchString.Replace(" ", "%20");

        if (filterByDate)
        {
            if (useCategoryFilter)
                StartCoroutine(GetChannelsResult("https://www.googleapis.com/youtube/v3/search?q=" + newSearchString + "&key="+YourAPIKey+"&part=snippet,id&order=date&type=channel&category=" + category + "&maxResults=" + maxresults + ""));
            else
                StartCoroutine(GetChannelsResult("https://www.googleapis.com/youtube/v3/search?q=" + newSearchString + "&key="+YourAPIKey+"&part=snippet,id&order=date&type=channel&maxResults=" + maxresults + ""));
        }
        else
        {
            if (useCategoryFilter)
                StartCoroutine(GetChannelsResult("https://www.googleapis.com/youtube/v3/search?q=" + newSearchString + "&key="+YourAPIKey+"&part=snippet,id&type=channel&category=" + category + "&maxResults=" + maxresults + ""));
            else
                StartCoroutine(GetChannelsResult("https://www.googleapis.com/youtube/v3/search?q=" + newSearchString + "&key="+YourAPIKey+"&part=snippet,id&type=channel&maxResults=" + maxresults + ""));
        }

    }

    private string loadintStr = "";

    public bool filterByDate = true;

    IEnumerator VideoSearchV3(string url)
    {
        generatedChannels = false;
        videos = new List<vSData>();
        WWW call = new WWW("https://www.googleapis.com/youtube/v3/search?key="+YourAPIKey+"&channelId="+url);
        loadintStr = "Loading...";
        yield return call;
        Debug.Log(call.text);
        JSONNode youtubeReturn = JSON.Parse(call.text);
        youtubeReturn = youtubeReturn["items"];
        if (call.text == "" || call.text == null)
        {
            loadintStr = "ERROR>>>";
        }
        Debug.Log(call.text);

        //counter to update thumbnails, i'll do a better thumb load on future, it's just a sample.
        getDataCounter = youtubeReturn.Count;

        if (getDataCounter <= 0)
        {
            loadintStr = "nothing found";
        }
        Debug.Log(youtubeReturn.Count);
        for (int i = 0; i < youtubeReturn.Count; i++)
        {
            StartCoroutine(GetVideoInfo(youtubeReturn[i]["id"]["videoId"], youtubeReturn[i]["snippet"]["thumbnails"]["default"]["url"], youtubeReturn[i]["snippet"]["title"], youtubeReturn[i]["snippet"]["description"]));
        }

    }

    private int getDataCounter = 0;
    private int currentLoadCounter = 0;

    IEnumerator GetVideoInfo(string videoId, string videoThumb, string videoTitle, string videoDescription)
    {
        WWW ncal = new WWW("https://www.googleapis.com/youtube/v3/videos?key="+YourAPIKey+"&part=contentDetails,statistics&id=" + videoId);
        yield return ncal;
        JSONNode youtubeNewReturn = JSON.Parse(ncal.text);
        JSONNode data = JSON.Parse(youtubeNewReturn["items"][0].ToString());//Fixed, is getting some error to get Data

        string title = videoTitle;
        string desc = videoDescription;
        string id = videoId;
        string thumb = videoThumb;
        if (data != null)
        {
            string duration = data["contentDetails"]["duration"];
            //fix duration string:
            duration = duration.Replace("PT", "");
            string views = data["statistics"]["viewCount"];
            string likes = data["statistics"]["likeCount"];
            string dislikes = data["statistics"]["dislikeCount"];
            string favorites = data["statistics"]["favoriteCount"];
            string comments = data["statistics"]["commentCount"];

            //string licensed = data["statistics"]["licensedContent"];

            videoList.Add(new vData(title, duration, thumb, id, desc, views, likes, dislikes, favorites, comments));
        }
        
        //increment the counter, when all system get all data we can do the final steps
        currentLoadCounter++;

        if (currentLoadCounter >= getDataCounter)
        {
            thumbs = new Texture2D[videoList.Count];
            //all data loaded, we can download thumbnails now:
            for (int x = 0; x < videoList.Count; x++)
            {
                StartCoroutine(DownloadThumbs(videoList[x].thumbnail, x));
            }
            loadintStr = "";
            //Now update the scene with thumbnails:
            generated = true;
        }
    }

    IEnumerator GetChannelsResult(string url)
    {
        WWW call = new WWW(url);
        loadintStr = "Loading...";
        yield return call;
        JSONNode youtubeReturn = JSON.Parse(call.text);
        youtubeReturn = youtubeReturn["items"];
        if (call.text == "" || call.text == null)
        {
            loadintStr = "ERROR>>>";
        }

        Debug.Log(call.text);
        for (int i = 0; i < youtubeReturn.Count; i++)
        {
            videos.Add(new vSData(youtubeReturn[i]["snippet"]["title"], youtubeReturn[i]["snippet"]["thumbnails"]["default"]["url"], youtubeReturn[i]["id"]["channelId"]));
        }

        thumbs = new Texture2D[videos.Count];

        for (int x = 0; x < videos.Count; x++)
        {
            StartCoroutine(DownloadThumbs(videos[x].thumbnail, x));
        }
        loadintStr = "";
        generatedChannels = true;

    }



    public IEnumerator DownloadThumbs(string url, int idx)
    {
        WWW www = new WWW(url);
        yield return www;
        thumbs[idx] = new Texture2D(100, 100);
        www.LoadImageIntoTexture(thumbs[idx]);
    }
}
