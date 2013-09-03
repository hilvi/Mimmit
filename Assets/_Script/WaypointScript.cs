using UnityEngine;
using System.Collections;

public class WaypointScript : MonoBehaviour {

	

float accel = 0.8f; 

float inertia = 0.9f; 

float speedLimit = 10.0f; 

float minSpeed = 1.0f; 

float stopTime = 1.0f; 

float currentSpeed = 0.0f;

int functionState = 0;

bool accelState;

bool slowState;

public Transform waypoint; 

float rotationDamping = 6.0f; 

bool smoothRotation = true; 

public Transform[] waypoints; 

int WPindexPointer; 

 
void Start ()

{
    functionState = 0;      
}

void Update () 
{
	if (functionState == 0) 
	{
        Accell (); 
    }
    if (functionState == 1)
    {
  		StartCoroutine(Slow ());
    }
    waypoint = waypoints[WPindexPointer]; 
}
void Accell () 

{

    if (accelState == false) 

    {                       
        accelState = true;   
        slowState = false;   
    }                        

    if (waypoint)

    {
        if (smoothRotation) 
        {
            var rotation = Quaternion.LookRotation(waypoint.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
        }
    }
    currentSpeed = currentSpeed + accel * accel;
    transform.Translate (0,0,Time.deltaTime * currentSpeed);

    

    if (currentSpeed >= speedLimit)

    {
        currentSpeed = speedLimit;
    }
}
void OnTriggerEnter ()

{

    functionState = 1; 
    WPindexPointer++;  

    

    if (WPindexPointer >= waypoints.Length)

    {
        WPindexPointer = 0; 
    }
}

IEnumerator Slow () 

{
    if (slowState == false) 
    {                       
        accelState = false; 
        slowState = true;   
    }                       
    currentSpeed = currentSpeed * inertia;

    transform.Translate (0,0,Time.deltaTime * currentSpeed);

    if (currentSpeed <= minSpeed)
    {
        currentSpeed = 0.0f; 
        yield return new WaitForSeconds (stopTime); 
        functionState = 0; 
    }
}
}

