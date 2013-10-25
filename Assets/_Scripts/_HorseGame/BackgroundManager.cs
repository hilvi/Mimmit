using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour {
	public Texture2D texture;
	public int tiles;

	// Use this for initialization
	void Start () {
		Shader __diffuse = Shader.Find("Diffuse");
		
		Material __mat = new Material(__diffuse);
		__mat.mainTexture = texture;
		__mat.mainTextureScale = new Vector2(tiles, 1);
		__mat.mainTexture.wrapMode = TextureWrapMode.Repeat;
		renderer.material = __mat;
		
		
		/*__scale = textures.Length;
		Material[] __mats = new Material[__scale];
		for(int i = 0; i < __scale; i++) {
			Material __mat = new Material(Shader.Find("Diffuse"));
			__mat.mainTexture = textures[i];
			__mat.mainTextureScale = new Vector2(__scale, 1);
			__mat.mainTextureOffset = new Vector2(i, 0);
			__mat.mainTexture.wrapMode = TextureWrapMode.Clamp;
			__mats[i] = __mat;
		}
		renderer.materials = __mats;*/
	}
}
