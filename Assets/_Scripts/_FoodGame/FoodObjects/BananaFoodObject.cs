using UnityEngine;
using System.Collections;

public class BananaFoodObject : MonoBehaviour, IFoodObject
{
    void Start ()
    {
        renderer.material.color = Color.yellow;
    }

    public void Handle ()
    {
        Debug.Log ("Handle banana");
    }
    
    public FoodType GetFoodType ()
    {
        return FoodType.Banana;
    }
}

