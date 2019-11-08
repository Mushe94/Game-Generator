using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
[CustomEditor (typeof(GameManager))]
public class GameManagerInspector : Editor
{
    GameManager _target;
    bool scenestoogle;
    GUIStyle titlestyle;
    GUIStyle subtitlestyle;
    GUIStyle choosestyle;
    private void OnEnable()
    {
        _target = (GameManager)target;

        titlestyle = new GUIStyle
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter

        };

        subtitlestyle = new GUIStyle
        {
            fontSize = 12,
            fontStyle = FontStyle.Italic,
            alignment = TextAnchor.MiddleLeft
        };

        choosestyle = new GUIStyle
        {
            fontSize = 10,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField("Manager", titlestyle);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Amount Of Scenes",subtitlestyle);
        _target.amountScenes = EditorGUILayout.IntField(_target.amountScenes);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Time For Round",subtitlestyle);
        _target.amountOfTime = EditorGUILayout.FloatField(_target.amountOfTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Amount Of Enemies In This Level",subtitlestyle);
        _target.enemies = EditorGUILayout.IntField(_target.enemies);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.BeginVertical();
        scenestoogle = EditorGUILayout.BeginToggleGroup("Levels", scenestoogle);
        for (int i = 0; i < _target.myscenes.Length; i++)
        {
            EditorGUILayout.LabelField("Scene Name: " + _target.myscenes[i].name,subtitlestyle);
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
       if(GUILayout.Button("Get Scenes"))
        {   
            _target.myscenes = new UnityEngine.SceneManagement.Scene[_target.amountScenes];
            Debug.Log(_target.amountScenes);
            for (int i = 0; i < _target.amountScenes; i++)
            {
                _target.myscenes[i] = EditorSceneManager.GetSceneByPath("Assets/Resources/Prefabs/Level" + i + ".unity");
                Debug.Log(_target.myscenes[i].name);
                Repaint();
            }
        }
    }
}
