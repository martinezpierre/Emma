using UnityEditor;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class ScriptBatch 
{
	[MenuItem("MyTools/Custom Build")]
	public static void BuildGame ()
	{
		TextManager.LoadText (TestEditor.fileName);

		// Get filename.
		string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

		if (path.Length == 0)
			return;
		
		string[] levels = new string[EditorBuildSettings.scenes.Length];


		int i = 0;
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			levels [i] = scene.path;
			i++;
		}

		// Build player.

		string buildName = "BuiltGame";

		#if UNITY_STANDALONE

		BuildPipeline.BuildPlayer(levels, path + "/" + buildName+".exe", EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

		#else

		BuildPipeline.BuildPlayer(levels, path + "/" + buildName, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

		#endif


	
		#if UNITY_STANDALONE_OSX

		// Copy a file from the project folder to the build folder, alongside the built game.
		System.IO.Directory.CreateDirectory(path+"/BuiltGame.app/Contents/Resources/Save");
		System.IO.File.Copy ("Assets/Resources/Save/frenchsave.txt", path+"/BuiltGame.app/Contents/Resources/Save/frenchsave.txt",true);

		// Run the game (Process class from System.Diagnostics).
		Process proc = new Process();
		proc.StartInfo.FileName = path + "/BuiltGame.app";

		#elif UNITY_STANDALONE

		// Copy a file from the project folder to the build folder, alongside the built game.
		System.IO.Directory.CreateDirectory(path+"/"+buildName+"_Data/Resources/Save");
		System.IO.File.Copy ("Assets/Resources/Save/frenchsave.txt", path+"/"+buildName+"_Data/Resources/Save/frenchsave.txt",true);

		// Run the game (Process class from System.Diagnostics).
		Process proc = new Process();
		proc.StartInfo.FileName = path + "/" + buildName;

		#endif

		proc.Start();
	}
}