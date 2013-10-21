using UnityEngine;
using System.Collections;

public class GirlScript : MonoBehaviour {

	GUIAnimation anim;
	float timer;
	public float wait = 2;
	public bool randomValue = false;
	void Start () 
	{
		anim = GetComponent<GUIAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!anim.IsPlaying())
		{
			timer += Time.deltaTime;
		}
		if(timer > wait)
		{
			anim.Play();
			timer = 0;
			if(randomValue)
			{
				wait = Random.Range (2f,5f);
			}
		}
	}
}
