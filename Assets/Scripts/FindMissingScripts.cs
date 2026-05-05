// Put this in an Editor folder: Assets/Editor/FindMissingScripts.cs
using UnityEngine;
using UnityEditor;

public class FindMissingScripts : Editor
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void Find()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject go in allObjects)
        {
            foreach (Component c in go.GetComponents<Component>())
            {
                if (c == null)
                    Debug.LogError($"Missing script on: {go.name}", go);
            }
        }
    }
}