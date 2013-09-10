using UnityEngine;
using System.Collections;

public class IntroBackBird : MonoBehaviour {
	
	public Texture2D bird;
	public float speedAnim = 2;
	public float speedMovement = 2;
	Rect position, texCoord;
	bool left;
	int sign = -1;
	// Use this for initialization
	void Start () {
#if UNITY_WEBPLAYER
		float width = Screen.width;
		float height = Screen.height;
		position = new Rect(0 - width / 15,260, width / 15, height /15);
#endif
		
		texCoord = new Rect(0 ,0, 0.5f,1f);
		StartCoroutine(WaitForNewBird());
	}
	
	void OnGUI()
	{
		GUI.DrawTextureWithTexCoords(position,bird,texCoord);
	}
	IEnumerator MoveBird()
	{
		float frame = 0;
		float movement = 0;
		float posXorigin = position.x;
		float endAnim = Screen.width + Screen.width / 15;
		
		while(Mathf.Abs (movement)< endAnim)
		{
			frame+=Time.deltaTime*speedAnim *sign;
			if(sign > 0)
			{
				float numerator = (int)frame % 2;
				texCoord = new Rect(numerator / 2 ,0, 0.5f,1f);
			}
			else
			{
				float numerator = (int)frame % 2;
				texCoord = new Rect(numerator / 2 + 1 ,0, -0.5f,1f);	
			}
			position.x = posXorigin + movement;
			movement += Time.deltaTime * speedMovement * -sign;
			yield return null;
		}
		sign *= -1;
		StartCoroutine(WaitForNewBird());
	}
	
	IEnumerator WaitForNewBird()
	{
		int rand = Random.Range (3,5);
		float timer = 0;
		while(timer < rand)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (MoveBird ());
	}
}
