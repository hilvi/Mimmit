using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class PaintToolbar
{
    #region MEMBERS
    // Selected color
    public Color selectedColor;
    // Button regions
	public Rect eraseToolRegion;
	public Rect resetToolRegion;
	public Rect saveToolRegion;
    public Rect[] colorRegionArray;
    // Complete pallette
    public Color[] colorArray;
    public Texture2D tickTexture;
    private Rect _tickRect;
    // References
	private ColoringGameManager _manager;
    // Style for buttons
    private GUIStyle _noStyle = new GUIStyle();
    #endregion

    public PaintToolbar (ColoringGameManager manager)
	{
        // Set reference
		_manager = manager;
	}

	public void OnGUI ()
	{
        // Draw color pallette buttons
        for (int i = 0; i < colorArray.Length; i++)
        {
            if (GUI.Button(colorRegionArray[i], "", _noStyle))
            {
                ChangeColor(colorArray[i], colorRegionArray[i]);
                _manager.PlayClickSound();
            }
        }

        // Draw erase button
        if (GUI.Button(eraseToolRegion, "", _noStyle))
        {
            ChangeColor(Color.white, eraseToolRegion);
            _manager.PlayEraseSound();
        }

        // Draw reset button
        if (GUI.Button(resetToolRegion, "", _noStyle))
            _manager.ResetPictureToOriginal();	

        // Draw save button
        if (GUI.Button(saveToolRegion, "", _noStyle))
            _SavePicture();

        // Draw tick on selected color / tool
        GUI.DrawTexture(_tickRect, tickTexture);
	}

    public void ChangeColor(Color color, Rect rect)
    {
        selectedColor = color;
        _tickRect = rect;
    }

	private void _SavePicture ()
	{
		Texture2D __t = _manager.GetPictureFromFrame ();
		if (__t == null)
			return;
		
		// Encode texture to byte array
		byte[] __pngData = __t.EncodeToPNG ();
		
		// Further encode that to Base64 (html-friendly format)
		string __b64 = EncodePNGToBase64 (__pngData);
		
		/* Invoke JS-function showImageWindow()
		 * Example implementation thanks to Petri:
		 * 
         * <script type="text/javascript">
		 *   function showImageWindow(imageData) {
	     *   var win =  window.open("", "", "width=600, height=600");
	     *   win.document.body.innerHTML = '<img src="' + imageData + '">';
	     * }
	     *  </script>
	     */
		Application.ExternalCall ("showImageWindow", __b64);
	}
	
	private string EncodePNGToBase64 (byte[] pngData)
	{
		return "data:image/png;base64," +
    		Convert.ToBase64String (pngData, Base64FormattingOptions.None);
	}
	
	private bool IsBitSet (byte b, int pos)
	{
		return (b & (1 << pos)) != 0;
	}
}

