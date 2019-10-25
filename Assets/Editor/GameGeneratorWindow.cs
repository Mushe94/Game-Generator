using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameGeneratorWindow : EditorWindow
{
	[MenuItem("Engines Team/Game Generator #g")]
    public static void OpenWindow()
	{
		GetWindow<GameGeneratorWindow>();
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Player configuration"))
		{
			SceneGeneratorWindow.OpenWindow();
		}
		if (GUILayout.Button("Scene generator"))
		{
			LevelGeneratorWindow.OpenWindow();
		}
		if (GUILayout.Button("Game Mode configuration"))
		{
			GameModeGeneratorWindow.OpenWindow();
		}
		if (GUILayout.Button("Level configuration"))
		{
			LevelConfigurationWindow.OpenWindow();
		}
	}
}
