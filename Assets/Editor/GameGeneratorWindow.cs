using UnityEngine;
using UnityEditor;

public class GameGeneratorWindow : EditorWindow
{
	[MenuItem("Engines Team/Game Generator #g")]
    public static void OpenWindow()
	{
		GameGeneratorWindow gameGeneratorWindow = GetWindow<GameGeneratorWindow>("Game Generator", true);
		gameGeneratorWindow.minSize = new Vector2(200f, 150f);
		gameGeneratorWindow.maxSize = new Vector2(200f, 150f);
	}

	private void OnGUI()
	{
		EditorGUILayout.Space();
		GUIStyle bannerStyle = new GUIStyle
		{
			alignment = TextAnchor.MiddleCenter,
			fontSize = 15,
			fontStyle = FontStyle.Bold
		};
		EditorGUILayout.LabelField("Game Generator", bannerStyle);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (GUILayout.Button("Player configuration"))
		{
			PlayerGeneratorWindow.OpenWindow();
		}
		if (GUILayout.Button("Scene generator"))
		{
			SceneGeneratorWindow.OpenWindow();
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
