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

    private GameObject powerUpPreview;
    private GameObject platformPreview;
    private GameObject enemiesPreview;

    private bool seePowerUpsPreview;
    private bool seePlatformPreview;
    private bool seeEnemiesPreview;

    private bool getList;
    private bool getBool;
    private bool selectScriptableName;
    private string scriptableName;
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
        }

        getList = true;
        getBool = true;
    }

    private void OnGUI()
    {
        FindScriptableList();
        CreateScriptable();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (scriptable != null)
        {
            scriptable.gameObjectsPreview.Clear();
            if (scriptable.gameObjectsPreview.Count == 0)
            {
                foreach (var item in Resources.LoadAll<GameObject>("Prefabs/Level Configuration"))
                {
                    scriptable.gameObjectsPreview.Add(item);
                }
            }



            if (getBool)
            {
                getBool = false;
                seePowerUpsPreview = scriptable.previewPowerUp;
                seeEnemiesPreview = scriptable.previewEnemies;
                seePlatformPreview = scriptable.previewPlatform;
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            DrawPowerUpsParameters();
            DrawPlatformParameters();
            DrawEnemiesParameters();
        }
        else
        {
            EditorGUILayout.HelpBox("Scriptable not selected", MessageType.Warning);
        }
    }
    private void CreateScriptable()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        scriptable = (LevelConfigurationData)EditorGUILayout.ObjectField("Select Scriptable", scriptable, typeof(LevelConfigurationData), false);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        selectScriptableName = EditorGUILayout.BeginToggleGroup("Select Scriptable Name", selectScriptableName);

        if (selectScriptableName)
        {
            scriptableName = EditorGUILayout.TextField("Scriptable Name", scriptableName);
        }

        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Scriptable"))
        {
            LevelConfigurationData data = CreateInstance<LevelConfigurationData>();

            var path = "";
            if (selectScriptableName)
            {
                path = "Assets/Resources/Data/" + scriptableName + ".asset";
            }
            else
            {
                path = "Assets/Resources/Data/LevelConfigurationData.asset";
            }

            path = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(data, path);
        }

        EditorGUILayout.LabelField("Data Path: Assets/Resources/Data");
    }
    private void FindScriptableList()
    {
        if (scriptable != null && getList)
        {
            getList = false;
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
    }

    private void DrawPlatformParameters()
    {
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Platforms", _secondaryStyle);
        EditorGUILayout.Space();

        seePlatformPreview = EditorGUILayout.Toggle("Set Preview", seePlatformPreview);

        scriptable.previewPlatform = seePlatformPreview;

        if (seePlatformPreview)
        {
            platformPreview = (GameObject)EditorGUILayout.ObjectField("Preview Object", platformPreview, typeof(GameObject), false);

            if (platformPreview != null)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Set Preview"))
                {
                    foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
                    {
                        if (obj.GetComponent<Platform>())
                        {
                            var child = obj.GetComponent<GetChild>();
                            child.child.SetActive(true);
                            child.child.hideFlags = HideFlags.None;
                            var childRend = obj.GetComponentInChildren<Renderer>();
                            var childMesh = obj.GetComponentInChildren<MeshFilter>();

                            if (childRend != null)
                            {
                                child.child.transform.localScale = platformPreview.transform.localScale;
                                childMesh.sharedMesh = platformPreview.GetComponent<MeshFilter>().sharedMesh;

                                var mat = new Material(Shader.Find("TransparencyShader"));
                                mat.SetColor("_Color", Color.cyan);
                                mat.SetFloat("_Opacity", 0.71f);
                                mat.SetFloat("_Emission", -0.17f);

                                childRend.sharedMaterial = mat;
                                child.child.hideFlags = HideFlags.HideInHierarchy;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.GetComponent<Platform>())
                {
                    var child = obj.GetComponent<GetChild>();
                    child.child.hideFlags = HideFlags.None;

                    var childRend = obj.GetComponentInChildren<Renderer>();

                    if (childRend != null)
                    {
                        childRend.gameObject.SetActive(false);
                        child.child.hideFlags = HideFlags.HideInHierarchy;
                    }
                }
            }
        }
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
                foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.GetComponent<Platform>())
                    {
                        var childComponent = obj.GetComponent<GetChild>();
                        if (childComponent == null)
                        {
                            obj.AddComponent<GetChild>();
                        }
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

        seeEnemiesPreview = EditorGUILayout.Toggle("Set Preview", seeEnemiesPreview);

        scriptable.previewEnemies = seeEnemiesPreview;

        if (seeEnemiesPreview)
        {
            enemiesPreview = (GameObject)EditorGUILayout.ObjectField("Preview Object", enemiesPreview, typeof(GameObject), false);

            if (enemiesPreview != null)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Set Preview"))
                {
                    foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
                    {
                        if (obj.GetComponent<Enemy>())
                        {
                            var child = obj.GetComponent<GetChild>();
                            child.child.SetActive(true);
                            child.child.hideFlags = HideFlags.None;
                            var childRend = obj.GetComponentInChildren<Renderer>();
                            var childMesh = obj.GetComponentInChildren<MeshFilter>();

                            if (childRend != null)
                            {
                                child.child.transform.localScale = platformPreview.transform.localScale;
                                childMesh.sharedMesh = enemiesPreview.GetComponent<MeshFilter>().sharedMesh;

                                var mat = new Material(Shader.Find("TransparencyShader"));
                                mat.SetColor("_Color", Color.cyan);
                                mat.SetFloat("_Opacity", 0.71f);
                                mat.SetFloat("_Emission", -0.17f);

                                childRend.sharedMaterial = mat;
                                child.child.hideFlags = HideFlags.HideInHierarchy;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.GetComponent<Enemy>())
                {
                    var child = obj.GetComponent<GetChild>();
                    child.child.hideFlags = HideFlags.None;

                    var childRend = obj.GetComponentInChildren<Renderer>();

                    if (childRend != null)
                    {
                        childRend.gameObject.SetActive(false);
                        child.child.hideFlags = HideFlags.HideInHierarchy;
                    }
                }
            }
        }
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
                foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.GetComponent<Enemy>())
                    {
                        var childComponent = obj.GetComponent<GetChild>();
                        if (childComponent == null)
                        {
                            obj.AddComponent<GetChild>();
                        }
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

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("PowerUps", _secondaryStyle);
        EditorGUILayout.Space();

        seePowerUpsPreview = EditorGUILayout.Toggle("Set Preview", seePowerUpsPreview);

        scriptable.previewPowerUp = seePowerUpsPreview;

        if (seePowerUpsPreview)
        {
            powerUpPreview = (GameObject)EditorGUILayout.ObjectField("Preview Object", powerUpPreview, typeof(GameObject), false);

            if (powerUpPreview != null)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Set Preview"))
                {
                    foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
                    {
                        if (obj.GetComponent<PowerUp>())
                        {
                            var child = obj.GetComponent<GetChild>();
                            child.child.SetActive(true);
                            child.child.hideFlags = HideFlags.None;
                            var childRend = obj.GetComponentInChildren<Renderer>();
                            var childMesh = obj.GetComponentInChildren<MeshFilter>();

                            if (childRend != null)
                            {
                                child.child.transform.localScale = powerUpPreview.transform.localScale;
                                childMesh.sharedMesh = powerUpPreview.GetComponent<MeshFilter>().sharedMesh;

                                var mat = new Material(Shader.Find("TransparencyShader"));
                                mat.SetColor("_Color", Color.cyan);
                                mat.SetFloat("_Opacity", 0.71f);
                                mat.SetFloat("_Emission", -0.17f);
                                childRend.sharedMaterial = mat;
                                child.child.hideFlags = HideFlags.HideInHierarchy;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.GetComponent<PowerUp>())
                {
                    var child = obj.GetComponent<GetChild>();
                    child.child.hideFlags = HideFlags.None;

                    var childRend = obj.GetComponentInChildren<Renderer>();

                    if (childRend != null)
                    {
                        childRend.gameObject.SetActive(false);
                        child.child.hideFlags = HideFlags.HideInHierarchy;
                    }
                }
            }
        }

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

                foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.GetComponent<PowerUp>())
                    {
                        var childComponent = obj.GetComponent<GetChild>();
                        if (childComponent == null)
                        {
                            GetChild getChild = obj.AddComponent<GetChild>();
                            if (powerUpPreview != null)
                            {
                                getChild.spawn = powerUpPreview;
                            }
                            else
                            {
                                getChild.spawn = new GameObject();
                            }
                        }
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

            GUI.backgroundColor = previousColor;
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
