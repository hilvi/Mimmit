using UnityEngine;
using System.Collections;
//Added namespace
using System.Collections.Generic;
 
[RequireComponent(typeof(CharacterController))]
public class AIScript : MonoBehaviour
{
    //Cached variables
    CharacterController _controller;
    Transform _transform;
 
    //Movement variables
    float speed = 1f;
    float gravity = 100f;
    Vector3 moveDirection;
    float maxRotSpeed = 200.0f;
    float minTime = 0.1f;
    float velocity;
    //Added variables
    float range;
    public List<Transform> waypoint;
    int index;
    public string strTag;
 
    void Start()
    {    
        _controller = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
 
        index = 0;
 
        //Added codes
        if (string.IsNullOrEmpty(strTag))
            Debug.LogError("No waypoint tag given");
        range = 2.5f;
    }
 
    void Update()
    {
        if ((_transform.position - waypoint[index].position).sqrMagnitude > range)
        {
            Move(waypoint[index]);
            //animation.CrossFade("walk");
        }
        else NextIndex();
    }
 
    void Move(Transform target)
    {
		if(GameObject.Find ("SeaSerpantObject") == gameObject) {
	        // Movement
	        moveDirection = _transform.forward;
	        moveDirection *= speed;
	        moveDirection.y -= gravity;
	        _controller.Move(moveDirection * Time.deltaTime);
	        //Rotation
	        var newRotation = Quaternion.LookRotation(target.position - _transform.position).eulerAngles;
	        var angles = _transform.rotation.eulerAngles;
	        _transform.rotation = Quaternion.Euler(angles.x,
	         Mathf.SmoothDampAngle(angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
		}
    }
 
    void NextIndex()
    {
        if (++index == waypoint.Count) index = 0;
    }
}