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
	private readonly List<Vector3> cubesPosition = new List<Vector3>();

    public static void OpenWindow()
	{
		SceneGeneratorWindow sceneGeneratorWindow = GetWindow<SceneGeneratorWindow>("Scene Generator", true);
		sceneGeneratorWindow.minSize = new Vector2(200f, 150f);
		sceneGeneratorWindow.maxSize = new Vector2(200f, 150f);
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
		useSprayPattern = EditorGUILayout.Toggle("Spray Pattern", useSprayPattern);
		if (useSprayPattern)
		{
			repeats = EditorGUILayout.IntField("Repeat pattern", repeats);
		}
		numberOfLevels = EditorGUILayout.IntField("Number of Levels", numberOfLevels);
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
				string path = "Assets/Scenes/Level.unity";
				path = AssetDatabase.GenerateUniqueAssetPath(path);
				EditorSceneManager.SaveScene(scene, path, false);
				EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
				cubesPosition.Clear();
				foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Generated"))
				{
					cubesPosition.Add(gameObject.transform.position);
				}
				Vector3 finalCubePosition;
				for (int i = 0; i < sceneWidth; i++)
				{
					for (int j = 0; j < sceneWidth; j++)
					{
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.tag = "Generated";
						cube.transform.position = Vector3.zero;
						finalCubePosition = cube.transform.position + cube.transform.forward * j;
						finalCubePosition += cube.transform.right * i;
						if (cubesPosition.Contains(finalCubePosition))
						{
							DestroyImmediate(cube);
						} else
						{
							cube.transform.position = finalCubePosition;
							cubesPosition.Add(finalCubePosition);
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
								GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
								cube.tag = "Generated";
								cube.transform.position = Vector3.zero;
								int numberOfTries = 0;
								Vector3 randomVector = RandomVector(ref numberOfTries);
								cube.transform.rotation = Quaternion.LookRotation(randomVector);
								finalCubePosition = cube.transform.position + cube.transform.forward * (j + numberOfTries);
								cube.transform.position = finalCubePosition;
			
								for (int k = 0; k < i; k++)
								{
									randomVector = RandomVector(ref numberOfTries);
									cube.transform.rotation = Quaternion.LookRotation(randomVector);
									finalCubePosition = cube.transform.position + cube.transform.forward;
									cube.transform.position = finalCubePosition;
								}
								bool repeat = false;
								do
								{
									foreach (Vector3 cubePosition in cubesPosition)
									{
										if (cubePosition == finalCubePosition)
										{
											randomVector = RandomVector(ref numberOfTries);
											cube.transform.rotation = Quaternion.LookRotation(randomVector);
											finalCubePosition = cube.transform.position + cube.transform.forward * (j + numberOfTries);
											cube.transform.position = finalCubePosition;
											repeat = true;
											break;
										} else
										{
											repeat = false;
										}
									}
								} while (repeat);
								cube.transform.position = finalCubePosition;
								cubesPosition.Add(finalCubePosition);
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
