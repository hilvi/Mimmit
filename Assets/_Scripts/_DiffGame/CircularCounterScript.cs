using UnityEngine;
using System.Collections;

public class CircularCounterScript : MonoBehaviour {
	
	private Texture2D _counterTexture;
	private Texture2D[] _pieSliceTextures = new Texture2D[7];
	private Rect _counterRegion;
	private int _sectorsActive = 0;
	
	#region UNITY_METHODS
	void Update () 
	{
		//_sectorsActive = Mathf.FloorToInt(Time.time) % 7;
	}
	#endregion
	
	#region METHODS	
	public void Draw () 
	{
		GUI.DrawTexture(_counterRegion, _counterTexture);
		for (int i = 0; i < _sectorsActive; i++) {
			GUI.DrawTexture(_counterRegion, _pieSliceTextures[i]);
		}
	}
	
	public void SetPosition(Vector2 position) {
		_counterRegion.x = position.x;
		_counterRegion.y = position.y;
	}
	
	public void SetPosition(float x, float y) {
		_counterRegion.x = x;
		_counterRegion.y = y;
	}
	
	public void SetActiveSectors(int n) {
		_sectorsActive = n;
		_sectorsActive = Mathf.Clamp(_sectorsActive, 0, 7);
	}

    public void Create(Color color)
    {
        int __width = 100;
        int __height = 100;
        _counterRegion = new Rect(0f, 0f, (float)__width, (float)__height);

        // Create empty textures
        _counterTexture = _CreateClearTexture(__width, __height);
        for (int i = 0; i < _pieSliceTextures.Length; i++)
        {
            _pieSliceTextures[i] = _CreateClearTexture(__width, __height);
        }

        // Begin constructing a circle with seven sectors
        const float __sectorArcLength = Mathf.PI * 2f / 7f;
        Vector2 __center = new Vector2(_counterRegion.width / 2f, _counterRegion.height / 2f);
        for (int x = 0; x < _counterTexture.width; x++)
        {
            for (int y = 0; y < _counterTexture.height; y++)
            {
                float _distFromCenter = Vector2.Distance(new Vector2(x, y), __center);

                // Set background layer
                if (_distFromCenter < __center.x)
                {
                    Color __fadedColor = new Color();
                    __fadedColor.r = color.r * 0.5f;
                    __fadedColor.g = color.g * 0.5f;
                    __fadedColor.b = color.b * 0.5f;
                    __fadedColor.a = 1f;
                    _counterTexture.SetPixel(x, y, __fadedColor);

                    // Artificial border
                    if (_distFromCenter > 43f)
                    {
                        _counterTexture.SetPixel(x, y, color);
                        continue;
                    }

                    // Color different sectors
                    Vector2 __op = new Vector2(x - __center.x, y - __center.y); // Offsetted position
                    float __angle = Mathf.Atan2(__op.y, __op.x);

                    //  Map angle from [-pi,pi] to [0, 2pi]
                    if (__angle < 0)
                        __angle = Mathf.PI * 2f + __angle;

                    // Check if pixel is in any of the seven sectors
                    for (int sector = 0; sector < 7; sector++)
                    {
                        if (__angle >= sector * __sectorArcLength &&
                            __angle < (float)(1 + sector) * __sectorArcLength)
                        {
                            _pieSliceTextures[sector].SetPixel(x, y, new Color(0f, 0.5f + sector / 14f, 0f));
                        }
                    }
                }
            }
        }

        // Save textures
        _counterTexture.Apply();
        for (int i = 0; i < _pieSliceTextures.Length; i++)
        {
            _pieSliceTextures[i].Apply();
        }
    }

	private Texture2D _CreateClearTexture(int width, int height)
	{
		Texture2D __t = new Texture2D(width, height);
		Color[] __c = __t.GetPixels();
		for (int i = 0; i < __c.Length; i++) {
			__c[i] = Color.clear;
		}
		
		__t.SetPixels(__c);
		__t.Apply();
		return __t;
	}
	#endregion
}
