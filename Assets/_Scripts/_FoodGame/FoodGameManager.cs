using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodGameManager : GameManager
{
    #region MEMBERS
    public GameObject musicObject;
    public AudioClip music;
    private Transform grabbedObject;
    #endregion

    #region UNITY_METHODS
    public override void Start ()
    {
        // Boilerplate
        base.Start ();
        SetGameState (GameState.Running);

        if (InGameMenuGUI.music == null) {
            InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
            InGameMenuGUI.music.audio.clip = music;
            InGameMenuGUI.music.audio.Play ();
        }
    }

    void Update ()
    {
        // Run drag method
        UpdateHoldDrag ();
    }
    #endregion

    #region METHODS
    private void Grab ()
    {
        if (grabbedObject)
            grabbedObject = null;
        else {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) {
                if (hit.collider.CompareTag ("FoodObject"))
                    grabbedObject = hit.transform;
            }
        }
    }

    private void Drag ()
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        Vector3 position = transform.position + transform.forward;
        Plane plane = new Plane (-transform.forward, position);
        float distance = 0f;

        if (plane.Raycast (ray, out distance)) {
            var v = ray.origin + ray.direction * distance;
            v.z = 0f;
            grabbedObject.position = v;
        }
    }

    private void UpdateHoldDrag ()
    {
        if (Input.GetMouseButton (0)) {
            if (grabbedObject)
                Drag ();
            else
                Grab ();
        } else 
            grabbedObject = null;
    }
    #endregion
}