using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodGameManager : GameManager
{
    #region MEMBERS
    public GameObject musicObject;
    public AudioClip music;

    [Range(1, 20)]
    public float cameraScrollingSpeed;
    public Rect leftScrollArrow, rightScrollArrow;
    private enum CameraState { Left, Center, Right }
    private CameraState currentCameraState;
    private bool cameraIsMoving;

    private Transform grabbedObject;

    private enum ActionState { Idle, VerticalWaggle, HorizontalWaggle, RepeatClick }
    private ActionState currentActionState;
    private Rect actionStateLabelRect;

    private bool horizontalWaggleEnabled;
    private bool verticalWaggleEnabled;
    private bool repeatClickEnabled;

    private IngredientManager ingredientManager;
    private PreparationTableScript prepTable;
    private ActionQueueScript actionQueue;
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

        currentActionState = ActionState.Idle;
        repeatClickEnabled = false;
        horizontalWaggleEnabled = false;

        // Developer 
        actionStateLabelRect = new Rect(300, 0, 100, 20);

        prepTable = GameObject.Find("Preparing Table").GetComponent<PreparationTableScript>();
        if (prepTable == null)
            Debug.LogError("Couldn't find preparing table!");

        ingredientManager = GameObject.Find("Ingredient Container").GetComponent<IngredientManager>();
        if (ingredientManager == null)
            Debug.LogError("Couldn't find ingredient mgr!");

        actionQueue = GameObject.Find("Action Queue").GetComponent<ActionQueueScript>();
        if (actionQueue == null)
            Debug.LogError("Couldn't find action queue!");
    }

    void Update()
    {
        UpdateActionState();

        // Run drag method
        switch (currentActionState)
        {
            case ActionState.Idle:
                HandleIdleState();
                break;
            case ActionState.VerticalWaggle:
                HandleVerticalWaggleState();
                break;
            case ActionState.HorizontalWaggle:
                HandleHorizontalWaggleState();
                break;
            case ActionState.RepeatClick:
                HandleRepeatClickState();
                break;
        }
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

        GUI.Label(actionStateLabelRect, currentActionState.ToString());
    }
    #endregion

    #region METHODS
    private void HandleIdleState()
    {
        UpdateHoldDrag();
    }
    private void HandleHorizontalWaggleState()
    {
        if (!horizontalWaggleEnabled)
        {
            horizontalWaggleEnabled = true;
            StartCoroutine(HorizontalWaggler(10));
        }
    }

    private void HandleVerticalWaggleState()
    {
        if (!verticalWaggleEnabled)
        {
            verticalWaggleEnabled = true;
            StartCoroutine(VerticalWaggler(10));
        }
    }

    private void HandleRepeatClickState()
    {
        if (!repeatClickEnabled)
        {
            repeatClickEnabled = true;
            StartCoroutine(ClickCounter(10));
        }
    }

    private void UpdateActionState()
    {
        // Developer/debugger controls
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentActionState = ActionState.Idle;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            currentActionState = ActionState.HorizontalWaggle;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            currentActionState = ActionState.VerticalWaggle;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            currentActionState = ActionState.RepeatClick;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Cutting experiment, integrate this inside each food object
            var foodOnTable = prepTable.GetFoodOnTable();
            foreach (IFoodObject food in foodOnTable)
            {
                var type = food.GetFoodType();
                if (type == FoodType.Apple)
                {
                    var apple = (AppleFoodObject)food;
                    if (apple.State == FoodState.Full)
                    {
                        // Spawn new apple and set its properties
                        var p1 = apple.transform.position;
                        p1.x += 0.5f;
                        var newApple = ingredientManager.SpawnFood(FoodType.Apple, p1).
                            GetComponent(typeof(IFoodObject)) as AppleFoodObject;
                        newApple.State = FoodState.Half;
                        newApple.transform.localRotation = Quaternion.Euler(0, 0, 90);

                        // Modify original apple properties
                        var p2 = apple.transform.position;
                        p2.x -= 0.5f;
                        apple.transform.position = p2;
                        apple.State = FoodState.Half;
                        apple.transform.localRotation = Quaternion.Euler(0, 0, 270);
                    }
                    else if (apple.State == FoodState.Half)
                    {
                        // Spawn new apple and set its properties
                        var p1 = apple.transform.position;
                        p1.x += 0.5f;
                        var newApple = ingredientManager.SpawnFood(FoodType.Apple, p1).
                            GetComponent(typeof(IFoodObject)) as AppleFoodObject;
                        newApple.State = FoodState.Quarter;
                        newApple.transform.localRotation = Quaternion.Euler(0, 0, 90);

                        // Modify original apple properties
                        var p2 = apple.transform.position;
                        p2.x -= 0.5f;
                        apple.transform.position = p2;
                        apple.State = FoodState.Quarter;
                        apple.transform.localRotation = Quaternion.Euler(0, 0, 270);
                    }
                }
            }
        }
    }

    private IEnumerator HorizontalWaggler(int maxHits)
    {
        int hits = 0;
        bool moveToLeft = true;
        while (true)
        {
            // Get cursor position and offset horizontal origin to center of screen
            // If mouseX is positive, cursor is on right side
            // If mouseX is negative, cursor is on left side
            var mousePos = Input.mousePosition;
            float mouseX = mousePos.x - Screen.width / 2f;

            if (moveToLeft)
            {
                if (mouseX < 0f)
                {
                    hits++;
                    moveToLeft = false;
                    Debug.Log(hits + "/" + maxHits);
                }
            }
            else
            {
                if (mouseX > 0f)
                {
                    hits++;
                    moveToLeft = true;
                    Debug.Log(hits + "/" + maxHits);
                }
            }

            // Stop after max hits
            if (hits == maxHits)
                break;

            yield return null;
        }

        horizontalWaggleEnabled = false;
        currentActionState = ActionState.Idle; // Swap back to idle
    }

    private IEnumerator VerticalWaggler(int maxHits)
    {
        int hits = 0;
        bool moveUp = true;
        while (true)
        {
            // Get cursor position and offset horizontal origin to center of screen
            // If mouseY is positive, cursor is on down side
            // If mouseY is negative, cursor is on up side
            var mousePos = Input.mousePosition;
            float mouseY = Screen.height - (mousePos.y + Screen.height / 2f);
            if (moveUp)
            {
                if (mouseY < 0f)
                {
                    hits++;
                    moveUp = false;
                    Debug.Log(hits + "/" + maxHits);
                }
            }
            else
            {
                if (mouseY > 0f)
                {
                    hits++;
                    moveUp = true;
                    Debug.Log(hits + "/" + maxHits);
                }
            }

            // Stop after max hits
            if (hits == maxHits)
                break;

            yield return null;
        }

        horizontalWaggleEnabled = false;
        currentActionState = ActionState.Idle; // Swap back to idle
    }

    private IEnumerator ClickCounter(int maxClicks)
    {
        int clicks = 0;
        while (true)
        {
            // Count clicks until enough
            if (Input.GetMouseButtonDown(0))
            {
                clicks++;
                Debug.Log("Clicks " + clicks + "/" + maxClicks);
            }

            if (clicks == maxClicks)
                break;

            yield return null;
        }

        repeatClickEnabled = false;
        currentActionState = ActionState.Idle; // Swap back to idle
    }

    private IEnumerator CameraMoveTowards(CameraState cameraState)
    {
        Vector3 camPosition = Camera.main.transform.position;
        // Determine where the camera should go
        float targetX = 0f;
        switch (cameraState)
        {
            case CameraState.Left: targetX = -5f; break;
            case CameraState.Center: targetX = 0f; break;
            case CameraState.Right: targetX = 5f; break;
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