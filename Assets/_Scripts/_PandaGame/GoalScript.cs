using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class GoalScript : MonoBehaviour {

    public delegate void CaptureBall();
    public static event CaptureBall OnBallCapture; 

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Ball")
        {
            OnBallCapture();
        }
    }
}
