// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
using UnityEditor;

namespace AmplifyShaderEditor
{
	public class TemplateMenuItems
	{
		[MenuItem( "Assets/Create/Amplify Shader/Unlit", false, 85 )]
		public static void ApplyTemplate0()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "0770190933193b94aaa3065e307002fa" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Post-Processing Stack", false, 85 )]
		public static void ApplyTemplate1()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "32139be9c1eb75640a847f011acf3bcf" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Custom RT Init", false, 85 )]
		public static void ApplyTemplate2()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "6ce779933eb99f049b78d6163735e06f" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Custom RT Update", false, 85 )]
		public static void ApplyTemplate3()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "32120270d1b3a8746af2aca8bc749736" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Lightweight PBR", false, 85 )]
		public static void ApplyTemplate4()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "1976390536c6c564abb90fe41f6ee334" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Lightweight Unlit", false, 85 )]
		public static void ApplyTemplate5()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "e2514bdcf5e5399499a9eb24d175b9db" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Post Process", false, 85 )]
		public static void ApplyTemplate6()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "c71b220b631b6344493ea3cf87110c93" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Default Unlit", false, 85 )]
		public static void ApplyTemplate7()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "6e114a916ca3e4b4bb51972669d463bf" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Default UI", false, 85 )]
		public static void ApplyTemplate8()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "5056123faa0c79b47ab6ad7e8bf059a4" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Unlit Lightmap", false, 85 )]
		public static void ApplyTemplate9()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "899e609c083c74c4ca567477c39edef0" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Default Sprites", false, 85 )]
		public static void ApplyTemplate10()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "0f8ba0101102bb14ebf021ddadce9b49" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Particles Alpha Blended", false, 85 )]
		public static void ApplyTemplate11()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "0b6a9f8b4f707c74ca64c0be8e590de0" );
		}
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Multi Pass Unlit", false, 85 )]
		public static void ApplyTemplate12()
		{
			AmplifyShaderEditorWindow.CreateNewTemplateShader( "e1de45c0d41f68c41b2cc20c8b9c05ef" );
		}
	}
}
