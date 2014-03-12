using UnityEngine;
using System.Collections;

public class PuzzleMenu : MonoBehaviour {
	public PuzzleMenuItem[] levels;
	public GUIStyle guiStyle;
	public Texture2D frame;

	public int gridWidth = 3;
	public int gridHeight = 2;
	public int spaceX = 50;

	public Vector2 margin;

	Vector2 _usableScreen;
	Vector2 _buttonSize;
	int _spaceY;

	void CalculateUsableScreen() {
		_usableScreen.x = Screen.width - margin.x * 2;
		_usableScreen.y = Screen.height - margin.y;
	}

	void CalculateButtonSize() {
		float width = frame.width * gridWidth;
		float height = frame.height * gridHeight;
		width += (gridWidth - 1) * spaceX;

		if (width > _usableScreen.x) {
			height = height / width * _usableScreen.x;
			width = _usableScreen.x;
		}
		if (height > _usableScreen.y) {
			width = width / height * _usableScreen.y;
			height = _usableScreen.y;
		}

		width -= (gridWidth - 1) * spaceX;
		_spaceY = (int)((_usableScreen.y - height) / (float)(gridHeight));

		width /= gridWidth;
		height /= gridHeight;

		_buttonSize.x = width;
		_buttonSize.y = height;
	}

	// Use this for initialization
	void Start () {
		CalculateUsableScreen ();
		CalculateButtonSize ();
	}
	
	void OnGUI() {
		Vector2 position = new Vector2(margin.x, margin.y);
		for(int i = 0, count = 0; i < gridHeight; i++) 
		{
			position.x = margin.x;
			for(int j = 0; j < gridWidth; j++, count++) 
			{
				PuzzleMenuItem level = levels[count];

				Rect button = new Rect(position.x, position.y, _buttonSize.x, _buttonSize.y);
				Rect texture = new Rect(button);
				texture.width *= 0.92f;
				texture.height *= 0.86f;
				texture.center = button.center;

				GUI.DrawTexture(texture, level.texture);
				GUI.Button(button, frame, guiStyle);
				GUI.Box(texture, level.size.ToString(), guiStyle);

				position.x += spaceX + _buttonSize.x;
			}
			position.y += _spaceY + _buttonSize.y;
		}
	}
}
