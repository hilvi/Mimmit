using UnityEngine;
using System.Collections;

public class FoodAnimation : MonoBehaviour
{
    protected bool _started = false;

    public void Begin()
    {
        _started = true;

        gameObject.SetActive(true);
    }
}
