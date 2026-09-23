#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class PlayFromBeginningTool
{
    private const string START_SCENE_PATH = "Assets/CreatureCatcherProject/Scenes/MainMenu.unity";

    private const string PREVIOUS_SCENE_KEY = "PlayFromBeginning_PreviousScene";

    private const string USED_TOOL_KEY = "PlayFromBeginning_UsedTool";

    static PlayFromBeginningTool()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    [MenuItem("Tools/Play From Beginning #b")]
    public static void PlayFromBeginning()
    {
        // Don't do anything if we're already playing
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("Can't Play because the editor is already playing a scene.");
            return;
        }

        Scene currentScene = SceneManager.GetActiveScene();

        if (string.IsNullOrEmpty(currentScene.path))
        {
            Debug.LogWarning("Save the current scene before using Play From Beginning.");
            return;
        }

        // Ask to save any modified scenes first
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        // Remember where we were
        SessionState.SetString(PREVIOUS_SCENE_KEY, currentScene.path);

        SessionState.SetBool(USED_TOOL_KEY, true);

        // Open the game's starting scene
        EditorSceneManager.OpenScene(START_SCENE_PATH, OpenSceneMode.Single);

        // Start playing
        EditorApplication.isPlaying = true;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredEditMode) return;
        if (!SessionState.GetBool(USED_TOOL_KEY, false)) return;

        string previousScene = SessionState.GetString(PREVIOUS_SCENE_KEY, "");

        // Clear this immediately so normal Play doesn't trigger anything
        SessionState.SetBool(USED_TOOL_KEY, false);
        SessionState.EraseString(PREVIOUS_SCENE_KEY);

        if (string.IsNullOrEmpty(previousScene)) return;

        EditorSceneManager.OpenScene(previousScene, OpenSceneMode.Single);
    }
}

#endif
