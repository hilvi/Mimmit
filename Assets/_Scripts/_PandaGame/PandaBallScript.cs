using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PandaBallScript : MonoBehaviour
{
    public delegate void ActivateBall();
    public static event ActivateBall OnBallActivate;

    private const float BOOST_MULTIPLIER = 100f;

    private bool _activated = false;
    private Rigidbody2D _rigidBody;
    private CircleCollider2D _circleCollider;

    #region UNITY_METHODS
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
                OnBallActivate();
            }
        }
    }

    void OnEnable()
    {
        PowerupScript.OnBallCapture += BoostBall;
    }

    void OnDisable()
    {
        PowerupScript.OnBallCapture -= BoostBall;
    }
    #endregion

    private void BoostBall()
    {
        _rigidBody.AddForce(_rigidBody.velocity * BOOST_MULTIPLIER);
    }
}
