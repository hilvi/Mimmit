using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PuzzleGameManager : GameManager
{

    public Texture2D picture;
    //public PuzzlePiece[] pieces;
    public GameObject puzzlePiece;
    public GameObject puzzleSlot;
    //public int puzzleWidth;
    //public int puzzleHeight;
    public float snapDistance = 10;
    public GameObject musicObject;
    public AudioClip music;
	public Vector3 completedPosition = new Vector3(0, 0, 0);

	GameObject _completed;
	Vector3 _completedPositionOrg;
	Vector3 _completedScaleOrg;
	Vector3 _puzzleSize;

    GameObject _picked = null;

    List<GameObject> _pieces = new List<GameObject>();
	float _timer = 0;

	public override void GoToNextLevel()
	{
		Time.timeScale = 1;
		LoadLevel("PuzzleMenu");
	}

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (InGameMenuGUI.music == null)
        {
            InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
            InGameMenuGUI.music.audio.clip = music;
            InGameMenuGUI.music.audio.Play();
            InGameMenuGUI.music.audio.loop = true;
        }

        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("PuzzlePiece"))
        {
            _pieces.Add(piece);
        }

		_puzzleSize = GetPuzzleSize();
		_completed = GameObject.Find("Completed");
		_completedPositionOrg = _completed.transform.position;
		_completedScaleOrg = _completed.transform.localScale;
		completedPosition.z = -2;

        CreatePuzzle();
        SetGameState(GameState.Running);
    }

    void CreatePuzzle()
    {
        foreach (GameObject piece in _pieces)
        {
            GameObject __obj = Instantiate(puzzleSlot) as GameObject;
            PuzzleSlotScript __slotScript = __obj.GetComponent<PuzzleSlotScript>();

            __slotScript.puzzlePiece = piece;

            Vector3 __pos = piece.transform.position;
            __pos.z = 10;
            __obj.transform.position = __pos;
            __obj.transform.localScale = piece.transform.localScale;
        }
        ShufflePieces();
    }

	Vector2 GetPuzzleSize()
	{
		float xMax, xMin;
		float yMax, yMin;

		yMin = xMin = float.MaxValue;
		xMax = yMax = float.MinValue;

		foreach(GameObject piece in _pieces)
		{
			Vector2 pos = piece.transform.position;
			Vector2 scale = piece.transform.localScale/2;
			Vector2 max = pos+scale;
			Vector2 min = pos-scale;

			xMax = Mathf.Max(max.x, xMax);
			xMin = Mathf.Min (min.x, xMin);

			yMax = Mathf.Max(max.y, yMax);
			yMin = Mathf.Min (min.y, yMin);
		}
		return new Vector2(xMax - xMin, yMax - yMin);
	}

    void ShufflePieces()
    {
        foreach (GameObject piece in _pieces)
        {
            Vector2 __randPos = Random.insideUnitCircle;
            Vector3 __pos = puzzlePiece.transform.position;
            __pos.x += 1.5f * __randPos.x;
            __pos.y += 1.5f * __randPos.y;
            __pos.z = -Random.value - 5;
            piece.transform.position = __pos;
        }
    }

    IEnumerator LerpToPos(GameObject obj, Vector3 pos, bool removeZ)
    {
        float __time = 0;
		pos.z = obj.transform.position.z;
        while (Vector2.Distance(obj.transform.position, pos) > 0.05f)
        {
            __time += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(obj.transform.position, pos, __time);
            yield return null;
        }
		if (removeZ)
			pos = (Vector2)pos;
		obj.transform.position = pos;
    }
	IEnumerator LerpToSize(GameObject obj, Vector3 size)
	{
		float __time = 0;
		while (Vector3.Distance(obj.transform.localScale, size) > 0.05f)
		{
			__time += Time.deltaTime;
			obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, size, __time);
			yield return null;
		}
		obj.transform.localScale = size;
	}


    void PutPickedOnTop()
    {
        if (_picked == null)
            return;
        float min = float.MaxValue;
        foreach (GameObject piece in _pieces)
        {
			if(piece.collider.enabled != false)
            	min = Mathf.Min(min, piece.transform.position.z);
        }
        Vector3 __pickedPos = _picked.transform.position;
        __pickedPos.z = min - 0.0001f;
        _picked.transform.position = __pickedPos;
    }

    bool GameWon()
    {
        foreach (GameObject piece in _pieces)
        {
            if (piece.collider.enabled == true)
                return false;
        }
        return true;
    }

	void PickUp()
	{
		RaycastHit hit;
		Ray ray;

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.gameObject.tag == "PuzzlePiece")
			{
				_picked = hit.collider.gameObject;
				PutPickedOnTop();
			}
			if (hit.collider.gameObject.name == "Completed")
			{
				if(hit.collider.gameObject.transform.localScale == _puzzleSize
				   && hit.collider.gameObject.transform.position == completedPosition)
				{
					StartCoroutine(LerpToPos(hit.collider.gameObject, _completedPositionOrg, false));
					StartCoroutine (LerpToSize(hit.collider.gameObject, _completedScaleOrg));
				}
				else if(hit.collider.gameObject.transform.localScale == _completedScaleOrg
				        && hit.collider.gameObject.transform.position == _completedPositionOrg)
				{
					StartCoroutine(LerpToPos(hit.collider.gameObject, new Vector3(0,0,-2), false));
					StartCoroutine(LerpToSize(hit.collider.gameObject, _puzzleSize));
				}
			}

		}
	}

	void DropDown()
	{
		RaycastHit hit;
		Ray ray;

		ray = new Ray(_picked.transform.position + new Vector3(0, 0, 1), Vector3.forward);
		
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.gameObject.tag == "PuzzleSlot")
			{
				PuzzleSlotScript __slotScript = hit.collider.gameObject.GetComponent<PuzzleSlotScript>();
				
				float distance = Vector2.Distance(_picked.transform.position, hit.collider.gameObject.transform.position);
				
				if (__slotScript.puzzlePiece == _picked && distance < snapDistance)
				{
					_picked.collider.enabled = false;
					StartCoroutine(LerpToPos(_picked, hit.collider.gameObject.transform.position, true));
				}
			}
		}
		_picked = null;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (GameWon())
			SetGameState(GameState.Won);

		if (_picked != null)
		{
			Vector3 __pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			__pos.z = _picked.transform.position.z;
            _picked.transform.position = Vector3.Lerp(_picked.transform.position, __pos, Time.deltaTime * 20);
        }

		_timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
			_timer = 0;
            if (_picked == null)
            {
				PickUp ();
            }
            else
            {
				DropDown ();
            }
        }

		if(_picked != null && Input.GetMouseButtonUp(0) && _timer > 0.2f)
			DropDown();
    }
}
