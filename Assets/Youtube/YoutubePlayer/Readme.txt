Youtube Video Player

!Important, rembember to set API Compatibility level to .NET 2.0 in unity player settings, if you dont do this you will receive one error in the dlls.

How it works:
You need to pass the youtube video id and the system returns the video .mp4 file. 
You can call the unity3D Handheld.PlayFullScreenMovie to play your video in fullscreen.

Class Usage:
You need to instantiate the YoutubeVideo class.
And call StartCoroutine(youtube.LoadVideo(videoId1)) to get the video .mp4.

Example Scene:
We have one gameobject called YoutubeManager in that gameobject we have 2 fields:
videoId1 and videoId2 you can pass the youtube video id here.
When you run your project on mobile plataforms, if you click in one of the two buttons one video will be loaded in fullscreen.


Update: If you need to use your server to host the php files i added the php files to project, just upload to your server and change the variable
serverGetVideoFile to your getvideo.php path, attention: all files need to be in same directory in server.
-Working on pro version of this plugin, in the new version we will have control to search for videos inside unity, see thumbnails, and much more.
-In pro version we help you to make this work for desktop, inside a texture(in pro this will work on android too).


About the PHP Files:
If you want to use your own server to get the videos, upload the php files to your server and change the variable ServerGetVideoFile in YoutubeVideo
class using the new url of the php files.
The Php is a system to get the video in mp4 format.


ABOUT YOUTUBE API V3:

HOW TO DO A CORRECT USE:
-Changing your api key. (optional) I added my public api key to the project, but if you need to change with your api key check the search and ChannelSearch scripts.
 	I explain how to change if you need to point to your app and monitor the use.
-The youtube player only works on mobile, using handheld.playfullscreenmovie function for now.


In this version you can search for videos and search for channels (And the videos of desired channel), integrated to the youtube video player plugin.
You can play the desired video on mobile plataforms.

Changelog:

	-UPDATE THE WHOLE API , Updated to Youtube V3
	-Added Json Library
	-The return Types are in Json Format
	-Added the channel search in separated scene (Now you can search for Channels and if you click in one channel in results you will search for all videos of that channel)
	-Added a separeted scene of simple search, you can search for videos, and use 2 simple filters, you can add more, just use the Youtube docs for the calls. (contact me if you need some data and dont know how to get).
	-New Support.

For the next version:

	-ADD a new sample scene, using 2D UI.
	-ADD video callbacks.
	-ADD the new native player(in development)
	-ADD new Samples, with better code.
	-The videoplayer plugin is a php server system to provide a .mp4 video format to the mobile. I'll make this a little better, i'm working on one new system(dont use php and third party software). :)
		But for now i created a new host (a little better) to manage the files. But you can host your own server to prevent any server issues.
	-Video quality - I'll release this feature as a minor update. Need some tests to release.




Support:

If you need help you can send one email to

Email: kelvinparkour@gmail.com