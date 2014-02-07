using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class VideoStreamScript : MonoBehaviour
{
    // Flags
    public bool looping = false;
    public bool displayProgress = false;
    // Location of the video being streamed
    public string url = "mimmit.com/game/";
    public string videoName = "win_scene.ogv";
    // WWW-object retrieves contents of URL
    private WWW www;

    void Start ()
    {
        // Start downloading content
        www = new WWW (url + videoName);
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
            m.loop = looping;
            m.Play ();
        }
    }

    void OnGUI () {
        if (displayProgress) {
            // Display download progress
            Rect r = new Rect (0f, 0f, 300f, 20f);
            GUI.Box (r, www.progress * 100f + "%");
        }
    }
}
