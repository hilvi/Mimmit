using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientQueueScript : MonoBehaviour
{
    public Rect currentActionRect;
    public Vector2 queueAnchor;

    private bool slidingInAction = false;
    private List<Rect> actionList = new List<Rect>();

    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            Rect r = new Rect(queueAnchor.x, queueAnchor.y, 60f, 60f);
            r.x -= (i * 60) + (i * 10);
            actionList.Add(r);
        }
    }

    void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(SlideCurrentActionOut());
            if (!slidingInAction)
            {
                if (actionList.Count > 0)
                    StartCoroutine(SlideNextActionIn());
            }
        }
    }

    void OnGUI()
    {
        GUI.Box(currentActionRect, "current");

        int index = 0;
        foreach (Rect r in actionList)
        {
            GUI.Box(r, (index++).ToString());
        }
    }

    private IEnumerator SlideCurrentActionOut()
    {
        // This co-routine is safe to call, because it will end immediately
        // if there is nothing to do.

        // Calculate movement ratio, so objects move at same pace
        float dy = Mathf.Abs(-100f - currentActionRect.y);
        while (true)
        {
            // Move object away
            currentActionRect.y = Mathf.MoveTowards(currentActionRect.y, -100f, Time.deltaTime * dy * 2f);

            // If done, terminate
            if (currentActionRect.y == -100f)
                break;

            yield return null;
        }
    }

    private IEnumerator SlideNextActionIn()
    {
        /* 
         * Prevent other coroutines if this is busy
         * This is very important, because we don't want 
         * multiple coroutines partying in the same list
         */
        slidingInAction = true;

        // These values set movement ratios, so everything will be done in exactly the same time
        Rect t = actionList[0];
        float dx = (760f - t.x);
        float dw = (90f - t.width);
        float dh = (90f - t.height);

        // Pre-calculate new positions for every node other than big one
        float[] newX = new float[actionList.Count - 1];
        for (int i = 1; i < actionList.Count; i++)
        {
            newX[i - 1] = actionList[i].x + 70f;
        }

        while (true)
        {
            // First value will move and also expand its size
            Rect r = actionList[0];
            r.x = Mathf.MoveTowards(r.x, 760f, Time.deltaTime * dx);
            r.width = Mathf.MoveTowards(r.width, 90f, Time.deltaTime * dw);
            r.height = Mathf.MoveTowards(r.height, 90f, Time.deltaTime * dh);
            actionList[0] = r;

            // Update the rest at same pace
            for (int i = 1; i < actionList.Count; i++)
            {
                Rect rr = actionList[i];
                rr.x = Mathf.MoveTowards(rr.x, newX[i - 1], Time.deltaTime * dx);
                actionList[i] = rr;
            }

            // If done, terminate
            if (r.x == 760f)
                break;

            yield return null;
        }

        // Change currently active action
        currentActionRect = actionList[0];
        actionList.RemoveAt(0);

        slidingInAction = false;
    }
}
