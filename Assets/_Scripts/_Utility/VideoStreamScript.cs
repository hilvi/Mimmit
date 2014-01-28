using UnityEngine;
using System.Collections;

public class VideoStreamScript : MonoBehaviour
{
    // Location of the video being streamed
    //private string url = "users.metropolia.fi/~vinht/test01.ogg";
    private string url = "users.metropolia.fi/~vinht/test02.ogg";
    // WWW-object retrieves contents of URL
    private WWW www;

    void Start ()
    {
        // Start downloading content
        www = new WWW (url);
        guiTexture.texture = www.movie;

        // Fit the video on screen 
        Rect r = new Rect (Screen.width / 2f, -Screen.height / 2f, 0f, 0f);
        guiTexture.pixelInset = r;
    }

    void Update ()
    {
        // Play movie once it has enough data
        var m = guiTexture.texture as MovieTexture;
        if (!m.isPlaying && m.isReadyToPlay) {
            m.loop = true;
            m.Play ();
        }
    }

    void OnGUI () {
        // Display download progress
        Rect r = new Rect (0f, 0f, 300f, 20f);
        GUI.Box (r, www.progress * 100f + "%");
    }
}
