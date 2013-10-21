using UnityEngine;
using System.Collections;
[System.Serializable]
public class Animation2D :MonoBehaviour{
	#region MEMBERS
	public string animName;
	public Texture2D frames;
	public int frameX;
	public int frameY;
	public int startFrame = 1;
	public int endFrame = 0;
	public float frameRate = 1;
	public bool looping = true;
	
	float _frameWidth;
	float _frameHeight;
	int _textureWidth;
	int _textureHeight;
	#region PROPERTIES
	public float frameWidth
	{
		get
		{
			return _frameWidth;
		}
	}
	
	public float textureWidth
	{
		get
		{
			return _textureWidth;
		}
	}
	public float frameHeight
	{
		get
		{
			return _frameHeight;
		}
	}
	
	public float textureHeight
	{
		get
		{
			return _textureHeight;
		}
	}
	#endregion
	#endregion
	
	#region UNITY_METHODS
	void Start () 
	{
		_frameWidth = frames.width / frameX; 
		_textureWidth = frames.width;
		_frameHeight = frames.height / frameY; 
		_textureHeight = frames.height;
	}
	#endregion
	
	#region METHODS
	public string GetName() 
	{
		return animName;
	}
		
	
	#endregion
}
