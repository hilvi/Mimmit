using UnityEngine;
using System.Collections;

public class PreparationTableScript : MonoBehaviour
{
    private IFoodObject foodOnPrepTable;

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
            StartCoroutine(SnapFoodToTable(col.gameObject));
        }
    }

    public IFoodObject GetFoodOnTable()
    {
        return foodOnPrepTable;
    }

    private IEnumerator SnapFoodToTable(GameObject o)
    {
        // Ignore z
        Vector3 prepTablePos = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 foodPos = o.transform.position;
        while (true)
        {
            foodPos = Vector3.MoveTowards(foodPos, prepTablePos, Time.deltaTime * 10f);
            if (foodPos == prepTablePos)
                break;

            o.transform.position = foodPos;

            yield return null;
        }

        foodOnPrepTable = o.GetComponent(typeof(IFoodObject)) as IFoodObject;
    }
}
