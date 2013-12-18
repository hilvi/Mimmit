using UnityEngine;
using System.Collections;

public class RotatorScript : MonoBehaviour
{
    #region MEMBERS
    public float rotateSpeed;
    public float angularVelocityLimit;
    #endregion

    #region UNITY_METHODS
    void FixedUpdate()
    {
        rigidbody2D.AddTorque(rotateSpeed);
        if (rigidbody2D.angularVelocity > angularVelocityLimit)
            rigidbody2D.angularVelocity = angularVelocityLimit;
    }
    #endregion
}
