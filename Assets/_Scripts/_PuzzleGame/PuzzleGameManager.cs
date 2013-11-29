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

    GameObject _picked = null;

    List<GameObject> _pieces = new List<GameObject>();
	float _timer = 0;

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

    void ShufflePieces()
    {
        foreach (GameObject piece in _pieces)
        {
            Vector2 __randPos = Random.insideUnitCircle;
            Vector3 __pos = puzzlePiece.transform.position;
            __pos.x += 1.5f * __randPos.x;
            __pos.y += 2.5f * __randPos.y;
            __pos.z = -Random.value - 5;
            piece.transform.position = __pos;
        }
    }

    IEnumerator LerpToPos(GameObject obj, Vector3 pos)
    {
        float __time = 0;
		pos.z = obj.transform.position.z;
        while (Vector2.Distance(obj.transform.position, pos) > 0.05f)
        {
            __time += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(obj.transform.position, pos, __time);
            yield return null;
        }
		obj.transform.position = (Vector2)pos;
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
					StartCoroutine(LerpToPos(_picked, hit.collider.gameObject.transform.position));
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
