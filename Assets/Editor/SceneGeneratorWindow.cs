﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneGeneratorWindow : EditorWindow
{
	private int sceneWidth;
	private int sceneDepth;
	private int repeats;
	private readonly List<Vector3> cubesPosition = new List<Vector3>();

    public static void OpenWindow()
	{
		GetWindow<SceneGeneratorWindow>();
	}

	private void OnGUI()
	{
		sceneDepth = EditorGUILayout.IntField("Depth", sceneDepth);
		sceneWidth = EditorGUILayout.IntField("Width", sceneWidth);
		repeats = EditorGUILayout.IntField("Repeat pattern", repeats);
		if (GUILayout.Button("Generate"))
		{
			cubesPosition.Clear();
			if (repeats == 0)
			{
				repeats = 1;
			}
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Generated"))
			{
				cubesPosition.Add(gameObject.transform.position);
			}
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
						Vector3 finalCubePosition = cube.transform.position + cube.transform.forward * (j + numberOfTries);
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
}
