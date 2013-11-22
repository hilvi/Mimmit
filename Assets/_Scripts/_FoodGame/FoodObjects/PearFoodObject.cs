using UnityEngine;
using System.Collections;

public class PearFoodObject : MonoBehaviour, IFoodObject
{
    void Start ()
    {
        renderer.material.color = Color.green;
    }

    public void Handle ()
    {
        Debug.Log ("Handle pear");
    }
    
    public FoodType GetFoodType ()
    {
        return FoodType.Banana;
    }
}

