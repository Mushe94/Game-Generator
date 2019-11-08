using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object = UnityEngine.Object;

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

    GameModeProperties scriptable;
    public List<SceneAsset> myscenes;

	private void OnEnable()
  	{
        myscenes = new List<SceneAsset>();
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
            if (scriptable != null) scriptable.gm = GameMode.platform;
            else scriptable = CreateInstance<GameModeProperties>();
            EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            endLevelEnd = EditorGUILayout.Toggle("Collect Coins", endLevelEnd);
            endLevelEnd = EditorGUILayout.Toggle("Get fom A point to B point", !endLevelEnd);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

            if (endLevelEnd)
            {
                selectedObj = "Win by Finishing Level";
                scriptable.objPlatform = ObjectivePlatformer.GetToPointB;
            }
            if (!endLevelEnd)
            {
                selectedObj = "Collect Coins";
                scriptable.objPlatform = ObjectivePlatformer.CollectCoins;
            }

		}

		if (choseSurvival)
		{
            if (scriptable != null) scriptable.gm = GameMode.survival;
            else scriptable = CreateInstance<GameModeProperties>();
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

            if (survTime)
            {
                selectedObj = "Win surviving at a giving time";
                scriptable.objSurvival = ObjectiveSurvival.BYTIME;
            }
            if (!survTime)
            {
                selectedObj = "Win Killing everyone";
                scriptable.objSurvival = ObjectiveSurvival.BYKILLING;
            }

            

		}

		if (choseEndless)
		{
            if (scriptable != null) scriptable.gm = GameMode.endless;
            else scriptable = CreateInstance<GameModeProperties>();
            EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			killEveryOnePlat = EditorGUILayout.Toggle("Based on Time", killEveryOnePlat);
			killEveryOnePlat = EditorGUILayout.Toggle("Based on Points", !killEveryOnePlat);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

            if (!killEveryOnePlat)
            {
                selectedObj = "Based on Time";
                scriptable.objEndless = ObjectiveEndless.BYTIME;
            }
            if (killEveryOnePlat)
            {
                selectedObj = "Based on Points";
                scriptable.objEndless = ObjectiveEndless.BYPOINTS;
            }
   
        }
	}

	void CreateScriptable()
	{
		if (EditorUtility.DisplayDialog("Attention", "Is this information ok? " + "\n" + "\n" + selectedPers + "\n" + selectedMode + "\n" + selectedObj + "\n" + "LEVELS: " + howManyLevels, "Yes", "No"))
		{

			if(scriptable == null) scriptable = CreateInstance<GameModeProperties>();

            scriptable.amountOfLevels = howManyLevels;

            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Data");
                Debug.Log("The introduced folder doesn't exist, so I just created a default one for you.");
                AssetDatabase.Refresh();
        
            }
            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Data/Game Mode Properties/Level_Properties.asset");

             

			gameModePage = true;
			propertiesPage = false;
            AssetDatabase.CreateAsset(scriptable, path);


            for (int i = 0; i < howManyLevels; i++)
            {
                if(scriptable.pers == Perspective.side) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestSide.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
                if(scriptable.pers == Perspective.top) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestTop.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
                if(scriptable.pers == Perspective.third) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestTop.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
                if(scriptable.pers == Perspective.iso) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestTop.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
              
            }
			AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.OpenScene("Assets/Resources/Prefabs/Level0.unity");

           

            var manager = FindObjectOfType<GameManager>();
            manager.scriptable = scriptable;
            manager.myscenes = new UnityEngine.SceneManagement.Scene[howManyLevels];
            manager.amountScenes = howManyLevels;
            for (int i = 0; i < howManyLevels; i++)
            {
                manager.myscenes[i] = UnityEngine.SceneManagement.SceneManager.GetSceneByPath("Assets/Resources/Prefabs/Level" + i + ".unity");
            }
            scriptable = null;
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
            if (scriptable != null) scriptable.gm = GameMode.endless;
            else
            {
                scriptable = CreateInstance<GameModeProperties>();

            }
    
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
            if (scriptable != null) scriptable.gm = GameMode.platform;
            else scriptable = CreateInstance<GameModeProperties>();
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
            if (scriptable != null) scriptable.gm = GameMode.survival;
            else scriptable = CreateInstance<GameModeProperties>();
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
            if (scriptable != null) scriptable.pers = Perspective.iso;
            else scriptable = CreateInstance<GameModeProperties>();
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
            if (scriptable != null) scriptable.pers = Perspective.third;
            else scriptable = CreateInstance<GameModeProperties>();
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
            if (scriptable != null) scriptable.pers = Perspective.side;
            else scriptable = CreateInstance<GameModeProperties>();
            choseISO = false;
            choseTP = false;
            choseTD = false;
            choseSIDE = true;
            selectedPers = "SIDE-SCROLLER";
            Debug.Log(scriptable.pers);
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
            if (scriptable != null) scriptable.pers = Perspective.top;
            else scriptable = CreateInstance<GameModeProperties>();
            choseISO = false;
            choseTP = false;
            choseTD = true;
            choseSIDE = false;
            selectedPers = "TOP DOWN";
            Debug.Log(scriptable.pers);
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
				EditorUtility.DisplayDialog("Oops!", "There's an option you haven't selected.", "Alright");
			}
		}
	}

}

