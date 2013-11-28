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

    GameObject _picked = null;

    List<GameObject> _pieces = new List<GameObject>();

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("PuzzlePiece"))
        {
            _pieces.Add(piece);
        }

        CreatePuzzle();
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
            __pos.x += 1.2f * __randPos.x;
            __pos.y += 2 * __randPos.y;
            __pos.z = Random.value;
            piece.transform.position = __pos;
        }
    }

    /*void CreatePuzzle()
    {
        foreach (PuzzlePiece piece in pieces)
        {
            GameObject __obj = Instantiate(puzzlePiece) as GameObject;
            __obj.renderer.material.mainTexture = piece.picture;

            float __height = piece.picture.height;
            float __width = piece.picture.width;

            float __ratio = __height / __width;

            Vector3 __size = __obj.transform.localScale;
            __size.x *= piece.size.x;
            __size.y *= piece.size.y * __ratio;
            __obj.transform.localScale = __size;

            Vector2 __randPos = Random.insideUnitCircle;
            Vector3 __pos = __obj.transform.position;
            __pos.x += 1.2f * __randPos.x;
            __pos.y += 2 * __randPos.y;
            __pos.z = Random.value;
            __obj.transform.position = __pos;

            PuzzlePieceScript __script = __obj.GetComponent<PuzzlePieceScript>();
            __script.x = (int)piece.position.x;
            __script.y = (int)piece.position.y;

            _pieces.Add(__obj);
        }
        CreateGrid();
    }*/
    /*void CreateGrid()
    {
        Vector3 __startPos = puzzleSlot.transform.position;
        foreach (GameObject piece in _pieces)
        {
            GameObject __obj = Instantiate(puzzleSlot) as GameObject;
            PuzzlePieceScript __pieceScript = piece.GetComponent<PuzzlePieceScript>();
            PuzzleSlotScript __slotScript = __obj.GetComponent<PuzzleSlotScript>();

            __slotScript.x = __pieceScript.x;
            __slotScript.y = __pieceScript.y;

            Vector3 __size = piece.transform.localScale;
            __obj.transform.localScale = __size;

            Vector3 __gridPos = __startPos;
            __gridPos.x += __pieceScript.x * __size.x + __size.x / 2;
            __gridPos.y -= __pieceScript.y * __size.y + __size.y / 2;
            __obj.transform.position = __gridPos;
        }

    }*/

    IEnumerator LerpToPos(GameObject obj, Vector3 pos)
    {
        float __time = 0;
        while (Vector2.Distance(obj.transform.position, pos) > 0.05f)
        {
            __time += Time.deltaTime;
            obj.transform.position = Vector2.Lerp(obj.transform.position, pos, __time);
            yield return null;
        }
        //Scrap z-axis.
        obj.transform.position = (Vector2)pos;
    }

    void PutPickedOnTop()
    {
        if (_picked == null)
            return;
        float min = float.MaxValue;
        foreach (GameObject piece in _pieces)
        {
            min = Mathf.Min(min, piece.transform.position.z);
        }
        Vector3 __pickedPos = _picked.transform.position;
        __pickedPos.z = min - 0.0001f;
        _picked.transform.position = __pickedPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (_picked != null)
        {
            Vector3 __pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            __pos.z = _picked.transform.position.z;
            _picked.transform.position = Vector3.Lerp(_picked.transform.position, __pos, Time.deltaTime * 10);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray;

            if (_picked == null)
            {
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
            else
            {
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
        }
    }
}
