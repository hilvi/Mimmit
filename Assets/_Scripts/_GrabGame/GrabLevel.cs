using UnityEngine;
using System.Collections;

[System.Serializable]
public class GrabLevel {
	public int numberOfCollectable;
	public int numberOfAvoidable;
	public int numberToCollect;
	public int minSpeed;
	public int maxSpeed;
	public int minOscillation;
	public int maxOscillation;
	public Pattern[] patterns;
	public int lanes;
	public float frequency;
	public int missesAllowed;

	internal GrabObjects _objects;

	public FallingObjectSettings[] GetFallingObjects() {
		FallingObjectSettings[] fallingObjects = new FallingObjectSettings[numberOfCollectable+numberOfAvoidable];

		Texture2D[] collectableObjects = _objects.collectable;
		Texture2D[] avoidableObjects = _objects.avoidable;

		_ShuffleObjects(ref collectableObjects);
		_ShuffleObjects(ref avoidableObjects);

		int id = 0;

		for(int i = 0; i < collectableObjects.Length && i < numberOfCollectable; i++, id++) {
			FallingObjectSettings fallingObject = _CreateObject();

			fallingObject.numberToCollect = numberToCollect;
			fallingObject.texture = collectableObjects[i];
			fallingObject.collect = true;

			fallingObjects[id] = fallingObject;
		}

		for(int i = 0; i < avoidableObjects.Length && i < numberOfAvoidable; i++, id++) {
			FallingObjectSettings fallingObject = _CreateObject();

			fallingObject.texture = avoidableObjects[i];
			fallingObject.numberToCollect = 1;
			fallingObject.collect = false;
			
			fallingObjects[id] = fallingObject;
		}

		return fallingObjects;
	}

	FallingObjectSettings _CreateObject() {
		FallingObjectSettings fallingObject = new FallingObjectSettings();

		fallingObject.maxSpeed = maxSpeed;
		fallingObject.minSpeed = minSpeed;
		fallingObject.maxOscillationAmplitude = maxOscillation;
		fallingObject.minOscillationAmplitude = minOscillation;

		return fallingObject;
	}

	void _ShuffleObjects(ref Texture2D[] objs)
	{
		for (int i = 0; i < objs.Length; i++)
		{
			Texture2D tmp;
			int newPos = Random.Range(0, objs.Length);
			
			tmp = objs[newPos];
			objs[newPos] = objs[i];
			objs[i] = tmp;
		}
	}
}
