using UnityEngine;
using System.Collections;


public class PuzzleMenu : MonoBehaviour {
	public PuzzleMenuItem[] levels;
	public GUIStyle guiStyle;
	public Texture2D frame;

	public int maxWidth = 3;
	public int maxHeight = 3;
	public int space = 50;

	int _buttonWidth;
	int _buttonHeight;

	PuzzleMenuManager _manager;

	// Use this for initialization
	void Start () {
		CountButtonSize();
		_manager = GameObject.Find ("GameManager").GetComponent<PuzzleMenuManager> ();
	}
	
	// Update is called once per frame
	void OnGUI () {
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
				if(GUI.Button(rect, levels[count].texture, guiStyle))
					StartCoroutine(_manager.LoadGame(levels[count].scene));

				float frameWidth, frameHeight, framePositionY, framePositionX;
				if(_buttonWidth < _buttonHeight) {
					frameWidth = (float)_buttonWidth / frame.width * levels[count].texture.width * 1.1f;
					frameHeight = frameWidth * levels[count].texture.height / levels[count].texture.width * 1.07f;

					framePositionY = position.y + _buttonHeight / 2 - frameHeight / 2 - frameHeight * 0.015f;
					framePositionX = position.x - frameWidth * 0.07f;
				} else {
					frameHeight = (float)_buttonHeight / frame.height * levels[count].texture.height * 1.17f;
					frameWidth = frameHeight * levels[count].texture.width / levels[count].texture.height * 0.94f;

					framePositionY = position.y + _buttonHeight / 2 - frameHeight / 2 - frameHeight * 0.02f;
					framePositionX = position.x + frameWidth * 0.05f;
				}

				GUI.DrawTexture(new Rect(framePositionX, framePositionY, frameWidth, frameHeight), frame);

				position.x += space + _buttonWidth;
			}
			position.y += space + _buttonHeight;
		}
	}
}
