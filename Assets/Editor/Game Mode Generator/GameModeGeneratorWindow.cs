using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class GameModeGeneratorWindow : EditorWindow
{
	GUIStyle titlestyle;
	GUIStyle subtitlestyle;
	GUIStyle choosestyle;

	Texture2D platExample;
	Texture2D survExample;
	Texture2D endlessExample;

	Texture2D isoExample;
	Texture2D tpExample;
	Texture2D tdEcample;
	Texture2D sideExample;


	bool choseEndless;
	bool chosePlatformer;
	bool choseSurvival;

	bool choseTP;
	bool choseTD;
	bool choseISO;
	bool choseSIDE;

	string selectedMode;
	string selectedPers;
	string selectedDim;
	string selectedObj;

	bool gameModePage;
	bool propertiesPage;

	bool toPointPlat;
	bool killEveryOnePlat;
	bool endLevelEnd;
	bool continueEnd;
	bool survTime;

	int howManyLevels;
	private void OnEnable()
	{
		titlestyle = new GUIStyle
		{
			fontSize = 20,
			fontStyle = FontStyle.Bold,
			alignment = TextAnchor.MiddleCenter
		};

		subtitlestyle = new GUIStyle
		{
			fontSize = 18,
			fontStyle = FontStyle.Italic,
			alignment = TextAnchor.MiddleLeft
		};

		choosestyle = new GUIStyle
		{
			fontSize = 15,
			fontStyle = FontStyle.Bold,
			alignment = TextAnchor.MiddleCenter
		};

		var a = AssetDatabase.GetAllAssetPaths();

		survExample = (Texture2D)Resources.Load("Survival_Example");
		endlessExample = (Texture2D)Resources.Load("Endless_Example");
		platExample = (Texture2D)Resources.Load("Platformer_Example");

		tpExample = (Texture2D)Resources.Load("TP_Example");
		tdEcample = (Texture2D)Resources.Load("TD_Example");
		isoExample = (Texture2D)Resources.Load("Iso_Example");
		sideExample = (Texture2D)Resources.Load("Side_Example");

		choseSurvival = false;
		choseEndless = false;
		chosePlatformer = false;

		gameModePage = true;
		propertiesPage = false;
	}

	public static void OpenWindow()
	{
        GameModeGeneratorWindow gameModeGeneratorWindow = GetWindow<GameModeGeneratorWindow>("Game Mode Generator", true);
        gameModeGeneratorWindow.minSize = new Vector2(400f, 500f);
	}

	private void OnGUI()
	{

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (gameModePage)
		{
			EditorGUILayout.LabelField("Generate a Game Mode", titlestyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			ChooseTypeOfGame();

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			//ChooseDimensions();

			//EditorGUILayout.Space();
			//EditorGUILayout.Space();

			ChoosePerspective();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GotoProperties();
		}
		if (propertiesPage)
		{
			PropertiesGame();

			if (GUILayout.Button("Create Scriptable"))
			{
				CreateScriptable();
			}
		}
	}


	void PropertiesGame()
	{
		EditorGUILayout.LabelField("Properties", titlestyle);

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		if (chosePlatformer)
		{
			EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			endLevelEnd = EditorGUILayout.Toggle("Kill Everyone", endLevelEnd);
			endLevelEnd = EditorGUILayout.Toggle("Get fom A point to B point", !endLevelEnd);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

			if (endLevelEnd) selectedObj = "Win by Finishing Level";
			if (!endLevelEnd) selectedObj = "Play until Death";

		}

		if (choseSurvival)
		{
			EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			survTime = EditorGUILayout.Toggle("Kill Everyone", survTime);
			survTime = EditorGUILayout.Toggle("Survive at a giving Time", !survTime);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

			if (survTime) selectedObj = "Win surviving at a giving time";
			if (!survTime) selectedObj = "Win Killing everyone";

		}

		if (choseEndless)
		{
			EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			killEveryOnePlat = EditorGUILayout.Toggle("End Level", killEveryOnePlat);
			killEveryOnePlat = EditorGUILayout.Toggle("Continue until Death", !killEveryOnePlat);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

			if (killEveryOnePlat) selectedObj = "KILL EVERYONE";
			if (!killEveryOnePlat) selectedObj = "GET TO OBJECTIVE";
		}
	}

	void CreateScriptable()
	{
		if (EditorUtility.DisplayDialog("Attention", "Is this information ok? " + "\n" + "\n" + selectedPers + "\n" + selectedMode + "\n" + selectedObj + "\n" + "LEVELS: " + howManyLevels, "Yes", "No"))
		{
			var a = CreateInstance<GameModeProperties>();
			a.endlessMode = choseEndless;
			a.platformMode = chosePlatformer;
			a.survivalMode = choseSurvival;

			a.isoPers = choseISO;
			a.tpPers = choseTP;
			a.tdPers = choseTD;
			a.horPers = choseSIDE;

            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Data");
                Debug.Log("The introduced folder doesn't exist, so I just created a default one for you.");
                AssetDatabase.Refresh();
            }
            var b = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Data/Level_Properties.asset");

			gameModePage = true;
			propertiesPage = false;

			AssetDatabase.CreateAsset(a, b);

			AssetDatabase.SaveAssets();
		}
	}

	void ChooseTypeOfGame()
	{


		EditorGUILayout.LabelField("Game Mode: ", subtitlestyle);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.BeginVertical();
		GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), endlessExample);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.BeginDisabledGroup(choseEndless);
		if (GUI.Button(GUILayoutUtility.GetRect(50, 50, 50, 50), "ENDLESS"))
		{
			choseEndless = true;
			choseSurvival = false;
			chosePlatformer = false;
			selectedMode = "ENDLESS";
			Repaint();
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical();
		GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), platExample);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.BeginDisabledGroup(chosePlatformer);
		if (GUI.Button(GUILayoutUtility.GetRect(50, 50, 50, 50), "PLATFORMER"))
		{
			choseEndless = false;
			choseSurvival = false;
			chosePlatformer = true;
			selectedMode = "PLATFORMER";
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();
		GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), survExample);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.BeginDisabledGroup(choseSurvival);
		if (GUI.Button(GUILayoutUtility.GetRect(50, 50, 50, 50), "SURVIVAL"))
		{
			choseEndless = false;
			choseSurvival = true;
			chosePlatformer = false;
			selectedMode = "SURVIVAL";
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndVertical();

		EditorGUILayout.EndHorizontal();

	}

	void ChoosePerspective()
	{
		EditorGUILayout.LabelField("Choose Perspective: ", subtitlestyle);
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();


		EditorGUILayout.BeginVertical();
		GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), isoExample);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.BeginDisabledGroup(choseISO);
		if (GUI.Button(GUILayoutUtility.GetRect(50, 50, 50, 50), "Isometric"))
		{
			choseISO = true;
			choseTP = false;
			choseTD = false;
			choseSIDE = false;
			selectedPers = "ISOMETRIC";

		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndVertical();



		EditorGUILayout.BeginVertical();
		GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), tpExample);
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUI.BeginDisabledGroup(choseTP);
		if (GUI.Button(GUILayoutUtility.GetRect(50, 50, 50, 50), "Third Person"))
		{
			choseISO = false;
			choseTP = true;
			choseTD = false;
			choseSIDE = false;
			selectedPers = "THIRD PERSON";
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndVertical();


		EditorGUILayout.BeginVertical();
		GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), sideExample);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.BeginDisabledGroup(choseSIDE);
		if (GUI.Button(GUILayoutUtility.GetRect(50, 50, 50, 50), "Side-Scroller"))
		{
			choseISO = false;
			choseTP = false;
			choseTD = false;
			choseSIDE = true;
			selectedPers = "SIDE-SCROLLER";
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndVertical();



		EditorGUILayout.BeginVertical();
		GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), tdEcample);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.BeginDisabledGroup(choseTD);
		if (GUI.Button(GUILayoutUtility.GetRect(50, 50, 50, 50), "Top Down"))
		{
			choseISO = false;
			choseTP = false;
			choseTD = true;
			choseSIDE = false;
			selectedPers = "TOP DOWN";
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndVertical();


		EditorGUILayout.EndHorizontal();
	}

	void GotoProperties()
	{
		if (GUILayout.Button("NEXT"))
		{
			if (choseTD || choseTP || choseSIDE || choseISO && choseSurvival || choseEndless || chosePlatformer)
			{

				if (EditorUtility.DisplayDialog("Attention", "Is this information ok? " + "\n" + "\n" + selectedPers + "\n" + selectedMode, "Yes", "No"))
				{

					gameModePage = false;
					propertiesPage = true;
					Repaint();

				}


			}
			else
			{
				EditorUtility.DisplayDialog("Oops!", "There's an option you haven't selected.",
			 "Alright");
			}
		}
	}
}
