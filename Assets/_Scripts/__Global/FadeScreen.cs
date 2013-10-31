using UnityEngine;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public float fadeSpeed = 1f;
	
	private Color _target;
	private Rect _screen;
	private Texture2D _texture;
    
    void Awake ()
    {
        _screen = new Rect(0f, 0f, Screen.width, Screen.height);
		_texture = new Texture2D(1,1);
		_SetColor(Color.clear);
    }
	void OnGUI() {
		GUI.depth = -1000;
		GUI.DrawTexture(_screen, _texture);
	}
	
	#region PROPERTIES
	public bool fadeInComplete {
		get {
			return _GetColor().a < 0.05f;
		}
	}
	public bool fadeOutComplete {
		get {
			return _GetColor().a > 0.95f;
		}
	}
	#endregion
    
	#region PUBLIC METHODS
	public void FadeIn() {
		StartCoroutine (WaitAndFadeIn());
	}
	public void FadeOut() {
		StartCoroutine (WaitAndFadeOut ());
	}
	
	public IEnumerator WaitAndFadeIn() {
		float __time = 0;
		_SetColor(Color.black);
		_target = Color.clear;
		while(!fadeInComplete) {
			__time += Time.deltaTime;
			_SetColor (Color.Lerp(_GetColor(), _target, fadeSpeed * __time));
			yield return null;
		}
		_SetColor(Color.clear);
	}
	public IEnumerator WaitAndFadeOut() {
		float __time = 0;
		_SetColor(Color.clear);
		_target = Color.black;
		while(!fadeOutComplete) {
			__time += Time.deltaTime;
			_SetColor (Color.Lerp(_GetColor(), _target, fadeSpeed * __time));
			yield return null;
		}
		_SetColor(Color.black);
	}
	#endregion
	
	#region PRIVATE METHODS
	private void _SetColor(Color color) {
		_texture.SetPixel(0,0,color);
		_texture.Apply ();
	}
	private Color _GetColor() {
		return _texture.GetPixel(0,0);
	}
	#endregion
}
