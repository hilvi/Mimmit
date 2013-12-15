using System;
using UnityEngine;

[System.Serializable]
public class PaintFrame
{
    #region MEMBERS
    // Debug controls
    public FilterMode filterMode;
    public Texture2D VolatilePicture
    {
        get
        {
            return _volatilePicture;
        }

        set
        {
            // We do pixel-perfect copy of incoming picture to 
            // avoid modifying cached images.
            Color[] __c = value.GetPixels();
            Texture2D __t = new Texture2D(value.width, value.height);
            __t.filterMode = filterMode;
            __t.SetPixels(__c);
            __t.Apply();
            _volatilePicture = __t;
        }
    }

    // Defines region where the picture is
    public Rect pictureRegion;
    private Texture2D _volatilePicture;
    #endregion

    #region METHODS
    public void OnGUI()
    {
        if (VolatilePicture != null)
            GUI.DrawTexture(pictureRegion, VolatilePicture, ScaleMode.StretchToFill, true);
    }

    public void Paint(Vector2 position, Color color)
    {
        position.y = Screen.height - position.y; // y-axis flips yet again

        // Convert global cursor position to local texture position
        Vector2 __p = position -
            new Vector2(pictureRegion.x, pictureRegion.y); // Offset origin

        // Map coordinates to range [0,1] ..
        float __newX = __p.x / pictureRegion.width;
        float __newY = __p.y / pictureRegion.height;
        // .. then map back to texture size
        __newX *= VolatilePicture.width;
        __newY *= VolatilePicture.height;

        // Get color of pixel under cursor
        Color cursorColor = VolatilePicture.GetPixel((int)__newX, (int)__newY);

        // Ignore black pixels
        // Using distance function, because of floating-point precision issue
        if (Vector4.Distance(cursorColor, Color.black) < 0.1f)
        {
            Debug.Log("color is black, return");
            return;
        }
        // Ignore pixel colors that are same as current brush color
        if (Vector4.Distance(cursorColor, color) < 0.1f)
        {
            Debug.Log("identical color, return");
            return;
        }

        // Begin flood fill
        VolatilePicture.FloodFillArea((int)__newX, (int)__newY, color);

        // Save picture after setPixel operations
        VolatilePicture.Apply();
    }
    #endregion
}


