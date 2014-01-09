using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstructionGUIQueue : MonoBehaviour
{
    public class InstructionIcon
    {
        public Rect rect, nextRect;
        public Texture texture;
        public string name;
        public InstructionIcon(Rect rect)
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

    public Rect activeInstructionRect;
    public Vector2 queueAnchor;

    private bool slidingInInstruction = false;
    private List<InstructionIcon> instructionQueue = new List<InstructionIcon>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            PushInstructionToQueue("Put Apple");
        if (Input.GetKeyDown(KeyCode.F2))
            Slide();
    }

    void OnGUI()
    {
        foreach (InstructionIcon a in instructionQueue)
            GUI.Box(a.rect, a.name);
    }

    void OnEnable()
    {
        FoodRecipe.OnPushInstruction += PushInstructionToQueue;
        FoodRecipe.OnInstructionDone += Slide;
    }

    void OnDisable()
    {
        FoodRecipe.OnPushInstruction -= PushInstructionToQueue;
        FoodRecipe.OnInstructionDone -= Slide;
    }

    public void PushInstructionToQueue(string instruction)
    {
        // Calculate rect based on current queue length
        Rect iconRect = new Rect(queueAnchor.x, queueAnchor.y, 60f, 60f);
        iconRect.x -= ((instructionQueue.Count - 1) * 60f) + ((instructionQueue.Count - 1) * 10f);

        // If this is first instruction on list, override it with different settings
        if (instructionQueue.Count == 0)
            iconRect = new Rect(activeInstructionRect);

        InstructionIcon ii = new InstructionIcon(iconRect);
        ii.name = instruction;
        instructionQueue.Add(ii);
    }

    public void Slide()
    {
        if (!slidingInInstruction)
        {
            if (instructionQueue.Count > 0)
                StartCoroutine(SlideCurrentInstructionOut());
            if (instructionQueue.Count > 1)
                StartCoroutine(SlideNextInstructionIn());
        }
    }

    private IEnumerator SlideCurrentInstructionOut()
    {
        // This co-routine is safe to call, because it will end immediately
        // if there is nothing to do.

        // Calculate movement ratio, so objects move at same pace
        float dy = Mathf.Abs(-100f - activeInstructionRect.y);

        while (true)
        {
            // Move object up
            instructionQueue[0].rect.y =
                Mathf.MoveTowards(instructionQueue[0].rect.y, instructionQueue[0].rect.height * -2f, Time.deltaTime * dy * 4f);

            // If done, terminate
            if (instructionQueue[0].rect.y < -activeInstructionRect.height)
                break;

            yield return null;
        }

        // Wait until sliding is done before removing first element
        while (slidingInInstruction)
            yield return null;

        // Remove first element
        instructionQueue.RemoveAt(0);
    }

    private IEnumerator SlideNextInstructionIn()
    {
        /* 
         * Prevent other coroutines if this is busy
         * This is very important, because we don't want 
         * multiple coroutines partying in the same list
         */
        slidingInInstruction = true;

        // These values set movement ratios, so everything will be done in exactly the same time
        Rect t = instructionQueue[1].rect;
        float dx = (activeInstructionRect.x - t.x) * 2;
        float dw = (activeInstructionRect.width - t.width) * 2;
        float dh = (activeInstructionRect.height - t.height) * 2;

        // Pre-calculate new positions for every node other than big one
        float[] newX = new float[instructionQueue.Count - 1];
        for (int i = 2; i < instructionQueue.Count; i++)
        {
            newX[i - 1] = instructionQueue[i].rect.x + 70f;
        }

        while (true)
        {
            // First value will move and also expand its size
            Rect r = instructionQueue[1].rect;
            r.x = Mathf.MoveTowards(r.x, activeInstructionRect.x, Time.deltaTime * dx);
            r.width = Mathf.MoveTowards(r.width, activeInstructionRect.width, Time.deltaTime * dw);
            r.height = Mathf.MoveTowards(r.height, activeInstructionRect.height, Time.deltaTime * dh);
            instructionQueue[1].SetRect(r);

            // Update the rest at same pace
            for (int i = 2; i < instructionQueue.Count; i++)
            {
                Rect rr = instructionQueue[i].rect;
                rr.x = Mathf.MoveTowards(rr.x, newX[i - 1], Time.deltaTime * dx);
                instructionQueue[i].SetRect(rr);
            }

            // If done, terminate
            if (r.x == activeInstructionRect.x)
                break;

            yield return null;
        }

        slidingInInstruction = false;
    }
}
