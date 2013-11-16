using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodGameManager : GameManager
{
    #region MEMBERS
    public GameObject musicObject;
    public AudioClip music;

    [Range(1, 20)] public float cameraScrollingSpeed;
    public Rect leftScrollArrow, rightScrollArrow;
    private enum CameraState { Left, Center, Right }
    private CameraState currentCameraState;
    private bool cameraIsMoving;

    private Transform grabbedObject;
    #endregion

    #region UNITY_METHODS
    public override void Start()
    {
        // Boilerplate
        base.Start();
        SetGameState(GameState.Running);

        if (InGameMenuGUI.music == null)
        {
            InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
            InGameMenuGUI.music.audio.clip = music;
            InGameMenuGUI.music.audio.Play();
        }

        currentCameraState = CameraState.Center;
        cameraIsMoving = false;
    }

    void Update()
    {
        // Run drag method
        UpdateHoldDrag();
    }

    void OnGUI()
    {
        if (GUI.Button(leftScrollArrow, "left") && !cameraIsMoving)
        {
            if (currentCameraState == CameraState.Center)
            {
                currentCameraState = CameraState.Left;
                StartCoroutine(CameraMoveTowards(currentCameraState));
            }
            else if (currentCameraState == CameraState.Right)
            {
                currentCameraState = CameraState.Center;
                StartCoroutine(CameraMoveTowards(currentCameraState));
            }
        }
        if (GUI.Button(rightScrollArrow, "right") && !cameraIsMoving)
        {
            if (currentCameraState == CameraState.Center)
            {
                currentCameraState = CameraState.Right;
                StartCoroutine(CameraMoveTowards(currentCameraState));
            }
            else if (currentCameraState == CameraState.Left)
            {
                currentCameraState = CameraState.Center;
                StartCoroutine(CameraMoveTowards(currentCameraState));
            }
        }
    }
    #endregion

    #region METHODS
    private IEnumerator CameraMoveTowards(CameraState cameraState)
    {
        Debug.Log("start");
        Vector3 camPosition = Camera.main.transform.position;
        // Determine where the camera should go
        float targetX = 0f;
        switch (cameraState)
        {
            case CameraState.Left: targetX = -5f;  break;
            case CameraState.Center: targetX = 0f;  break;
            case CameraState.Right: targetX = 5f;  break;
        }

        // Prevent accidental clicks
        cameraIsMoving = true;

        // Calculate new position
        Vector3 targetPosition = new Vector3(targetX, camPosition.y, camPosition.z);
        while (true)
        {
            // Move towards target position
            camPosition = Vector3.MoveTowards(camPosition, targetPosition, Time.deltaTime * cameraScrollingSpeed);
            Camera.main.transform.position = camPosition;

            // Exit loop when arrived
            if (camPosition == targetPosition) 
                break;

            yield return null;
        }

        cameraIsMoving = false;

        yield return null;
    }

    private void Grab()
    {
        if (grabbedObject)
            grabbedObject = null;
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("FoodObject"))
                    grabbedObject = hit.transform;
            }
        }
    }

    private void Drag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 position = transform.position + transform.forward;
        Plane plane = new Plane(-transform.forward, position);
        float distance = 0f;

        if (plane.Raycast(ray, out distance))
        {
            var v = ray.origin + ray.direction * distance;
            v.z = 0f;
            grabbedObject.position = v;
        }
    }

    private void UpdateHoldDrag()
    {
        if (Input.GetMouseButton(0))
        {
            if (grabbedObject)
                Drag();
            else
                Grab();
        }
        else
            grabbedObject = null;
    }
    #endregion
}