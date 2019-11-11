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
        myscenes = new List<SceneAsset>(); //TEST, YO NO ME ENCARGO DE LAS ESCENAS

        //GUISTYLES VARIOS
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
        //GUISTYLES VARIOS


        //CARGO LAS IMAGENES DE LAS PREVIEWS
        survExample = (Texture2D)Resources.Load("Survival_Example");
		endlessExample = (Texture2D)Resources.Load("Endless_Example");
		platExample = (Texture2D)Resources.Load("Platformer_Example");

		tpExample = (Texture2D)Resources.Load("TP_Example");
		tdEcample = (Texture2D)Resources.Load("TD_Example");
		isoExample = (Texture2D)Resources.Load("Iso_Example");
		sideExample = (Texture2D)Resources.Load("Side_Example");
        //CARGO LAS IMAGENES DE LAS PREVIEWS

        //SETEO FALSO TODOS LOS MODOS DE JUEGO
        choseSurvival = false;
		choseEndless = false;
		chosePlatformer = false;
        //SETEO FALSO TODOS LOS MODOS DE JUEGO

        //MI VENTANA TIENE DOS PAGINAS, UNA DONDE ELEGIS MODO DE JUEGO, OTRA DONDE ELEGIS SUS PROPIEDADES, DEPENDIENDO DEL MODO, EMPIEZA POR EL MODO DE JUEGO
        gameModePage = true;
		propertiesPage = false;
        //MI VENTANA TIENE DOS PAGINAS, UNA DONDE ELEGIS MODO DE JUEGO, OTRA DONDE ELEGIS SUS PROPIEDADES, DEPENDIENDO DEL MODO, EMPIEZA POR EL MODO DE JUEGO


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

        if (chosePlatformer) //SI ELIGIO PLATAFORMERO DIFERENTES OBJETIVOS PARA GANAR NIVEL
        {
            if (scriptable != null) scriptable.gm = GameMode.platform;
            else scriptable = CreateInstance<GameModeProperties>();
            EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            endLevelEnd = EditorGUILayout.Toggle("Collect Coins", endLevelEnd);//GANAR COLECTANDO MONEDAS
            endLevelEnd = EditorGUILayout.Toggle("Get fom A point to B point", !endLevelEnd);//GANAR LLEGANDO AL OTRO PUNTO

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

            if (endLevelEnd) //GANAR LLEGANDO AL OTRO PUNTO
            {
                selectedObj = "Win by Finishing Level";
                scriptable.objPlatform = ObjectivePlatformer.GetToPointB;
            }
            if (!endLevelEnd)//GANAR COLECTANDO MONEDAS
            {
                selectedObj = "Collect Coins";
                scriptable.objPlatform = ObjectivePlatformer.CollectCoins;
            }

		}

		if (choseSurvival) //SI ELIGIO SURVIVAL DIFERENTES OBJETIVOS PARA GANAR NIVEL
        {
            if (scriptable != null) scriptable.gm = GameMode.survival;
            else scriptable = CreateInstance<GameModeProperties>();
            EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			survTime = EditorGUILayout.Toggle("Kill Everyone", survTime);//GANA MATANDO A CIERTA CANTIDAD DE ENMIES
            survTime = EditorGUILayout.Toggle("Survive at a giving Time", !survTime);// GANA SOBREVIVIENDO UN TIEMPO

            EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

            if (survTime) // GANA SOBREVIVIENDO UN TIEMPO
            {
                selectedObj = "Win surviving at a giving time";
                scriptable.objPlatform = ObjectivePlatformer.BYTIME;
            }
            if (!survTime)//GANA MATANDO A CIERTA CANTIDAD DE ENMIES
            {
                selectedObj = "Win Killing everyone";
                scriptable.objPlatform = ObjectivePlatformer.BYKILLING;
            }

            

		}

		if (choseEndless)
		{
            if (scriptable != null) scriptable.gm = GameMode.endless;
            else scriptable = CreateInstance<GameModeProperties>();
            EditorGUILayout.LabelField("Choose Objective ", subtitlestyle);
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			killEveryOnePlat = EditorGUILayout.Toggle("Based on Time", killEveryOnePlat);// GANA SOBREVIVIENDO UN TIEMPO
            killEveryOnePlat = EditorGUILayout.Toggle("Based on Points", !killEveryOnePlat); //GANA CONSIGUIENDO PUNTOS

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("How many Levels?", subtitlestyle);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			howManyLevels = EditorGUILayout.IntField("Levels: ", howManyLevels);

            if (!killEveryOnePlat)
            {
                selectedObj = "Based on Time";// GANA SOBREVIVIENDO UN TIEMPO
                scriptable.objPlatform = ObjectivePlatformer.BYTIME;
            }
            if (killEveryOnePlat)
            {
                selectedObj = "Based on Points";// GANA CONSIGUIENDO PUNTOS
                scriptable.objPlatform = ObjectivePlatformer.BYPOINTS;
            }
   
        }

        //LO SELECCIONADO SE GUARDA EN UN ENUM QUE ES "GAMEMODE" y "PERSPECTIVE"
	}

	void CreateScriptable()
	{
        //ACA TE TIRA UN CARTEL DE UNITY QUE NO TE DEJA HACER NADA
		if (EditorUtility.DisplayDialog("Attention", "Is this information ok? " + "\n" + "\n" + selectedPers + "\n" + selectedMode + "\n" + selectedObj + "\n" + "LEVELS: " + howManyLevels, "Yes", "No"))
		{

			if(scriptable == null) scriptable = CreateInstance<GameModeProperties>(); //SI NO SE CREO ANTES, SE CREA AHORA EL SCRIPTABLEOBJ

            scriptable.amountOfLevels = howManyLevels;

            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data")) 
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Data");
                Debug.Log("The introduced folder doesn't exist, so I just created a default one for you.");
                AssetDatabase.Refresh();
        
            }
            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Data/Game Mode Properties/Level_Properties.asset"); //SE CREA UN PATH DONDE GUARDAR EL SCRIPTABLE

             

			gameModePage = true;
			propertiesPage = false; //SE VUELVE A LA PAGINA DONDE SE ELIGE EL GAMEMODE
            AssetDatabase.CreateAsset(scriptable, path); //SE PONE EL SCRIPTABLE EN EL PATH


            for (int i = 0; i < howManyLevels; i++) //ACA SE CREAN LAS ESCENAS DEPENDIENDO LA PERSPECTIVA CON EL NOMBRE LEVEL0
            {
                if(scriptable.pers == Perspective.side && scriptable.gm == GameMode.endless) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestEndlessSide.unity", "Assets/Resources/Prefabs/Level" + i + ".unity");
                else AssetDatabase.CopyAsset("Assets/Scenes/MatiTestSide.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
                if(scriptable.pers == Perspective.top) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestTop.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
                if(scriptable.pers == Perspective.third) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestTop.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
                if(scriptable.pers == Perspective.iso) AssetDatabase.CopyAsset("Assets/Scenes/MatiTestTop.unity", "Assets/Resources/Prefabs/Level" +i + ".unity");
              
            }
			AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.OpenScene("Assets/Resources/Prefabs/Level0.unity"); //SE ABRE EL PRIMER NIVEL

           

            var manager = FindObjectOfType<GameManager>();// SETEO LOS VALORES DEL MANAGER DEL NUEVO NIVEL
            manager.scriptable = scriptable; 
            manager.myscenes = new UnityEngine.SceneManagement.Scene[howManyLevels];
            manager.amountScenes = howManyLevels;
            for (int i = 0; i < howManyLevels; i++)  //ACA QUISE CONSEGUIR TODAS LAS ESCENAS CREADAS Y GUARDARLAS EN UNA VARIABLE EN EL MANAGER DEL NIVEL
            {
                manager.myscenes[i] = UnityEngine.SceneManagement.SceneManager.GetSceneByPath("Assets/Resources/Prefabs/Level" + i + ".unity");
            }
            scriptable = null; //EL SCRIPTABLE DE LA VENTANA SE HACE NULL, Y SE VA A CREAR UNA DE CERO
		}
	}
   
	void ChooseTypeOfGame()
	{
        //ME CANSE DE HACER COMENTARIOS

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

