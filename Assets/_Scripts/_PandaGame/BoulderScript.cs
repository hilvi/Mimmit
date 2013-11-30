using UnityEngine;
using System.Collections;

public class BoulderScript : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    #region UNITY_METHODS
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        PandaBallScript.OnBallActivate += DropStone;
    }

    void OnDisable()
    {
        PandaBallScript.OnBallActivate -= DropStone;
    }
    #endregion

    private void DropStone()
    {
        _rigidBody.isKinematic = false;
    }
}
