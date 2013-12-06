using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreparationTableScript : MonoBehaviour
{
    private List<IFoodObject> foodOnTable = new List<IFoodObject>();

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("FoodObject"))
        {
            IFoodObject foodObject = col.gameObject.GetComponent(typeof(IFoodObject)) as IFoodObject;
            if (foodObject != null)
                foodObject.Handle();
            else
                Debug.LogError("FoodObject is missing script");

            Debug.Log("Food came in");
            foodOnTable.Add(foodObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Food went out");
        foodOnTable.Remove((IFoodObject)other.GetComponent(typeof(IFoodObject)));
    }

    public List<IFoodObject> GetFoodOnTable()
    {
        return foodOnTable;
    }
}
