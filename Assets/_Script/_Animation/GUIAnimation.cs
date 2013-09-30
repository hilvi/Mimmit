using UnityEngine;
using System.Collections;

public class GUIAnimation : MonoBehaviour {
	
	public Texture2D texture;
	public string animName;
	public int frames;
	public Rect dimensions;
	public bool looping = true;
	public float speedAnim = 2;
	public float speedMovement = 2;
	public Vector2[] path;
	Rect _texCoord,_position;
	float _widthRatio;
	float _frame = 0;
	bool _playing = true;
	public bool Playing{ get; set; }
	int index = 0;
	Vector3 _direction;
	int _sign;

	
	void Start () 
	{
		_widthRatio = 1f / (float)frames;
		_texCoord = new Rect(0 ,0, _widthRatio,1f);
		_direction = new Vector3(0,0,0);
	}
	void Update()
	{
		PlayAnimation();
		SetPosition();
	}
	void OnGUI()
	{
		if(!_playing)return;
		GUI.DrawTextureWithTexCoords(dimensions,texture,_texCoord);
	}
	void PlayAnimation()
	{		
		_frame+=Time.deltaTime*speedAnim;
		Vector3 cross = Vector3.Cross (_direction, Vector3.forward);
		_sign = 1;
		if(cross.y < 0)
		{
			_sign = -1;
		}
		float __numerator = (int)_frame % frames;
		if(_sign > 0)
		{
			_texCoord = new Rect(__numerator / frames ,0, _widthRatio,1f);
		}
		else
		{
			_texCoord = new Rect(__numerator / frames + _widthRatio ,0, -_widthRatio,1f);	
		}
	}
	void SetPosition()
	{
		Vector2 __position = new Vector2(dimensions.x, dimensions.y);
		if(__position != path[index])
		{
			__position = Vector2.MoveTowards(__position,path[index], speedMovement);
		}else if(++index >= path.Length)
		{
			index = 0;
		}
		dimensions.x = __position.x;
		dimensions.y = __position.y;
		Vector3 __path = new Vector3(path[index].x, path[index].y, 0);
		Vector3 __pos = new Vector3(__position.x, __position.y,0);
		_direction = __path - __pos;
	}
}
