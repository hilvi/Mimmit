using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PuzzleGameManager : GameManager {

	public Texture2D picture;
	public PuzzlePiece[] pieces;
	public GameObject puzzlePiece;
	public GameObject puzzleSlot;
	public int puzzleWidth;
	public int puzzleHeight;

	GameObject _picked = null;
	
	List<GameObject> _pieces = new List<GameObject>();

	// Use this for initialization
	public override void Start () {
		base.Start();

		CreatePuzzle();
	}

	void CreatePuzzle() {
		foreach(PuzzlePiece piece in pieces) {
			GameObject __obj = Instantiate(puzzlePiece) as GameObject;
			__obj.renderer.material.mainTexture = piece.picture;

			Vector3 __size = __obj.transform.localScale;
			__size.x *= piece.size.x;
			__size.z *= piece.size.y;
			__obj.transform.localScale = __size;

			Vector2 __randPos = Random.insideUnitCircle;
			Vector2 __pos = __obj.transform.position;
			__pos.x += 1.2f * __randPos.x;
			__pos.y += 2 * __randPos.y;
			__obj.transform.position = __pos;

			PuzzlePieceScript __script = __obj.GetComponent<PuzzlePieceScript>();
			__script.x = (int)piece.position.x;
			__script.y = (int)piece.position.y;

			_pieces.Add(__obj);
		}
		CreateGrid ();
	}

	void CreateGrid() {
		//Planes are 10 times bigger than other objects..
		Vector3 __startPos = puzzleSlot.transform.position;
		foreach(GameObject piece in _pieces) {
			GameObject __obj = Instantiate (puzzleSlot) as GameObject;
			PuzzlePieceScript __pieceScript = piece.GetComponent<PuzzlePieceScript>();
			PuzzleSlotScript __slotScript = __obj.GetComponent<PuzzleSlotScript>();

			__slotScript.x = __pieceScript.x;
			__slotScript.y = __pieceScript.y;

			Vector3 __size = piece.transform.localScale * 10;
			__obj.transform.localScale = __size;

			Vector3 __gridPos = __startPos;
			__gridPos.x += __pieceScript.x * __size.x + __size.x / 2;
			__gridPos.y -= __pieceScript.y * __size.y + __size.y / 2;
			__obj.transform.position = __gridPos;
		}

	}

	IEnumerator LerpToPos(GameObject obj, Vector3 pos) {
		float __time = 0;
		while(true) {
			__time += Time.deltaTime;
			obj.transform.position = Vector2.Lerp(obj.transform.position, pos, __time);
			yield return null;
		}
	}

	// Update is called once per frame
	void Update () {
		if(_picked != null) {
			Vector3 __pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			__pos.z = _picked.transform.position.z;
			_picked.transform.position = Vector3.Lerp (_picked.transform.position, __pos, Time.deltaTime * 10);
		}
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray;

			if(_picked == null) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if(Physics.Raycast(ray, out hit)) {
					if(hit.collider.gameObject.tag == "PuzzlePiece")
						_picked = hit.collider.gameObject;
				}
			} else {
				ray = new Ray(_picked.transform.position + new Vector3(0, 0, 1), Vector3.forward);

				if(Physics.Raycast(ray, out hit)) {
					Debug.Log (hit.collider.gameObject.name);
					if(hit.collider.gameObject.tag == "PuzzleSlot") {
						PuzzlePieceScript __pieceScript = _picked.GetComponent<PuzzlePieceScript>();
						PuzzleSlotScript __slotScript = hit.collider.gameObject.GetComponent<PuzzleSlotScript> ();
						if(__pieceScript.x == __slotScript.x && __pieceScript.y == __slotScript.y) {
							_picked.collider.enabled = false;
							StartCoroutine (LerpToPos(_picked, hit.collider.gameObject.transform.position));
						}
					}
				}
				_picked = null;
			}
		}
	}
}
