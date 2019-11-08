using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerGeneratorWindow : EditorWindow
{
    static PlayerGeneratorWindow w;
    static GUIStyle mystyle;

    PlayerScriptable playerScriptable;
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

        playerScriptable = (PlayerScriptable)EditorGUILayout.ObjectField("Scriptable Player", playerScriptable, typeof(PlayerScriptable), false);
       
        EditSelectedPlayer();


        if (playerScriptable != null)
        {
            EditorGUILayout.Space();
            playerScriptable.name = EditorGUILayout.TextField("Name", playerScriptable.name);
            playerScriptable.Life = EditorGUILayout.FloatField("Life", playerScriptable.Life);
            playerScriptable.speed = EditorGUILayout.FloatField("Speed", playerScriptable.speed);
            EditorGUILayout.Space();

            playerScriptable.VerticalMovement = EditorGUILayout.Toggle("Vertical Movement", playerScriptable.VerticalMovement);
            playerScriptable.HorizontalMovement = EditorGUILayout.Toggle("Horizontal Movement", playerScriptable.HorizontalMovement);

            playerScriptable.MeleAttack = EditorGUILayout.Toggle("Melee Attack", playerScriptable.MeleAttack);

            if (!playerScriptable.VerticalMovement)
            {

                playerScriptable.CanJump = EditorGUILayout.Toggle("Can Jump", playerScriptable.CanJump);
                if (playerScriptable.CanJump)
                {
                    playerScriptable.jumpForce = EditorGUILayout.FloatField("Jump Force", playerScriptable.jumpForce);
                }
            }

            deadIndex = EditorGUILayout.Popup("Death Types", deadIndex, DeathType);
            playerScriptable.currentDeath = deadIndex;
            EditorUtility.SetDirty(playerScriptable); //para que se guarde los cambios
            if (GUILayout.Button("Create Scriptable Player"))
            {
                var scriptable = CreateInstance<PlayerScriptable>();
                if (!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", "Data");
                    Debug.Log("The introduced folder doesn't exist, so I just created a default one for you.");
                    AssetDatabase.Refresh();
                }
                var path = "Assets/Resources/Data/" + playerScriptable.name + ".asset";

                path = AssetDatabase.GenerateUniqueAssetPath(path);

                AssetDatabase.CreateAsset(scriptable, path);

                Save();
            }
            if (GUILayout.Button("Save has Player Prefab")) 
            {
                var myObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var script = myObject.AddComponent<Player>();
                script.data = playerScriptable;
                string path = "Assets/Resources/Prefabs/" + playerScriptable.name + ".prefab";
                Debug.Log("the prefab was saved in " + path);

                PrefabUtility.SaveAsPrefabAssetAndConnect(myObject, path, InteractionMode.AutomatedAction);

                Save();
            }

        }
        else
        {
            if (GUILayout.Button("Save Scriptable Player"))
            {
                EditNewPlayer();
            }
        }
    }

    void EditSelectedPlayer()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.tag == "Player")
        {
            if (GUILayout.Button("Edit Selected Player"))
            {
                var activePlayer = Selection.activeGameObject.GetComponent<Player>();
                playerScriptable = activePlayer.data;
            }
           
        }
    } 
    void EditNewPlayer()
    {
        playerScriptable = CreateInstance<PlayerScriptable>();
        playerScriptable.name = "New Player";
    }

    private void Save()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
