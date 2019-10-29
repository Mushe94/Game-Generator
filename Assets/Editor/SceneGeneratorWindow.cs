using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneGeneratorWindow : EditorWindow
{
	private int sceneWidth;
	private int sceneDepth;
	private List<Vector3> cubesPosition = new List<Vector3>();

    public static void OpenWindow()
	{
		GetWindow<SceneGeneratorWindow>();
	}

	private void OnGUI()
	{
		sceneDepth = EditorGUILayout.IntField("Depth", sceneDepth);
		sceneWidth = EditorGUILayout.IntField("Width", sceneWidth);
		if (GUILayout.Button("Generate"))
		{
			for (int i = 0; i < sceneWidth; i++)
			{
				for (int j = 0; j < sceneDepth; j++)
				{
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.position = Vector3.zero;
					Vector3 randomVector = RandomVector();
					cube.transform.rotation = Quaternion.LookRotation(randomVector);
					Vector3 finalCubePosition = cube.transform.position + cube.transform.forward * j;
					foreach (var item in cubesPosition)
					{
						Debug.Log("I have " + item);
					}
					Debug.Log("and " + finalCubePosition);
					bool repeat = false;
					do
					{
						foreach (var item in cubesPosition)
						{
							if (item == finalCubePosition)
							{
								randomVector = RandomVector();
								cube.transform.rotation = Quaternion.LookRotation(randomVector);
								finalCubePosition = cube.transform.position + cube.transform.forward * j;
								Debug.Log("asd");
								repeat = true;
							}
						}
					} while (repeat);
					cube.transform.position = finalCubePosition;
					cubesPosition.Add(finalCubePosition);
				}
			}
		}
	}

	private Vector3 RandomVector()
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
		return randomVector;
	}
}
