using UnityEngine;
using System.Collections;

public class SceneControl : MonoBehaviour {

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height / 2), "Video Search"))
        {
            Application.LoadLevel("VideoSearch");
        }
        if (GUI.Button(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2), "Channel Search"))
        {
            Application.LoadLevel("ChannelSearch");
        }
    }
}
