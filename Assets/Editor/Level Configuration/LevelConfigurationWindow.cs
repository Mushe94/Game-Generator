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


    private AnimBool _animEmpty;
    private AnimBool _animObject;
    private AnimBool _animPlatform;
    private AnimBool _animEnemy;

    public static void OpenWindow()
    {
        LevelConfigurationWindow levelConfigurationWindow = GetWindow<LevelConfigurationWindow>("Level Configuration", true);
        levelConfigurationWindow.minSize = new Vector2(400f, 300f);
    }

    private void OnEnable()
    {
        _style = new GUIStyle()
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        _animEmpty = new AnimBool(false);
        _animEmpty.valueChanged.AddListener(Repaint);
        _animObject = new AnimBool(false);
        _animObject.valueChanged.AddListener(Repaint);
        _animPlatform = new AnimBool(false);
        _animPlatform.valueChanged.AddListener(Repaint);
        _animEnemy = new AnimBool(false);
        _animEnemy.valueChanged.AddListener(Repaint);

        _secondaryStyle = new GUIStyle()
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold
        };

        if (scriptable == null)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Data");
                Debug.Log("The introduced folder doesn't exist, so I just created a default one for you.");
                AssetDatabase.Refresh();
            }
            scriptable = Resources.Load<LevelConfigurationData>("Data/LevelConfigurationData");
            if (scriptable == null)
            {
                LevelConfigurationData data = CreateInstance<LevelConfigurationData>();
                AssetDatabase.CreateAsset(data, "Assets/Resources/Data/LevelConfigurationData.asset");
                scriptable = data;
            }
        }


        if (scriptable.emptyCreated.Count > 0)
        {
            List<string> names = new List<string>();
            foreach (var name in scriptable.emptyCreated)
            {
                var b = GameObject.Find(name);
                if (b == null)
                {
                    names.Add(name);
                }
            }
            foreach (var item in names)
            {
                scriptable.emptyCreated.Remove(item);
            }
            spawnedEmptyName = scriptable.emptyCreated;
        }

    }

    private void OnGUI()
    {
        DrawPowerUpsParameters();
        DrawPlatformParameters();
        DrawEnemiesParameters();
    }

    private void DrawPlatformParameters()
    {
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Platforms", _secondaryStyle);

        EditorGUILayout.Space();

        _animPlatform.target = EditorGUILayout.Toggle("Use Empty Object", _animPlatform.target);

        if (EditorGUILayout.BeginFadeGroup(_animPlatform.faded))
        {
            if (GUILayout.Button("SpawnPlatform", GUILayout.Width(200)))
            {
                for (int i = 0; i < scriptable.gameObjectsPreview.Count; i++)
                {
                    Platform c = scriptable.gameObjectsPreview[i].GetComponent<Platform>();

                    if (c)
                    {
                        GameObject a = scriptable.gameObjectsPreview[i];

                        PrefabUtility.InstantiatePrefab(a);
                    }

                }
            }

        }

        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.Space();
    }

    private void DrawEnemiesParameters()
    {
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Enemies", _secondaryStyle);

        EditorGUILayout.Space();

        _animEnemy.target = EditorGUILayout.Toggle("Use Empty Object", _animEnemy.target);

        if (EditorGUILayout.BeginFadeGroup(_animEnemy.faded))
        {
            if (GUILayout.Button("SpawnEnemy", GUILayout.Width(200)))
            {
                for (int i = 0; i < scriptable.gameObjectsPreview.Count; i++)
                {
                    Enemy c = scriptable.gameObjectsPreview[i].GetComponent<Enemy>();

                    if (c)
                    {
                        GameObject a = scriptable.gameObjectsPreview[i];

                        PrefabUtility.InstantiatePrefab(a);
                    }

                }
            }

        }

        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.Space();
    }

    private void DrawPowerUpsParameters()
    {
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Level Configuration", _style);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("PowerUps", _secondaryStyle);
        EditorGUILayout.Space();

        powerupsIndex = EditorGUILayout.Popup("Power Ups Types", powerupsIndex, allPowerups);
        currentPowerup = allPowerups[powerupsIndex];

        EditorGUILayout.Space();

        _animEmpty.target = EditorGUILayout.Toggle("Use Empty Object", _animEmpty.target);

        if (EditorGUILayout.BeginFadeGroup(_animEmpty.faded))
        {
            if (GUILayout.Button("SpawnPowerUp", GUILayout.Width(200)))
            {
                for (int i = 0; i < scriptable.gameObjectsPreview.Count; i++)
                {
                    PowerUp c = scriptable.gameObjectsPreview[i].GetComponent<PowerUp>();

                    if (c)
                    {
                        GameObject a = scriptable.gameObjectsPreview[i];
                        a.name = "";
                        a.name = "PowerUp - " + currentPowerup;

                        if (spawnedEmptyName.Contains(a.name))
                        {
                            if (currentPowerup == "Damage")
                            {
                                powerUpsCopies[0]++;
                                a.name = "PowerUp - " + currentPowerup + " (" + powerUpsCopies[0] + ")";
                            }
                            else if (currentPowerup == "Health")
                            {
                                powerUpsCopies[1]++;
                                a.name = "PowerUp - " + currentPowerup + " (" + powerUpsCopies[1] + ")";
                            }
                            else
                            {
                                powerUpsCopies[2]++;
                                a.name = "PowerUp - " + currentPowerup + " (" + powerUpsCopies[2] + ")";
                            }
                        }

                        PrefabUtility.InstantiatePrefab(a);

                        spawnedEmptyName.Add(a.name);

                    }

                }
            }

        }

        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.Space();

        _animObject.target = EditorGUILayout.Toggle("Assign Object", _animObject.target);

        if (EditorGUILayout.BeginFadeGroup(_animObject.faded))
        {
            var previousColor = GUI.backgroundColor;

            if (powerUp == null)
            {
                GUI.backgroundColor = new Color(255, 0, 0, 0.4f);
            }
            powerUp = (GameObject)EditorGUILayout.ObjectField("Power Up Asset", powerUp, typeof(GameObject), false, GUILayout.Width(400));

            EditorGUILayout.Space();

            if (powerUp != null)
            {
                allEmpty = spawnedEmptyName.ToArray();
                if (allEmpty.Length > 0)
                {
                    emptyIndex = EditorGUILayout.Popup("Select From Spawned", emptyIndex, allEmpty);
                    currentEmpty = allEmpty[emptyIndex];

                    EditorGUILayout.Space();

                    /*if (GUILayout.Button("Remove"))
                    {
                        GameObject b = GameObject.Find(allEmpty[emptyIndex]);
                        spawnedEmptyName.Remove(b.name);
                        DestroyImmediate(b);
                        allEmpty = spawnedEmptyName.ToArray();

                        foreach (var item in spawnedEmptyName)
                        {
                            Debug.Log(item);
                        }
                    }*/

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Spawn Asset in: " + currentEmpty);

                    if (GUILayout.Button("Spawn"))
                    {
                        GameObject b = powerUp;
                        b.transform.position = GameObject.Find(allEmpty[emptyIndex]).transform.position;
                        PrefabUtility.InstantiatePrefab(b);
                    }

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.HelpBox("No empty objects created", MessageType.Warning);
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (powerUpName == null)
                {
                    powerUpName = powerUp.name;
                }
                powerUpName = EditorGUILayout.TextField("Name", powerUpName);

                if (GUILayout.Button("Spawn Object"))
                {
                    GameObject b = powerUp;
                    b.name = powerUpName;
                    PrefabUtility.InstantiatePrefab(b);
                }

            }

        }
        EditorGUILayout.EndFadeGroup();
        if (spawnedEmptyName.Count > 0)
        {
            scriptable.emptyCreated = spawnedEmptyName;
        }
    }
}
