using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GameModeProperties))]
public class ScriptableEditor : Editor
{

    GameModeProperties _target;

    private void OnEnable()
    {
        _target = (GameModeProperties)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Game Mode: ");
        EditorGUILayout.EnumPopup(_target.gm);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Perspective: ");
        EditorGUILayout.EnumPopup(_target.pers);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if(_target.gm == GameMode.endless)
        {
        EditorGUILayout.LabelField("Objective ");
            EditorGUILayout.EnumPopup(_target.objEndless);
        }else if(_target.gm == GameMode.survival)
        {
        EditorGUILayout.LabelField("Objective ");
            EditorGUILayout.EnumPopup(_target.objSurvival);
        }
        else if (_target.gm == GameMode.platform)
        {
        EditorGUILayout.LabelField("Objective ");
            EditorGUILayout.EnumPopup(_target.objPlatform);
        }
        EditorGUILayout.EndHorizontal();
        



       
    }

}


