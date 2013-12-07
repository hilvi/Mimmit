using UnityEngine;
using System.Collections;

enum Edge {
	Left,
	Right,
	Top,
	Bottom
}

enum MatchDirection {
	Horizontal,
	Vertical
}

public class PuzzleSolver : MonoBehaviour {
	public Texture2D[] pieces;
	int _buffer = 50;

	// Use this for initialization
	void Start () {
		Debug.Log(FindMatch(pieces[0], Edge.Right));
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	Vector4 Count(Texture2D texture, Edge edge) {
		Vector4 result = new Vector4();
		int depth = 0;
		int until, position, step, size;
		until = position = step = size = 0;
		MatchDirection dir = 0;
		switch(edge) {
		case Edge.Right:
			until = texture.height;
			position = texture.width;
			size = texture.width;
			step = -1;
			dir = MatchDirection.Horizontal;
			break;
		case Edge.Left:
			until = texture.height;
			position = 0;
			size = texture.width;
			step = 1;
			dir = MatchDirection.Horizontal;
			break;
		case Edge.Bottom:
			until = texture.width;
			position = 0;
			size = texture.height;
			step = 1;
			dir = MatchDirection.Vertical;
			break;
		case Edge.Top:
			until = texture.width;
			position = texture.height;
			size = texture.height;
			step = -1;
			dir = MatchDirection.Vertical;
			break;
		}
		
		for(int i = _buffer; i < until-_buffer; i++) {
			int counter = 0;
			int pos = position;
			Color pixel;
			do {
				if(dir == MatchDirection.Horizontal)
					pixel = texture.GetPixel(pos, i);
				else
					pixel = texture.GetPixel(i, pos);
				
				counter++;
				pos += step;
			} while(pixel.a == 0 && counter < size);
			depth = Mathf.Max(depth, counter);
			result += (Vector4)pixel;
		}
		if(depth < _buffer)
			return Vector4.zero;
		else
			return result;
	}

	float CountMatch(Vector4 count, Texture2D texture, Edge edge) {
		Vector4 edgeCount;
		switch(edge) {
		case Edge.Right:
			edgeCount = Count (texture, Edge.Left);
			break;
		case Edge.Left:
			edgeCount = Count (texture, Edge.Right);
			break;
		case Edge.Top:
			edgeCount = Count (texture, Edge.Bottom);
			break;
		case Edge.Bottom:
			edgeCount = Count (texture, Edge.Top);
			break;
		default:
			edgeCount = new Vector4();
			break;
		}
		
		return Vector4.Distance(count, edgeCount);
	}
	
	Texture2D FindMatch(Texture2D texture, Edge edge) {
		float min = float.MaxValue;
		Texture2D match = null;
		Vector4 count = Count (texture, edge);
		if(count == Vector4.zero)
			return null;
		foreach(Texture2D tex in pieces) {
			float val = CountMatch(count, tex, edge);
			Debug.Log(val);
			if(val < min) {
				min = val;
				match = tex;
			}
		}
		return match;
	}
}