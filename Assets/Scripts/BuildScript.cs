using UnityEditor;

public class BuildScript
{
    public static void BuildWebGL()
    {
        string buildPath = "Builds/WebGL"; // Specify the output directory

        // Ensure the build path exists
        if (!System.IO.Directory.Exists(buildPath))
        {
            System.IO.Directory.CreateDirectory(buildPath);
        }

        // Build settings
        string[] scenes = { "Assets/Scenes/CharacterControllTestScene.unity" }; // Update with scene path(s)

        // Run the build
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);

        UnityEngine.Debug.Log("Build completed successfully.");
    }
}