using UnityEngine;
using System.Collections;

public class PreparationTableScript : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
	
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.CompareTag ("FoodObject")) {

            IFoodObject foodObject = col.gameObject.GetComponent (typeof(IFoodObject)) as IFoodObject;
            if (foodObject != null)
                foodObject.Handle ();
            else 
                Debug.LogError ("FoodObject is missing script");

            Debug.Log ("Food came in");
        }
    }
}
