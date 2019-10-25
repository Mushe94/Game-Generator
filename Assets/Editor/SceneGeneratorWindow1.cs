using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneGeneratorWindow : EditorWindow
{
    public static void OpenWindow()
	{
		GetWindow<SceneGeneratorWindow>();
	}
}
