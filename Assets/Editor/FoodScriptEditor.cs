using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FoodGameManager))]
public class FoodScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FoodGameManager __fgm = (FoodGameManager)target;

        GUILayout.BeginVertical("box"); // Begin Outer-most block
        GUILayout.Label("State Switchers - current: " + __fgm.GetActionState());
        GUILayout.BeginHorizontal("label");
        if (GUILayout.Button("Idle"))
            __fgm.SetActionState(FoodGameManager.ActionState.Idle);

        if (GUILayout.Button("Rep.Click"))
            __fgm.SetActionState(FoodGameManager.ActionState.RepeatClick);

        if (GUILayout.Button("H.Waggle"))
            __fgm.SetActionState(FoodGameManager.ActionState.HorizontalWaggle);

        if (GUILayout.Button("V.Waggle"))
            __fgm.SetActionState(FoodGameManager.ActionState.VerticalWaggle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("label");
        if (GUILayout.Button("CW Rot")) { }
        if (GUILayout.Button("CCW Rot")) { }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("label");
        if (GUILayout.Button("L.Swipe")) { }
        if (GUILayout.Button("R.Swipe")) { }
        if (GUILayout.Button("U.Swipe")) { }
        if (GUILayout.Button("D.Swipe")) { }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical(); // End Outer-most block
    }
}
