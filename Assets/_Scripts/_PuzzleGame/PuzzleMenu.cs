using UnityEngine;
using System.Collections;

enum Pieces {
	Six = 0,
	Twelve = 1,
	Twenty = 2
}

public class PuzzleMenu : MonoBehaviour {
	public PuzzleMenuItem[] levels;
	public GUIStyle guiStyle;
	public Texture2D frame;

	Pieces _value = 0;
	int _sliderWidth = 100;

	public int maxWidth = 3;
	public int maxHeight = 3;
	public int space = 50;

	int _buttonWidth;
	int _buttonHeight;

	Rect _sliderPosition;
	Rect _sliderTextPosition;
	string _sliderText = "6";

	// Use this for initialization
	void Start () {
		CountButtonSize();
		_sliderPosition = new Rect(Screen.width/2-_sliderWidth/2, 15, _sliderWidth, 30);
		_sliderTextPosition = new Rect(Screen.width/2+_sliderWidth/2, 0, 45, 45);

	}
	
	// Update is called once per frame
	void OnGUI () {
		_value = (Pieces)GUI.HorizontalSlider(_sliderPosition, (int)_value, 0, 2);

		switch(_value) {
		case Pieces.Six:
			_sliderText = "6";
			break;
		case Pieces.Twelve:
			_sliderText = "12";
			break;
		case Pieces.Twenty:
			_sliderText = "20";
			break;
		}

		GUI.Box(_sliderTextPosition, _sliderText, guiStyle);
		DrawButtons();
	}

	void CountButtonSize() {
		int width = Mathf.Min(levels.Length, maxWidth);
		space /= width;

		if(levels.Length <= maxWidth) {
			_buttonWidth = (Screen.width - space * (levels.Length + 1)) / levels.Length;
			_buttonHeight = Screen.height - space * 2;
		}
		else {
			_buttonWidth = (Screen.width - space * (maxWidth + 1)) / maxWidth;
			int ratio = levels.Length / maxHeight;
			if(levels.Length % maxHeight == 0)
				_buttonHeight = (Screen.height - (ratio + 1) * space) / ratio;
			else
				_buttonHeight = (Screen.height - (ratio + 2) * space) / (ratio + 1);
		}
	}

	void DrawButtons() {
		Vector2 position = new Vector2(space, space*1.5f);
		for(int i = 0, count = 0; i < maxHeight; i++) {
			position.x = space;
			for(int j = 0; j < maxWidth; j++, count++) {
				if(levels.Length <= count)
					return;

				Rect rect = new Rect(position.x, position.y, _buttonWidth, _buttonHeight);
				if(GUI.Button(rect, levels[i].texture, guiStyle))
					Application.LoadLevel(levels[i].scene);

				float frameWidth = (float)_buttonWidth / frame.width * levels[i].texture.width * 1.1f;
				float frameHeight = frameWidth * levels[i].texture.height / levels[i].texture.width * 1.07f;
				float framePositionY = position.y + _buttonHeight / 2 - frameHeight / 2 - frameHeight * 0.015f;
				float framePositionX = position.x - frameWidth * 0.07f;

				Debug.Log (_buttonHeight);
				Debug.Log (_buttonWidth);

				GUI.DrawTexture(new Rect(framePositionX, framePositionY, frameWidth, frameHeight), frame);

				position.x += space + _buttonWidth;
			}
			position.y += space + _buttonHeight;
		}
	}
}
