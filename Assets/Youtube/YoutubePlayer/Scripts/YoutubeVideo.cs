using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YoutubeExtractor;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class YoutubeVideo : MonoBehaviour {

    public static YoutubeVideo Instance;

    void Awake()
    {
        Instance = this;
    }

    public bool drawBackground = false;
    public Texture2D backgroundImage;

    public string RequestVideo(string urlOrId, int quality)
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;


        Uri uriResult;
        bool result = Uri.TryCreate(urlOrId, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (!result)
            urlOrId = "https://youtube.com/watch?v=" + urlOrId;


        IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(urlOrId, false);
        List<VideoInfo> list = videoInfos.ToList<VideoInfo>();

        bool foundDesiredQual = false;
        bool found360 = false;
        bool found240 = false;

        VideoInfo video = null;

        video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == quality);
        Debug.Log("trying to play in " + quality + "! You can force another quallity when call the video.");

        if (video.RequiresDecryption)
        {
            DownloadUrlResolver.DecryptDownloadUrl(video);
        }

//        Debug.Log("The mp4 is: " + video.DownloadUrl);

        return video.DownloadUrl;
    }

    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }

	void OnGUI()
	{
		GUI.depth = 1;
		if(drawBackground)
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), backgroundImage);
		}
	}



}
