using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelConfigurationWindow : EditorWindow
{
    public static void OpenWindow()
	{
		GetWindow<LevelConfigurationWindow>();
	}
}
