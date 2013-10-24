using System;
using UnityEngine;

public class PaintBrush
{
	public string name;
	public Color color;
	public Texture2D texture;
	
	// This pallette is manually computed from textures
	public static Color[] customPallette = {
		new Color(0.980f, 0.686f, 0.227f), // Yellow
		new Color(1.000f, 0.478f, 0.675f), // Pink
		new Color(0.918f, 0.318f, 0.125f), // Orange
		new Color(0.925f, 0.110f, 0.137f), // Red
		new Color(0.573f, 0.153f, 0.561f), // Magenta
		new Color(0.000f, 0.439f, 0.733f), // Blue
		new Color(0.000f, 0.572f, 0.271f), // Green
		new Color(0.459f, 0.294f, 0.137f)  // Brown
	};
	
	public PaintBrush(string name, Color color) {
		this.name = name;
		this.color = color;
	}
	
	public PaintBrush(string name, Color color, Texture2D texture) {
		this.name = name;
		this.color = color;
		this.texture = texture;
	}
}


