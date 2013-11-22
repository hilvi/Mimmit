using UnityEngine;
using System.Collections;

public class SlopeTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var edgeCol = GetComponent<EdgeCollider2D>();

        Vector2[] set = new Vector2[10];
        for (int i = 0; i < set.Length; i++)
        {
            set[i] = new Vector2(i, Random.value * 2f);
        }

        edgeCol.points = set;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
