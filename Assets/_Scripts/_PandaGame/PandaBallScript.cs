using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PandaBallScript : MonoBehaviour
{
    public delegate void ActivateBall();
    public static event ActivateBall OnBallActivate;

    public delegate void BallStucked();
    public static event BallStucked OnBallStuck;

    public float powerupBoostMultiplier = 1f;
    private const float BOOST_FACTOR = 100f;

    private float _transformPCT = 0f;

    private bool _activated = false;
    private bool _stuck = false;
    private float _stuckTime = 0f;
    private bool _gameOver = false;

    private enum PandaState { FastRolling, SlowRolling, Standing };

    // References
    private Rigidbody2D _rigidBody;
    private CircleCollider2D _circleCollider;
    private MeshRenderer _meshRenderer;
    private AudioSource _pandaMoveSound;

    #region UNITY_METHODS
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _pandaMoveSound = GetComponent<AudioSource>();

        SelectActiveMaterial(PandaState.Standing);
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

            // If ball is moving fast enough, play rolling sound
            if (Vector2.SqrMagnitude(_rigidBody.velocity) > 1f)
            {
                // This condition is necessary, because Play() will reset sound
                if (!_pandaMoveSound.isPlaying)
                {
                    _pandaMoveSound.Play();
                }
            }
            else
            {
                if (_pandaMoveSound.isPlaying)
                {
                    _pandaMoveSound.Stop();
                }
            }

            // While ball is activated, check its angular velocity and calculate material transformation
            float __angularSpeed = Mathf.Abs(_rigidBody.angularVelocity);
            _transformPCT += __angularSpeed / 4800f;
            _transformPCT = Mathf.Clamp01(_transformPCT);

            // Create color mask and its inverse
            Color __mask = new Color(1f, 1f, 1f, _transformPCT);
            Color __inverseMask = new Color(1f, 1f, 1f, 1f - _transformPCT);

            // Reset all materials
            SelectActiveMaterial(PandaState.SlowRolling);

            // Apply masks
            Material[] __mats = _meshRenderer.materials;
            __mats[(int)PandaState.SlowRolling].color = __inverseMask;
            __mats[(int)PandaState.FastRolling].color = __mask;
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

    private void SelectActiveMaterial(PandaState state)
    {
        Material[] __mats = _meshRenderer.materials;
        for (int i = 0; i < 3; i++)
        {
            __mats[i].color = Color.clear;
        }

        __mats[(int)state].color = Color.white;
    }

    private void BoostBall()
    {
        _rigidBody.AddForce(_rigidBody.velocity * BOOST_FACTOR * powerupBoostMultiplier);
    }

    private void SetGameOver()
    {
        _gameOver = true;

        // Stop ball so mommy bear can lick it clean
        _rigidBody.isKinematic = true;
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.angularVelocity = 0;

        // Reset transform
        transform.localRotation = Quaternion.identity;

        // Reset texture
        SelectActiveMaterial(PandaState.Standing);

        // Stop sounds
        _pandaMoveSound.Stop();
    }
}
