using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FoodRecipe : MonoBehaviour
{
    #region MEMBERS
    public delegate void RecipeDone();
    public static event RecipeDone OnRecipeDone;

    public delegate void PushGUIInstruction(string name);
    public static event PushGUIInstruction OnPushInstruction;

    public delegate void PopGUIInstruction();
    public static event PopGUIInstruction OnInstructionDone;

    protected Queue<FoodInstruction> _instructions = new Queue<FoodInstruction>();
    protected FoodHandler _foodHandler;
    #endregion

    #region UNITY_METHODS
    void Awake()
    {
        _foodHandler = GameObject.Find("Food Handler").GetComponent<FoodHandler>();
    }

    void Update()
    {
        var i = GetCurrentInstruction();
        if (i != null)
            i.Update();
        else
        {
            OnRecipeDone();
            Destroy(gameObject);
        }
    }

    void OnGUI()
    {
        Rect r = new Rect(250f, 0f, 200f, 20f);
        foreach (var i in _instructions)
        {
            GUI.Label(r, i.type.ToString());
            r.y += 20f;
        }
    }
    #endregion

    #region METHODS
    // Builds a recipe
    public abstract void Build();

    // If recipe has no more instructions left, recipe is considered done
    public bool IsFinished()
    {
        return (_instructions.Count == 0);
    }

    // Returns instruction in front of the queue
    public FoodInstruction GetCurrentInstruction()
    {
        if (_instructions.Count == 0)
        {
            Debug.LogError("No instructions exist.");
            return null;
        }

        return _instructions.Peek();
    }

    protected void _AddInstruction(FoodInstruction instruction)
    {
        instruction.OnInstructionDone += _RemoveInstruction;
        _instructions.Enqueue(instruction);

        OnPushInstruction(instruction.type.ToString());
    }

    // Everytime instruction is done, this is called once.
    private void _RemoveInstruction()
    {
        if (_instructions.Count > 0) {
            _instructions.Dequeue();

            OnInstructionDone();
        }
        else
            Debug.LogError("Trying to complete non-existant instruction.");
    }
    #endregion
}
