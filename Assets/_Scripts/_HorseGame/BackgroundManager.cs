using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour {
	public Texture2D[] textures;

	// Use this for initialization
	void Start () {
		Shader __diffuse = Shader.Find("Diffuse");
		
		Material __mat = new Material(__diffuse);
		__mat.mainTexture = _MergeTextures(textures);
		renderer.material = __mat;
	}
	
	private Texture2D _MergeTextures(Texture2D[] __textures) {
		int __x = 0, __y = 0;
		foreach(Texture2D texture in __textures) {
			__x += texture.width;
			__y = Mathf.Max(__y, texture.height);
		}
		Texture2D __texture = new Texture2D(__x, __y);
		
		int __offset = 0;
		foreach(Texture2D texture in __textures) {
			for(int i = 0; i < texture.height; i++) {
				for(int j = 0; j < texture.width; j++) {
					__texture.SetPixel(j+__offset, i, texture.GetPixel(j,i));
				}
			}
			__offset += texture.width;
		}
		__texture.Apply();
		return __texture;
	}
}
