using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SceneGeneratorWindow : EditorWindow
{
	private int sceneWidth = 1;
	private int sceneDepth = 1;
	private int repeats = 1;
	private bool useSprayPattern = false;
	private int numberOfLevels = 1;
	private string sceneName = "";
	private Color previewColor = Color.white;
	private bool showColor;
	private readonly List<Vector3> cubesPosition = new List<Vector3>();
	private static SceneGeneratorWindow sceneGeneratorWindow;
	private int numberOfObjects = 0;
	private readonly List<GameObject> gameObjects = new List<GameObject>();

	public static void OpenWindow()
	{
		sceneGeneratorWindow = GetWindow<SceneGeneratorWindow>("Scene Generator", true);
		sceneGeneratorWindow.minSize = new Vector2(200f, 170f);
		sceneGeneratorWindow.maxSize = new Vector2(300f, 350f);
	}

	private void OnGUI()
	{
		EditorGUI.BeginChangeCheck();
		if (numberOfLevels <= 0)
		{
			numberOfLevels = 1;
		} else if (numberOfLevels > 10)
		{
			numberOfLevels = 10;
		}
		if (sceneDepth <= 0)
		{
			sceneDepth = 1;
		} else if (sceneDepth > 10)
		{
			sceneDepth = 10;
		}
		if (sceneWidth <= 0)
		{
			sceneWidth = 1;
		} else if (sceneWidth > 10)
		{
			sceneWidth = 10;
		}
		if (repeats <= 0)
		{
			repeats = 1;
		} else if (repeats > 10)
		{
			repeats = 10;
		}
		sceneDepth = EditorGUILayout.IntField("Depth", sceneDepth);
		sceneWidth = EditorGUILayout.IntField("Width", sceneWidth);
		useSprayPattern = EditorGUILayout.Toggle("Use Spray Pattern?", useSprayPattern);
		if (useSprayPattern)
		{
			repeats = EditorGUILayout.IntField("Repeat pattern", repeats);
		}
		numberOfLevels = EditorGUILayout.IntField("Number of Levels", numberOfLevels);
		sceneName = EditorGUILayout.TextField("Scenes Name", sceneName);
		EditorGUILayout.Space();
		if (sceneDepth > 0 && sceneWidth > 0)
		{
			Texture2D texture = new Texture2D(sceneWidth * 10, sceneDepth * 10);
			for (int y = 0; y < texture.width; y++)
			{
				for (int x = 0; x < texture.height; x++)
				{
					texture.SetPixel(y, x, previewColor);
				}
			}
			texture.Apply();
			GUIStyle guiStyle = new GUIStyle
			{
				alignment = TextAnchor.MiddleCenter
			};
			if (GUILayout.Button(texture, guiStyle))
			{
				showColor = !showColor;				
			}
			if (showColor)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				previewColor = EditorGUILayout.ColorField(previewColor);
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.Space();
		if (numberOfObjects < 0)
		{
			numberOfObjects = 0;
		} else if (numberOfObjects >= 25)
		{
			numberOfObjects = 25;
		}
		numberOfObjects = EditorGUILayout.IntField("Number of Objects", numberOfObjects);
		for (int i = 0; i < numberOfObjects; i++)
		{
			if (i >= gameObjects.Count)
			{
				gameObjects.Add(null);
			}
			gameObjects[i] = (GameObject)EditorGUILayout.ObjectField("Model or Prefab", gameObjects[i], typeof(GameObject), false);
		}
		float windowFullSize = 10f * numberOfObjects;
		sceneGeneratorWindow.maxSize = new Vector2(sceneGeneratorWindow.maxSize.x, sceneGeneratorWindow.maxSize.y + windowFullSize);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (GUILayout.Button("Generate"))
		{
			for (int g = 0; g < numberOfLevels; g++)
			{
				Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
				if (sceneName == "")
				{
					sceneName = "Level";
				}
				string path = "Assets/Scenes/" + sceneName + ".unity";
				if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
				{
					AssetDatabase.CreateFolder("Assets", "Scenes");
					Debug.Log("The introduced folder doesn't exist, so I just created a default one for you.");
					AssetDatabase.Refresh();
				}
				path = AssetDatabase.GenerateUniqueAssetPath(path);
				EditorSceneManager.SaveScene(scene, path, false);
				EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
				cubesPosition.Clear();
				for (int i = gameObjects.Count - 1; i >= 0; i--)
				{
					if (gameObjects[i] == null)
					{
						gameObjects.Remove(gameObjects[i]);
					}
				}
				numberOfObjects = gameObjects.Count;
				Vector3 finalObjectPosition;
				for (int i = 0; i < sceneWidth; i++)
				{
					for (int j = 0; j < sceneWidth; j++)
					{
						GameObject gameObject;
						if (numberOfObjects == 0)
						{
							gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
						} else
						{
							gameObject = gameObjects[Random.Range(0, gameObjects.Count)];
							gameObject = Instantiate(gameObject);
						}
                        if (previewColor != Color.white)
                        {
                            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                            if (meshRenderer != null)
                            {
                                meshRenderer.sharedMaterial.color = previewColor;
                            }
                        }
						gameObject.transform.position = Vector3.zero;
						finalObjectPosition = gameObject.transform.position + gameObject.transform.forward * j;
						finalObjectPosition += gameObject.transform.right * i;
						if (cubesPosition.Contains(finalObjectPosition))
						{
							DestroyImmediate(gameObject);
						} else
						{
							gameObject.transform.position = finalObjectPosition;
							cubesPosition.Add(finalObjectPosition);
						}
					}
				}
				if (useSprayPattern)
				{
					for (int h = 0; h < repeats; h++)
					{
						for (int i = 0; i < sceneWidth; i++)
						{
							for (int j = 0; j < sceneDepth; j++)
							{
								GameObject gameObject;
								if (numberOfObjects == 0)
								{
									gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
								} else
								{
									gameObject = gameObjects[Random.Range(0, gameObjects.Count)];
									gameObject = Instantiate(gameObject);
								}
                                if (previewColor != Color.white)
                                {
                                    MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                                    if (meshRenderer != null)
                                    {
                                        meshRenderer.sharedMaterial.color = previewColor;
                                    }
                                }
                                gameObject.transform.position = Vector3.zero;
								int numberOfTries = 0;
								Vector3 randomVector = RandomVector(ref numberOfTries);
								gameObject.transform.rotation = Quaternion.LookRotation(randomVector);
								finalObjectPosition = gameObject.transform.position + gameObject.transform.forward * (j + numberOfTries);
								gameObject.transform.position = finalObjectPosition;			
								for (int k = 0; k < i; k++)
								{
									randomVector = RandomVector(ref numberOfTries);
									gameObject.transform.rotation = Quaternion.LookRotation(randomVector);
									finalObjectPosition = gameObject.transform.position + gameObject.transform.forward;
									gameObject.transform.position = finalObjectPosition;
								}
								bool repeat = false;
								do
								{
									foreach (Vector3 cubePosition in cubesPosition)
									{
										if (cubePosition == finalObjectPosition)
										{
											randomVector = RandomVector(ref numberOfTries);
											gameObject.transform.rotation = Quaternion.LookRotation(randomVector);
											finalObjectPosition = gameObject.transform.position + gameObject.transform.forward * (j + numberOfTries);
											gameObject.transform.position = finalObjectPosition;
											repeat = true;
											break;
										} else
										{
											repeat = false;
										}
									}
								} while (repeat);
								gameObject.transform.position = finalObjectPosition;
								cubesPosition.Add(finalObjectPosition);
							}
						}
					}
				}
				EditorSceneManager.SaveScene(scene, path, false);
				AddSceneToBuildSettings(path);
				EditorSceneManager.CloseScene(scene, true);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}

	private Vector3 RandomVector(ref int tries)
	{
		Vector3 randomVector = default;
		int random = Random.Range(0, 4);
		switch (random)
		{
			case 0:
				randomVector = Vector3.forward;
				break;
			case 1:
				randomVector = Vector3.back;
				break;
			case 2:
				randomVector = Vector3.left;
				break;
			case 3:
				randomVector = Vector3.right;
				break;
		}
		tries++;
		return randomVector;
	}

	private void AddSceneToBuildSettings(string pathOfSceneToAdd)
	{
		int indexOfSceneIfExist = -1;
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			if (EditorBuildSettings.scenes[i].path == pathOfSceneToAdd)
			{
				indexOfSceneIfExist = i;
				break;
			}
		}
		EditorBuildSettingsScene[] newScenes;
		if (indexOfSceneIfExist == -1)
		{
			newScenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];
			int i = 0;
			for (; i < EditorBuildSettings.scenes.Length; i++)
			{
				newScenes[i] = EditorBuildSettings.scenes[i];
			}
			newScenes[i] = new EditorBuildSettingsScene(pathOfSceneToAdd, true);
		} else
		{
			newScenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length];
			int i = 0, j = 0;
			for (; i < EditorBuildSettings.scenes.Length; i++)
			{
				if (i != indexOfSceneIfExist)
				{
					newScenes[j++] = EditorBuildSettings.scenes[i];
				}
			}
			newScenes[j] = new EditorBuildSettingsScene(pathOfSceneToAdd, true);
		}
		EditorBuildSettings.scenes = newScenes;
	}
}
