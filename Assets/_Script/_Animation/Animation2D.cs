using UnityEngine;
using System.Collections;
[System.Serializable]
public class Animation2D :MonoBehaviour{
	
	public string animName;
	public Texture2D frames;
	public int frameCount;
	public int startFrame = 1;
	public int endFrame = 0;
	public float frameRate = 1;
	public bool looping = true;
	
	float _frameWidth;
	public float frameWidth
	{
		get
		{
			return _frameWidth;
		}
	}
	int _textureWidth;
	public float textureWidth
	{
		get
		{
			return _textureWidth;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		_frameWidth = frames.width / frameCount; 
		_textureWidth = frames.width;
	}
	
	public string GetName() 
	{
		return animName;
	}
		
	public void InitMaterial() 
	{
		renderer.material.SetTexture("_MainTex", frames);
	}
	
	public void SetAnimationRenderer(float tile, float frameWidth)
	{
		renderer.material.SetTextureOffset("_MainTex", new Vector2(tile, 0));
		renderer.material.SetTextureScale("_MainTex", new Vector2(frameWidth,1));
	}
}
