using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class LevelConfigurationWindow : EditorWindow
{
    private GUIStyle _style;
    private GUIStyle _secondaryStyle;

    private LevelConfigurationData scriptable;
    private GameObject powerUp;
    private string powerUpName;

    private int[] powerUpsCopies = { 0, 0, 0 };
    private string[] allPowerups = { "Damage", "Health", "Speed" };
    private string currentPowerup;
    private int powerupsIndex;

    private List<string> spawnedEmptyName = new List<string>();
    private string[] allEmpty;
    private string currentEmpty;
    private int emptyIndex;

    private GameObject previewObject;
    private bool showPowerUpsPreview;
    private AnimBool _animEmpty;
    private AnimBool _animObject;

    public static void OpenWindow()
    {
        GetWindow<LevelConfigurationWindow>();

    }

    //private void OnEnable()
    //{
    //    _style = new GUIStyle()
    //    {
    //        fontSize = 15,
    //        fontStyle = FontStyle.Bold,
    //        alignment = TextAnchor.MiddleCenter
    //    };
    //    _animEmpty = new AnimBool(false);
    //    _animEmpty.valueChanged.AddListener(Repaint);
    //    _animObject = new AnimBool(false);
    //    _animObject.valueChanged.AddListener(Repaint);

    //    _secondaryStyle = new GUIStyle()
    //    {
    //        fontSize = 13,
    //        fontStyle = FontStyle.Bold
    //    };

    //    if (scriptable == null)
    //    {
    //        scriptable = Resources.Load<LevelConfigurationData>("LevelConfigurationData");
    //    }


    //    //if (scriptable.emptyCreated.Count > 0)
    //    //{
    //    //    foreach (var name in scriptable.emptyCreated)
    //    //    {
    //    //        var b = GameObject.Find(name);
    //    //        if (b == null)
    //    //        {
    //    //            scriptable.emptyCreated.Remove(name);
    //    //        }
    //    //    }
    //    //    spawnedEmptyName = scriptable.emptyCreated;
    //    //}

    //}

    //private void OnGUI()
    //{
       

    //}
    //private void DrawEnemiesParameters()
    //{

    //}

    //private void DrawPowerUpsParameters()
    //{
    //    EditorGUILayout.Space();

    //    EditorGUILayout.LabelField("Level Configuration", _style);
    //    EditorGUILayout.Space();

    //    EditorGUILayout.LabelField("PowerUps", _secondaryStyle);
    //    EditorGUILayout.Space();

    //    previewObject = (GameObject)EditorGUILayout.ObjectField("Preview Object", previewObject, typeof(GameObject), false);

    //    var normalColor = GUI.backgroundColor;

    //    if (previewObject != null)
    //    {
    //        if (!showPowerUpsPreview)
    //        {
    //            GUI.backgroundColor = new Color(255, 0, 0, 0.4f);
    //        }
    //        else
    //        {
    //            GUI.backgroundColor = Color.green;

    //        }
    //        showPowerUpsPreview = EditorGUILayout.Toggle("Show preview", showPowerUpsPreview);
    //    }

    //    GUI.backgroundColor = normalColor;
    //    EditorGUILayout.Space();

    //    powerupsIndex = EditorGUILayout.Popup("Power Ups Types", powerupsIndex, allPowerups);
    //    currentPowerup = allPowerups[powerupsIndex];

    //    EditorGUILayout.Space();

    //    _animEmpty.target = EditorGUILayout.Toggle("Use Empty Object", _animEmpty.target);

    //    if (EditorGUILayout.BeginFadeGroup(_animEmpty.faded))
    //    {
    //        if (GUILayout.Button("SpawnPowerUp", GUILayout.Width(200)))
    //        {
    //            for (int i = 0; i < scriptable.gameobjectsPreview.Count; i++)
    //            {
    //                PowerUp c = scriptable.gameobjectsPreview[i].GetComponent<PowerUp>();

    //                if (c)
    //                {
    //                    GameObject a = scriptable.gameobjectsPreview[i];
    //                    a.name = "";
    //                    a.name = "PowerUp - " + currentPowerup;

    //                    if (spawnedEmptyName.Contains(a.name))
    //                    {
    //                        if (currentPowerup == "Damage")
    //                        {
    //                            powerUpsCopies[0]++;
    //                            a.name = "PowerUp - " + currentPowerup + " (" + powerUpsCopies[0] + ")";
    //                        }
    //                        else if (currentPowerup == "Health")
    //                        {
    //                            powerUpsCopies[1]++;
    //                            a.name = "PowerUp - " + currentPowerup + " (" + powerUpsCopies[1] + ")";
    //                        }
    //                        else
    //                        {
    //                            powerUpsCopies[2]++;
    //                            a.name = "PowerUp - " + currentPowerup + " (" + powerUpsCopies[2] + ")";
    //                        }
    //                    }

    //                    PrefabUtility.InstantiatePrefab(a);

    //                    spawnedEmptyName.Add(a.name);
    //                    if (showPowerUpsPreview)
    //                    {
    //                        GameObject b = new GameObject();
    //                        b.AddComponent<Renderer>();
    //                        //var mat = new Material(Shader.Find("TransparencyShader"));
    //                        //mat.SetFloat("_Opacity", 0.4f);
    //                        //mat.SetColor("_Color", new Color(0, 1, 1));
    //                        //b.GetComponent<Renderer>().sharedMaterial = mat;
    //                        //b.transform.position = a.transform.position;
    //                        //var follow = b.GetComponent<Follow>();
    //                        //if (follow == null)
    //                        //{
    //                        //    b.AddComponent<Follow>();
    //                        //    follow = b.GetComponent<Follow>();
    //                        //}
    //                        //follow.parent = a.transform;
    //                        //b.transform.parent = a.transform;
    //                    }
    //                }

    //            }
    //        }

    //    }

    //    EditorGUILayout.EndFadeGroup();
    //    EditorGUILayout.Space();

    //    _animObject.target = EditorGUILayout.Toggle("Assign Object", _animObject.target);

    //    if (EditorGUILayout.BeginFadeGroup(_animObject.faded))
    //    {
    //        var previousColor = GUI.backgroundColor;

    //        if (powerUp == null)
    //        {
    //            GUI.backgroundColor = new Color(255, 0, 0, 0.4f);
    //        }
    //        powerUp = (GameObject)EditorGUILayout.ObjectField("Power Up Asset", powerUp, typeof(GameObject), false, GUILayout.Width(400));

    //        EditorGUILayout.Space();

    //        if (powerUp != null)
    //        {
    //            allEmpty = spawnedEmptyName.ToArray();
    //            if (allEmpty.Length > 0)
    //            {
    //                emptyIndex = EditorGUILayout.Popup("Select From Spawned", emptyIndex, allEmpty);
    //                currentEmpty = allEmpty[emptyIndex];

    //                EditorGUILayout.Space();

    //                /*if (GUILayout.Button("Remove"))
    //                {
    //                    GameObject b = GameObject.Find(allEmpty[emptyIndex]);
    //                    spawnedEmptyName.Remove(b.name);
    //                    DestroyImmediate(b);
    //                    allEmpty = spawnedEmptyName.ToArray();

    //                    foreach (var item in spawnedEmptyName)
    //                    {
    //                        Debug.Log(item);
    //                    }
    //                }*/

    //                EditorGUILayout.BeginHorizontal();
    //                EditorGUILayout.LabelField("Spawn Asset in: " + currentEmpty);

    //                if (GUILayout.Button("Spawn"))
    //                {
    //                    GameObject b = powerUp;
    //                    b.transform.position = GameObject.Find(allEmpty[emptyIndex]).transform.position;
    //                    PrefabUtility.InstantiatePrefab(b);
    //                }

    //                EditorGUILayout.EndHorizontal();
    //            }
    //            else
    //            {
    //                EditorGUILayout.HelpBox("No empty objects created", MessageType.Warning);
    //            }

    //            EditorGUILayout.Space();
    //            EditorGUILayout.Space();

    //            if (powerUpName == null)
    //            {
    //                powerUpName = powerUp.name;
    //            }
    //            powerUpName = EditorGUILayout.TextField("Name", powerUpName);

    //            if (GUILayout.Button("Spawn Object"))
    //            {
    //                GameObject b = powerUp;
    //                b.name = powerUpName;
    //                PrefabUtility.InstantiatePrefab(b);
    //            }

    //        }

    //    }
    //    EditorGUILayout.EndFadeGroup();
    //    if (spawnedEmptyName.Count > 0)
    //    {
    //        scriptable.emptyCreated = spawnedEmptyName;
    //    }
    //}


}
