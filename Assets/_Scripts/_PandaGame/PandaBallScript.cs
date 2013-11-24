using UnityEngine;
using System.Collections;

public class PandaBallScript : MonoBehaviour
{
    private bool _activated = false;
    private Rigidbody2D _rigidBody;
    private CircleCollider2D _circleCollider;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        // If ball is already activated, forget further action
        if (_activated)
            return;

        // Check if user has pressed left mouse btn
        if (Input.GetMouseButtonDown(0))
        {
            /* Check if cursor position has collided with CircleCollider2D
             * in world space. If yes, drop the ball.
             */
            Vector3 __t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool __collided = _circleCollider.OverlapPoint(new Vector2(__t.x, __t.y));
            if (__collided)
            {
                _rigidBody.isKinematic = false;
                _activated = true;
            }
        }
    }
}
