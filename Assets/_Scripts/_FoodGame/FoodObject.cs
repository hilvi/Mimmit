using UnityEngine;
using System.Collections;

public class FoodObject : MonoBehaviour
{
    public enum Type { Apple, CutApple1, CutApple2, Banana, Knife }
    public Type type;
    public bool isFollowingCursor = false;

    public delegate void Clicked();
    public event Clicked OnClicked;

    void OnMouseDown()
    {
        OnClicked();
    }

    void Update()
    {
        if (isFollowingCursor)
        {
            Vector3 __p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            __p.z = 0f;
            transform.position = __p;
        }
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
