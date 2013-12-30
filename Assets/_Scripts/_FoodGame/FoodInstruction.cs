using UnityEngine;
using System.Collections;

[System.Serializable]
public class FoodInstruction
{
    #region MEMBERS
    public enum Type { MoveFood, SliceFood, PickKnife }
    public Type type;
    public FoodObject target = null;
    public FoodObject tool = null;

    public delegate void InstructionDone();
    public event InstructionDone OnInstructionDone;

    private FoodAnimation _anim = null;

    private int _frame = 0;
    private bool _runningVisualAction = false;

    private int _counter = 0;
    private int _triggerCount = 0;
    #endregion

    public FoodInstruction(Type type, FoodObject target)
    {
        this.type = type;
        this.target = target;

        switch (type)
        {
            case Type.MoveFood:
            case Type.PickKnife:
                _triggerCount = 1;
                target.OnClicked += _Trigger;
                break;
        }
    }

    public FoodInstruction(Type type, FoodObject target, FoodObject tool)
    {
        this.type = type;
        this.target = target;
        this.tool = tool;

        switch (type)
        {
            case Type.SliceFood:
                _triggerCount = 2;
                break;
        }
    }

    public void Update()
    {
        if (_runningVisualAction)
        {
            // Update visual action
            _VisualAction();

            // Return early, because we don't care about user input from this point on
            return;
        }

        // If we don't have target set, throw error
        if (target == null)
        {
            Debug.LogError("No target on instruction");
            return;
        }

        _HandleInstruction(type);
    }

    public void AddAnimation(FoodAnimation anim)
    {
        _anim = anim;
    }

    private void _VisualAction()
    {
        // Visual action only lasts for x amount of time
        _frame++;

        if (type == Type.MoveFood)
        {
            // Move object to destination
            Vector3 __p = target.transform.position;
            target.transform.position = Vector3.Lerp(__p, new Vector3(0f, -2.75f, 0f), _frame / 60f);

            if (_frame > 60)
            {
                // When visual action is done, trigger Recipe to pull out new instruction
                _runningVisualAction = false;
                OnInstructionDone();
            }
        }
        else if (type == Type.SliceFood)
        {
            if (target != null)
            {
                target.SelfDestruct();
                target = null;
            }

            if (_anim != null)
                _anim.Begin();

            if (_frame > 120)
            {
                _runningVisualAction = false;
                OnInstructionDone();
            }
        }
        else if (type == Type.PickKnife)
        {
            _runningVisualAction = false;
            OnInstructionDone();
        }
    }

    private void _HandleInstruction(Type type)
    {
        switch (type)
        {
            case Type.MoveFood:
                _HandleMove();
                break;
            case Type.SliceFood:
                _HandleSlice();
                break;
            case Type.PickKnife:
                _HandlePick();
                break;
        }
    }

    
    private void _HandleMove()
    {
        if (_counter == _triggerCount)
        {
            _runningVisualAction = true;
        }
    }

    private bool _overlapping = false;
    private void _HandleSlice()
    {
        if (target != null && tool != null)
        {
            bool __overlapping = (target.renderer.bounds.Intersects(tool.renderer.bounds));
            if (_overlapping != __overlapping)
            {
                _overlapping = __overlapping;
                _counter++;
            }
        }

        // If we have fulfilled required number of actions
        if (_counter == _triggerCount)
        {
            _runningVisualAction = true;
        }
    }

    
    private void _HandlePick()
    {
        if (_counter == _triggerCount)
        {
            target.isFollowingCursor = true;
            _runningVisualAction = true;
        }
    }

    private void _Trigger()
    {
        _counter++;
    }
}
