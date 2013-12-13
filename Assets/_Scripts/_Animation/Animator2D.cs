using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animator2D : MonoBehaviour {
	
	#region MEMBERS
	private int _currentFrameX = 0;
	private int _currentFrameY = 0;
	private float _phase = 0;
	private Animation2D _currentAnimation;
	private Dictionary<string, Animation2D> _animDict = new Dictionary<string, Animation2D>();
	private bool _playing;
	public float speed = 1.0f; 

	
	//public string defaultAnimation = "idle";
	public Renderer rendererObj;
	#endregion
	
	#region UNITY_METHODS
	void Awake()
	{
		Animation2D []_animations = GetComponents<Animation2D>();
		foreach(Animation2D a in _animations)
		{
			_animDict.Add (a.animName, a);
		}
		//SwitchAnimation(defaultAnimation);		
	}
	
	void Update()
	{
		if(!_playing)return;
		PlayAnimation(_currentAnimation.animName);
	}
	#endregion
	
	#region METHODS
	/// <summary>
	/// Plays the given animation if it exists.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public void PlayAnimation(string animationName) 
	{
		if(_currentAnimation == null)SwitchAnimation(animationName);
		if(animationName != _currentAnimation.animName)
		{
			SwitchAnimation(animationName);
		}
		_phase += Time.deltaTime *_currentAnimation.frameRate * speed;
		if(_phase > 1)
		{
			_SetCursor();
			_phase = 0;
		}
		SetFrame(_currentFrameX,_currentFrameY);
	}
	
	public void StopAnimation() 
	{
		_playing = false;
		_currentFrameY = 0;
		_currentFrameX = 0;
		SetFrame (_currentFrameX,_currentFrameY);
	}

	public bool IsPlaying()
	{
		return _playing;
	}
	
	public Animation2D GetCurrentAnimation() 
	{
		return _currentAnimation;
	}
	
	public void SetLooping(bool looping) 
	{
		if (_currentAnimation)
		{
			_currentAnimation.looping = looping;
		}
	}	
	
	void SwitchAnimation(string animationName) 
	{
		if(_animDict.ContainsKey(animationName))
		{
			_currentAnimation = _animDict[animationName];
			_currentFrameX = 0;
			_currentFrameY = _currentAnimation.frameY - 1;
			InitMaterial();
			_playing = true;
		}	
	}
	
	void SetFrame(int frameX, int frameY) 
	{
		float __frameRelativeWidth = _currentAnimation.frameWidth / _currentAnimation.textureWidth;
		float __frameRelativeHeight = _currentAnimation.frameHeight / _currentAnimation.textureHeight;
		float __tileX = __frameRelativeWidth * frameX;
		float __tileY = __frameRelativeHeight * frameY;

		renderer.material.SetTextureOffset("_MainTex", new Vector2(__tileX, __tileY));
		renderer.material.SetTextureScale("_MainTex", new Vector2(__frameRelativeWidth,__frameRelativeHeight));
	}
	
	public void InitMaterial() 
	{
		renderer.material.SetTexture("_MainTex",_currentAnimation.frames);
	}
	void _SetCursor()
	{
		if(++_currentFrameX < _currentAnimation.frameX)return;
		_currentFrameX = 0;
		if(_currentAnimation.frameY <= 1)return;
		if(--_currentFrameY >= 0)return;
		if(!_currentAnimation.looping)
		{
			_playing = false; 
			_currentFrameY = 0;
			_currentFrameX = 0;
			//SwitchAnimation(defaultAnimation);
			return;
		}
		_currentFrameX = 0;
		_currentFrameY = _currentAnimation.frameY - 1;
	}
	#endregion
}
