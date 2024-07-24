/*
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Class ensuring that the StartupScene is launched on Play Mode.
/// </summary>
[InitializeOnLoad]
class PlayStartupScene
{
    /// <summary>
    /// Scene the user is working in before entering PlayMode
    /// </summary>
    static string originalScenePath;

    /// <summary>
    /// Path to the startup scene.
    /// </summary>
    static readonly string startutpScenePath = "Assets/PropHunt/Scenes/Startup.unity";


    static PlayStartupScene()
    {
        EditorApplication.playModeStateChanged += ChangeScene;
    }


    /// <summary>
    /// Check play mode state to launch startup scene on playMode and reload the original scene when exiting play mode.
    /// </summary>
    /// <param name="state"></param>
    private static void ChangeScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.ExitingEditMode)
            return;
        if(state == PlayModeStateChange.EnteredPlayMode && SceneManager.GetActiveScene() != SceneManager.GetSceneByPath(startutpScenePath))
        {
            originalScenePath = EditorSceneManager.GetActiveScene().path;
            SceneManager.LoadScene(startutpScenePath);
            return;
        }
        if(state == PlayModeStateChange.ExitingPlayMode && originalScenePath != null)
        {
            SceneManager.LoadScene(originalScenePath);
        }
    }
}*/

