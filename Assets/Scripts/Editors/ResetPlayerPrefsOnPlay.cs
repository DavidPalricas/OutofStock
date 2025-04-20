using UnityEditor;
using UnityEngine;

/// <summary>
/// The ResetPlayerPrefs class is responsible for reseting specific PlayerPrefs keys when entering 
/// Play Mode in the Unity Editor.
/// </summary>
[InitializeOnLoad]
public static class ResetPlayerPrefsOnPlay
{
    /// <summary>
    /// The ResetPlayerPrefsOnPlay constructor, registers the playModeStateChanged event handler when the Unity Editor loads.
    /// </summary>
    static ResetPlayerPrefsOnPlay()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    /// <summary>
    /// The OnPlayModeStateChanged method is called when the play mode state changes in the Unity Editor.
    /// </summary>
    /// <param name="state">The current PlayModeStateChange event.</param>
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            if (PlayerPrefs.HasKey("CurrentDay"))
            {
                PlayerPrefs.DeleteKey("CurrentDay");
                PlayerPrefs.Save();
            }
        }
    }
}