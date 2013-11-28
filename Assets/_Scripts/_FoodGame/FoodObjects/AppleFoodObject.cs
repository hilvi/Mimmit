using UnityEngine;
using System.Collections;

public class AppleFoodObject : MonoBehaviour, IFoodObject
{
    public Texture full;
    public Texture threeQuarter;
    public Texture half;
    public Texture oneQuarter;

    private FoodState state;
    public FoodState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            switch (State)
            {
                case FoodState.Full:
                    renderer.material.SetTexture(0, full);
                    break;
                case FoodState.Half:
                    renderer.material.SetTexture(0, half);
                    break;
                case FoodState.Quarter:
                    renderer.material.SetTexture(0, oneQuarter);
                    break;
            }
        }
    }

    void Start()
    {
        //renderer.material.color = Color.red;
        //renderer.material.SetTexture(0, half);
        //State = FoodState.Quarter;
    }

    public void Handle()
    {
        Debug.Log("Handle apple");
    }

    public FoodType GetFoodType()
    {
        return FoodType.Apple;
    }
}

