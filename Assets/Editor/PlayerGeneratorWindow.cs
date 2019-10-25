using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerGeneratorWindow : EditorWindow
{
    public static void OpenWindow()
	{
		GetWindow<PlayerGeneratorWindow>();
	}
}
