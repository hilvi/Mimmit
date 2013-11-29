using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour
{
	#region MEMBERS
	protected static FadeScreen _fade;
	#endregion
	#region UNITY_METHODS
	public virtual void Awake ()
	{
		if(_fade == null)
			_fade = gameObject.AddComponent<FadeScreen> ();
	}
	#endregion
	#region METHODS
	public void FadeIn ()
	{
		StartCoroutine (_fade.FadeIn ());
	}

	public void FadeOut ()
	{
		StartCoroutine (_fade.FadeOut ());
	}

	public void LoadLevel (string level)
	{
		if(Application.CanStreamedLevelBeLoaded(level))
		{
			StartCoroutine (_LoadLevel (level));
		}
	}

	public void LoadLevelAndPlaySound (string level, AudioSource audio)
	{
		StartCoroutine (_PlaySound (audio));
		StartCoroutine (_LoadLevel (level));
	}

	private IEnumerator _PlaySound (AudioSource audio)
	{
		audio.Play ();
		while (audio.isPlaying) {
			yield return null;
		}
	}

	private IEnumerator _LoadLevel (string level)
	{
		yield return StartCoroutine(_fade.FadeOut());
		Manager.SetScreenChoice (ScreenChoice.Button);
		Application.LoadLevel (level);
	}
	#endregion
}
