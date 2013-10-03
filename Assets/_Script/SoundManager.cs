using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour 
{
	
	Dictionary<string,AudioClip> _audioDict = new Dictionary<string, AudioClip>();
	public AudioSource audioSource;

	void Awake () 
	{
		
		string path = "Sound/Effect/"+Application.loadedLevelName;
		string[] words = path.Split('_');
		Object[] sounds = Resources.LoadAll(words[0], typeof(AudioClip));
		for(int i = 0; i < sounds.Length;i++)
		{
			AudioClip ac = (AudioClip)sounds[i];
			_audioDict.Add (ac.name,ac);
		}
	}
	public void PlayAudio(string audioName)
	{
		if(_audioDict.ContainsKey(audioName))
		{
			audioSource.clip = _audioDict[audioName];
			audioSource.Play();
		}
	}
	
	public float GetLength(string audioName)
	{
		if(_audioDict.ContainsKey(audioName))
		{
			return _audioDict[audioName].length;
		}
		return -1f;
	}
}
