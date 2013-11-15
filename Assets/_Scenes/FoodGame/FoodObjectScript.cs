using UnityEngine;
using System.Collections;

public class FoodObjectScript : MonoBehaviour
{
    void Start ()
    {
        renderer.material.color = Color.red;
    }
	
    void Update ()
    {
	
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "Preparing Table") {
            Debug.Log ("prep table");
        }
    }
}
