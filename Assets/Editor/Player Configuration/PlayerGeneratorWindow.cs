using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerGeneratorWindow : EditorWindow
{
    static PlayerGeneratorWindow w;
    static GUIStyle mystyle;

    PlayerScriptable PlayerScriptable;
    public string[] DeathType = { "1 touch to dead", "can't die", "When life=0" };
    public int deadIndex;

    public static void OpenWindow()
    {
        PlayerGeneratorWindow a = GetWindow<PlayerGeneratorWindow>("Player Generator", true);
        a.minSize = new Vector2(220f, 210f);
        mystyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        
    }

    private void OnGUI()
    {



        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
       // EditorGUILayout.LabelField("Edit Custom Player", mystyle);
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();


        PlayerScriptable = (PlayerScriptable)EditorGUILayout.ObjectField("Scriptable Player", PlayerScriptable, typeof(PlayerScriptable), false);

        if (PlayerScriptable != null)
        {
            EditorGUILayout.Space();
            PlayerScriptable.name = EditorGUILayout.TextField("Name", PlayerScriptable.name);
            PlayerScriptable.Life = EditorGUILayout.FloatField("Life", PlayerScriptable.Life);
            PlayerScriptable.speed = EditorGUILayout.FloatField("Speed", PlayerScriptable.speed);
            EditorGUILayout.Space();

            PlayerScriptable.VerticalMovement = EditorGUILayout.Toggle("Vertical Movement", PlayerScriptable.VerticalMovement);
            PlayerScriptable.HorizontalMovement = EditorGUILayout.Toggle("Horizontal Movement", PlayerScriptable.HorizontalMovement);


            if (!PlayerScriptable.VerticalMovement)
            {

                PlayerScriptable.CanJump = EditorGUILayout.Toggle("Can Jump", PlayerScriptable.CanJump);
                if (PlayerScriptable.CanJump)
                {
                    PlayerScriptable.jumpForce = EditorGUILayout.FloatField("Jump Force", PlayerScriptable.jumpForce);
                }
            }

            deadIndex = EditorGUILayout.Popup("Death Types", deadIndex, DeathType);
            PlayerScriptable.currentDeath = deadIndex;
            EditorUtility.SetDirty(PlayerScriptable); //para que se guarde los cambios
            if (GUILayout.Button("Crear Scriptable Player"))
            {
                var scriptable = ScriptableObject.CreateInstance<PlayerScriptable>();
                var path = "Assets/Carpeta Gonza/" + PlayerScriptable.name + ".asset";

                path = AssetDatabase.GenerateUniqueAssetPath(path);

                AssetDatabase.CreateAsset(scriptable, path);

                Save();
            }
            if (GUILayout.Button("Crear Player Prefab")) //Ejemplo de como crear un prefab
            {
                var myObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var script=myObject.AddComponent<Player>();
                script.data = PlayerScriptable;
                string path = "Assets/Prefabs/" + PlayerScriptable.name + ".prefab";

                PrefabUtility.SaveAsPrefabAssetAndConnect(myObject, path, InteractionMode.AutomatedAction);

                Save();
            }

        }
        else
        {
            if (GUILayout.Button("Nuevo Scriptable Player"))
            {
                PlayerScriptable = new PlayerScriptable();
                PlayerScriptable.name = "New Player";
            }
        }
    }

    private void Save()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
