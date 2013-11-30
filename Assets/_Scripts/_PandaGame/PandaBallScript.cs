using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PandaBallScript : MonoBehaviour
{
    public delegate void ActivateBall();
    public static event ActivateBall OnBallActivate;

    public delegate void BallStucked();
    public static event BallStucked OnBallStuck;

    public float powerupBoostMultiplier = 1f;
    private const float BOOST_FACTOR = 100f;

    private bool _activated = false;
    private bool _stuck = false;
    private float _stuckTime = 0f;
    private bool _gameOver = false;
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
        // If game is over, prevent further updates
        if (_gameOver)
            return;

        // If ball is activated, check if it gets stuck
        if (_activated)
        {
            // If ball is not stuck yet, check if it gets stuck
            if (!_stuck)
            {
                // If balls velocity equal zero, start measuring time
                Vector2 _velocity = _rigidBody.velocity;
                if (Vector2.SqrMagnitude(_velocity) < 0.1f)
                {
                    _stuckTime += Time.deltaTime;

                    // If measured time is long enough, we can say that ball is stuck
                    if (_stuckTime > 1f)
                    {
                        OnBallStuck();
                        _stuck = true;
                    }
                }
                else
                {
                    // If ball moves after staying still for a while, reset stuck timer
                    _stuckTime = 0f;
                }
            }
        }

        // Check if user has pressed left mouse btn and ball is not already active
        if (Input.GetMouseButtonDown(0) && !_activated)
        {
            /* 
             * Check if cursor position has collided with CircleCollider2D
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
        GoalScript.OnBallCapture += SetGameOver;
    }

    void OnDisable()
    {
        PowerupScript.OnBallCapture -= BoostBall;
        GoalScript.OnBallCapture -= SetGameOver;
    }
    #endregion

    private void BoostBall()
    {
        _rigidBody.AddForce(_rigidBody.velocity * BOOST_FACTOR * powerupBoostMultiplier);
    }

    private void SetGameOver()
    {
        _gameOver = true;

        // Stop ball so mommy bear can lick it clean
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.angularVelocity = 0;
    }
}
