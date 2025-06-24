using UnityEditor;
using UnityEngine;

public class CopyComponentsUtility : MonoBehaviour
{
    [MenuItem("Tools/Copy Components From Source")]
    private static void CopyComponents()
    {
        if (Selection.gameObjects.Length < 2)
        {
            Debug.LogWarning("Select the source object first, then the targets.");
            return;
        }

        GameObject source = Selection.gameObjects[0];

        for (int i = 1; i < Selection.gameObjects.Length; i++)
        {
            GameObject target = Selection.gameObjects[i];

            foreach (var component in source.GetComponents<Component>())
            {
                if (component is Transform) continue; // Skip Transform

                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(target);
            }
        }

        Debug.Log("Components copied to selected targets.");
    }
}