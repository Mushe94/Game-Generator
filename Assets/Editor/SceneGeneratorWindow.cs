using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelGeneratorWindow : EditorWindow
{
    public static void OpenWindow()
	{
		GetWindow<LevelGeneratorWindow>();
	}
}
