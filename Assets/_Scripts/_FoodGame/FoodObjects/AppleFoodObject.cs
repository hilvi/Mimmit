using UnityEngine;
using System.Collections;

public class AppleFoodObject : MonoBehaviour, IFoodObject
{
    void Start ()
    {
        renderer.material.color = Color.red;
    }

    public void Handle ()
    {
        Debug.Log ("Handle apple");
    }

    public FoodType GetFoodType ()
    {
        return FoodType.Apple;
    }
}

