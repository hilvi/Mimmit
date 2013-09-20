using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour 
{
	
	Dictionary<string,AudioClip> _audioDict = new Dictionary<string, AudioClip>();
	Transform _transform;

	void Awake () 
	{
		_transform = Camera.main.transform;
		
		string path = "Sound/Effect/"+Application.loadedLevelName;
		string[] words = path.Split('_');
		Object[] sounds = Resources.LoadAll(words[0], typeof(AudioClip));
		for(int i = 0; i < sounds.Length;i++)
		{
			AudioClip ac = (AudioClip)sounds[i];
			_audioDict.Add (ac.name,ac);
		}
	}
	
	public void PlayAudio(string audioName, Vector3 position)
	{
		if(_audioDict.ContainsKey(audioName))
		{
			AudioSource.PlayClipAtPoint(_audioDict[audioName],position);
		}
	}
	public void PlayAudio(string audioName)
	{
		if(_audioDict.ContainsKey(audioName))
		{
			AudioSource.PlayClipAtPoint(_audioDict[audioName],_transform.position);
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
