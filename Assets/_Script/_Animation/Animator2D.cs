using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animator2D : MonoBehaviour {
	
	public string defaultAnimation = "idle";
	
	Animation2D currentAnimation;
	Dictionary<string, Animation2D> _animDict = new Dictionary<string, Animation2D>();
	float phase = 0;
	int currentFrame = 0;
	
	void Start()
	{
		Animation2D []_animations = GetComponents<Animation2D>();
		foreach(Animation2D a in _animations)
		{
			_animDict.Add (a.animName, a);
		}
		SwitchAnimation(defaultAnimation);		
	}
	
	/// <summary>
	/// Plays the given animation if it exists.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public void PlayAnimation(string animationName) 
	{
		if(animationName != currentAnimation.animName)
		{
			SwitchAnimation(animationName);
		}
		else if(currentAnimation == null)
		{
			Debug.LogError("No animation is currently assigned to the current animation variable");	
			return;
		}
		phase += Time.deltaTime;
		if(phase > 1 / currentAnimation.frameRate)
		{
			if(++currentFrame > currentAnimation.endFrame)
			{
				if(currentAnimation.looping == true)
				{
					currentFrame = 0;
				}
				else
				{
					return;
				}
			}
			phase = 0;
		}
		SetFrame(currentFrame);
	}
	
	public void StopAnimation() 
	{

	}
	
	public Animation2D GetCurrentAnimation() 
	{
		return currentAnimation;
	}
	
	public void SetLooping(bool looping) 
	{
		if (currentAnimation)
		{
			currentAnimation.looping = looping;
		}
	}
	
	
	void SwitchAnimation(string animationName) 
	{
		if(_animDict.ContainsKey(animationName))
		{
			currentAnimation = _animDict[animationName];
			currentFrame = currentAnimation.startFrame;
			currentAnimation.InitMaterial();
		}	
	}
	
	void SetFrame(int frame) 
	{
		float frameRelativeWidth = currentAnimation.frameWidth / currentAnimation.textureWidth;
		float tile = frameRelativeWidth * frame;
		currentAnimation.SetAnimationRenderer(tile,frameRelativeWidth);
	}
}
