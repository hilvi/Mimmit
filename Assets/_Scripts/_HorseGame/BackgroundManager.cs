﻿using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
	#region MEMBERS
	public Texture2D[] textures;
	public bool shuffle = false;
	public Texture2D background;
	#endregion
	
	#region UNITY_METHODS
	void Awake ()
	{
		Shader __diffuse = Shader.Find ("Diffuse");
		
		if (shuffle)
			_Shuffle ();
		
		Material __mat = new Material (__diffuse);
		background = _MergeTextures ();
		__mat.mainTexture = background;
		renderer.material = __mat;
	}
	#endregion
	
	#region METHODS
	private void _Shuffle ()
	{
		Texture2D tmp;
		for (int i = 0; i < textures.Length; i++) {
			int __pos = Random.Range (0, textures.Length);
			tmp = textures [i];
			textures [i] = textures [__pos];
			textures [__pos] = tmp;
		}
	}
	
	private Texture2D _MergeTextures ()
	{
		int __x = 0, __y = 0;
		foreach (Texture2D texture in textures) {
			__x += texture.width;
			__y = Mathf.Max (__y, texture.height);
		}
		Texture2D __texture = new Texture2D (__x, __y);
		
		int __offset = 0;
		foreach (Texture2D texture in textures) {
			Color[] pixels = texture.GetPixels ();
			__texture.SetPixels (__offset, 0, texture.width, texture.height, pixels);
			__offset += texture.width;
		}
		__texture.Apply ();
		return __texture;
	}
	#endregion
}
