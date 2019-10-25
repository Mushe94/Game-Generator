using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameModeGeneratorWindow : EditorWindow
{
    public static void OpenWindow()
	{
		GetWindow<GameModeGeneratorWindow>();
	}
}
