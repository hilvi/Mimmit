using System;
using UnityEngine;

public class PaintBrush
{
	public int id;
	public string name;
	public Color color;
	public Texture2D texture;

	public PaintBrush(int id, string name, Color color) {
		this.id = id;
		this.name = name;
		this.color = color;
	}
	
	public PaintBrush(int id, string name, Color color, Texture2D texture) {
		this.id = id;
		this.name = name;
		this.color = color;
		this.texture = texture;
	}
}


