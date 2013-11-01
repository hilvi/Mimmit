using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour {
	protected FadeScreen _fade;
	
	public virtual void Awake() {
		_fade = gameObject.AddComponent<FadeScreen>();
	}

	public void FadeIn() {
		StartCoroutine(_fade.FadeIn());
	}
	public void FadeOut() {
		StartCoroutine (_fade.FadeOut());
	}
	public void LoadLevel(string level) {
		StartCoroutine(_LoadLevel(level));
	}
	public void LoadLevelAndPlaySound(string level, AudioSource audio) {
		StartCoroutine(_PlaySound(audio));
		StartCoroutine(_LoadLevel(level));
	}

	private IEnumerator _PlaySound(AudioSource audio)
	{
		audio.Play();
		while(audio.isPlaying)
		{
			yield return null;
		}
	}
	private IEnumerator _LoadLevel(string level) {
		yield return StartCoroutine(_fade.FadeOut());
		Manager.SetScreenChoice(ScreenChoice.Button);
		Application.LoadLevel(level);
	}
}
