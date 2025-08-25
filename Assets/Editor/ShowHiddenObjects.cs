using UnityEditor;
using UnityEngine;

public class ShowHiddenObjects
{
    [MenuItem("Tools/Show All Hidden Objects")]
    static void ShowAllHiddenObjects()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if ((go.hideFlags & HideFlags.HideInHierarchy) != 0)
                go.hideFlags &= ~HideFlags.HideInHierarchy;
            if ((go.hideFlags & HideFlags.HideAndDontSave) != 0)
                go.hideFlags &= ~HideFlags.HideAndDontSave;
        }
        EditorApplication.RepaintHierarchyWindow();
    }
}
