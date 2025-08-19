#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class SteamAppIDPostBuild
{
    private const string steamAppID = "480";

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        string buildFolder = Path.GetDirectoryName(pathToBuiltProject);

        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        {
            string appidPath = Path.Combine(buildFolder, "steam_appid.txt");
            File.WriteAllText(appidPath, steamAppID);
            Debug.Log("steam_appid.txt created at: " + appidPath);
        }
    }
}
#endif
