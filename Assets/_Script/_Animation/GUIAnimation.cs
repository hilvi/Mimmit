using UnityEngine;
using System.Collections;

public class GUIAnimation : MonoBehaviour {
	
	public Texture2D texture;
	public string animName;
	public int frameX;
	public int frameY;
	public Rect dimensions;
	public bool looping = true;
	public bool pingPong = true;
	public bool playOnStart = true;
	public bool invert = false;
	public float speedAnim = 2;
	public float speedMovement = 2;
	public int depth = 0;
	public Vector2[] path;
	Rect _texCoord,_position;
	float _widthRatio;
	float _heightRatio;
	float _frame = 0;
	bool _playing = true;
	public bool Playing{ get; set; }
	int index = 0;
	Vector3 _direction;
	int _sign;

	
	void Start () 
	{
		_widthRatio = 1f / (float)frameX;
		_heightRatio = 1f / (float)frameY;
		_texCoord = new Rect(0 ,0, _widthRatio,1f);
		_direction = new Vector3(0,0,0);
		_currentFrameY = frameY - 1;
		_sign = 1;
		if(pingPong)__ping = true;
		if(path.Length != 0)
			StartCoroutine("SetPosition");
		if(!playOnStart){
			_playing = false;
			_currentFrameX = 0;
			_currentFrameY = frameY - 1; 
			_texCoord = new Rect((float)_currentFrameX * _widthRatio ,(float)_currentFrameY * _heightRatio, _widthRatio,_heightRatio);
		}
		if(invert) _sign = 0;
	}
	bool __ping;
	bool __pong;
	void Update()
	{
		if(!_playing)return;
		PlayAnimation();
		if(!pingPong)
		{
			SetCursor();
			return;
		}
		if(__ping)
		{
			__pong = SetCursor();
			if(__pong)
			{
				__ping = false;
				_currentFrameX = frameX - 1;
				if(frameY > 1)
					_currentFrameY = frameY - 1;
			}
		}
		else if(__pong)
		{
			__ping = SetCursorBackward();
			if(__ping)
			{
				__pong = false;
				_currentFrameX = 0;
				if(frameY != 1)
					_currentFrameY = 0;
			}
		}
	}
	void OnGUI()
	{
		GUI.depth = depth;
		GUI.DrawTextureWithTexCoords(dimensions,texture,_texCoord);
	}
	void PlayAnimation()
	{		
		if(path.Length != 0)
		{
			Vector3 cross = Vector3.Cross (_direction, Vector3.forward);
			_sign = 1;
			if(cross.y < 0)
			{
				_sign = -1;
			}
		}
		_frame+=Time.deltaTime*speedAnim;
		
		float __X = (float)_currentFrameX * _widthRatio;
		float __Y = (float)_currentFrameY * _heightRatio;
		if(_sign > 0)
		{
			_texCoord = new Rect((float)_currentFrameX * _widthRatio ,(float)_currentFrameY * _heightRatio, _widthRatio,_heightRatio);
		}
		else
		{
			_texCoord = new Rect(__X + _widthRatio ,__Y, -_widthRatio,_heightRatio);	
		}
	}
	IEnumerator SetPosition()
	{
		while(true)
		{
			Vector2 __position = new Vector2(dimensions.x, dimensions.y);			if(__position != path[index])
			{
				__position = Vector2.MoveTowards(__position,path[index], speedMovement);
			}else if(++index >= path.Length)
			{
				int rand = Random.Range (1,5);
				yield return new WaitForSeconds(rand);
				index = 0;
			}
			dimensions.x = __position.x;
			dimensions.y = __position.y;
			Vector3 __path = new Vector3(path[index].x, path[index].y, 0);
			Vector3 __pos = new Vector3(__position.x, __position.y,0);
			_direction = __path - __pos;
			yield return null;
		}
	}
	int _currentFrameX = 0;
	int _currentFrameY = 0;
	
	bool SetCursor()
	{
		if(_frame < 1)return false;
		_frame = 0;
		if(++_currentFrameX < frameX)return false;
		if(!pingPong)
			_currentFrameX = 0;
		if(frameY > 1)
		{
			if(--_currentFrameY >= 0)return false;
		}
		if(!looping && !pingPong)
		{
			_playing = false; 
			_currentFrameY = 0;
			_currentFrameX = frameX - 1;
			return true;
		}
		if(pingPong) return true;
		if(frameY > 1)
		{
			_currentFrameX = 0;
			_currentFrameY = frameY - 1;
		}
		return true;
	}
	bool SetCursorBackward()
	{
		if(_frame < 1)return false;
		_frame = 0;
		if(--_currentFrameX >= 0)return false;
		_currentFrameX = frameX - 1;
		if(frameY > 1)
		{
			if(--_currentFrameY >= 0)return false;
		}
		if(!looping)
		{
			_playing = false; 
			_currentFrameY = frameY - 1;
			_currentFrameX = 0;
			return true;
		}
		if(pingPong) return true;
		if(frameY > 1)
		{
			_currentFrameX = 0;
			_currentFrameY = frameY - 1;
		}
		return true;
	}
	public void Play()
	{
		_currentFrameX = 0;
		if(frameY > 1)
		{
			_currentFrameY = frameY - 1;
		}
		else
			_currentFrameY = 0;
		_playing = true;
	}
	public bool IsPlaying()
	{
		return _playing;
	}
}
