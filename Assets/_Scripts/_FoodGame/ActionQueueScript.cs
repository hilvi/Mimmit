using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionQueueScript : MonoBehaviour
{
    public class ActionIcon
    {
        public Rect rect, nextRect;
        public Texture texture;
        public string name;
        public ActionIcon(Rect rect)
        {
            this.rect = rect;
            this.texture = null;
            this.name = "null";
        }

        public void SetRect(Rect rect)
        {
            this.rect = rect;
        }
    }

    public Rect currentActionRect;
    public Vector2 queueAnchor;

    private bool slidingInAction = false;
    private List<ActionIcon> actionList = new List<ActionIcon>();

    // Action icon textures
    public Texture cutApple, cutBanana, cutPear;
    public Texture putApple, putBanana, putPear;

    void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.F1))
            Slide();
    }

    void OnGUI()
    {
        foreach (ActionIcon a in actionList)
            GUI.DrawTexture(a.rect, a.texture);
    }

    public void PushActionToQueue(string action)
    {
        // Calculate rect based on current queue length
        Rect iconRect = new Rect(queueAnchor.x, queueAnchor.y, 60f, 60f);
        iconRect.x -= ((actionList.Count - 1) * 60f) + ((actionList.Count - 1) * 10f);

        // If this is first action on list, override it with different settings
        if (actionList.Count == 0)
            iconRect = new Rect(currentActionRect);

        if (action == "Put Apple")
        {
            ActionIcon ai = new ActionIcon(iconRect);
            ai.texture = putApple;
            ai.name = action;
            actionList.Add(ai);

        }
        else if (action == "Put Banana")
        {

        }
        else if (action == "Put Pear")
        {

        }
        else if (action == "Cut Apple")
        {
            ActionIcon ai = new ActionIcon(iconRect);
            ai.texture = cutApple;
            ai.name = action;
            actionList.Add(ai);
        }
        else if (action == "Cut Banana")
        {

        }
        else if (action == "Cut Pear")
        {

        }
        else
        {
            Debug.LogError("Unknown action is being inserted to action queue");
            return;
        }
    }

    public void Slide()
    {
        if (!slidingInAction)
        {
            if (actionList.Count > 0)
                StartCoroutine(SlideCurrentActionOut());
            if (actionList.Count > 1)
                StartCoroutine(SlideNextActionIn());
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
            // Move object up
            actionList[0].rect.y = 
                Mathf.MoveTowards(actionList[0].rect.y, -100f, Time.deltaTime * dy * 4f);

            // If done, terminate
            if (actionList[0].rect.y == -100f)
                break;

            yield return null;
        }

        // Wait until sliding is done before removing first element
        while (slidingInAction)
            yield return null;

        // Remove first element
        actionList.RemoveAt(0);
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
        Rect t = actionList[1].rect;
        float dx = (760f - t.x) * 2;
        float dw = (90f - t.width) * 2;
        float dh = (90f - t.height) * 2;

        // Pre-calculate new positions for every node other than big one
        float[] newX = new float[actionList.Count - 1];
        for (int i = 2; i < actionList.Count; i++)
        {
            newX[i - 1] = actionList[i].rect.x + 70f;
        }

        while (true)
        {
            // First value will move and also expand its size
            Rect r = actionList[1].rect;
            r.x = Mathf.MoveTowards(r.x, 760f, Time.deltaTime * dx);
            r.width = Mathf.MoveTowards(r.width, 90f, Time.deltaTime * dw);
            r.height = Mathf.MoveTowards(r.height, 90f, Time.deltaTime * dh);
            actionList[1].SetRect(r);

            // Update the rest at same pace
            for (int i = 2; i < actionList.Count; i++)
            {
                Rect rr = actionList[i].rect;
                rr.x = Mathf.MoveTowards(rr.x, newX[i - 1], Time.deltaTime * dx);
                actionList[i].SetRect(rr);
            }

            // If done, terminate
            if (r.x == 760f)
                break;

            yield return null;
        }

        slidingInAction = false;
    }
}
