using UnityEngine;
using System.Collections;

public class Animation2D : MonoBehaviour {
	
	public string animName;
	public Texture2D frames;
	public int frameCount;
	public int startFrame = 1;
	public int endFrame = 0;
	public float frameRate = 1;
	public bool looping = true;
	
	float frameWidth;
	int textureWidth;
	int currentFrame = 0;
	
	bool playing = false;
	float phase = 0;
	
	public void Restart() {
		currentFrame = startFrame;
	}
	
	public void Play() {
		playing = true;
		InitMaterial();
	}
	
	public string GetName() {
		return animName;
	}
	
	public void Stop() {
		playing = false;
	}
	
	// Use this for initialization
	void Start () {
		currentFrame = startFrame;
		frameWidth = frames.width / frameCount; 
		textureWidth = frames.width;
	}
	
	public void InitMaterial() {
		renderer.material.SetTexture("_MainTex", frames);
	}
	
	public void SetFrame(int frame) {

		float frameRelativeWidth = frameWidth / textureWidth;

		float tile = frameRelativeWidth * frame;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(tile, 0));
		renderer.material.SetTextureScale("_MainTex", new Vector2(frameRelativeWidth,1));
	}
	
	// Update is called once per frame
	void Update () {
		if (playing) {
			phase += Time.deltaTime;
			if(phase > 1/frameRate)
			{
				if(++currentFrame > endFrame)
				{
					currentFrame = 0;
				}
				phase = 0;
			}
			SetFrame(currentFrame);
		}
	}
}
