using UnityEngine;
using UnityEditor;

public class GameGeneratorWindow : EditorWindow
{
    private bool scenesEnabled;

	[MenuItem("Engines Team/Game Generator #g")]
    public static void OpenWindow()
	{
		GameGeneratorWindow gameGeneratorWindow = GetWindow<GameGeneratorWindow>("Game Generator", true);
		gameGeneratorWindow.minSize = new Vector2(200f, 150f);
		gameGeneratorWindow.maxSize = new Vector2(200f, 150f);
	}

	private void OnGUI()
	{
        int numberOfItems = 0;
        foreach (GameModeProperties gameModeProperties in Resources.LoadAll("Data/Game Mode Properties/"))
        {
            numberOfItems++;
        }
        scenesEnabled = numberOfItems > 0;
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
		if (GUILayout.Button("Game Mode configuration"))
		{
			GameModeGeneratorWindow.OpenWindow();
		}
        EditorGUI.BeginDisabledGroup(!scenesEnabled);
		if (GUILayout.Button("Scene generator"))
		{
			SceneGeneratorWindow.OpenWindow();
		}
		if (GUILayout.Button("Player configuration"))
		{
			PlayerGeneratorWindow.OpenWindow();
		}
		if (GUILayout.Button("Level configuration"))
		{
            LevelConfigurationWindow.OpenWindow();
		}
        EditorGUI.EndDisabledGroup();
	}
}
