using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class FindUnusedAssets : EditorWindow
{
    bool groupEnabled = false;
    List<string> usedAssets = new List<string> ();
    List<string> includedDependencies = new List<string> ();
    private Vector2 scrollPos;
    private List<Object> unUsed;
    private Dictionary<string, List<Object>> unUsedArranged;
    private bool needToBuild = false;

    // Add menu named "CleanUpWindow" to the Window menu
    [MenuItem ("Window/Find Unused Assets")]
    static void Init ()
    {
        // Get existing open window or if none, make a new one:
        FindUnusedAssets window = (FindUnusedAssets)EditorWindow.GetWindow (typeof(FindUnusedAssets));
        window.Show ();
    }

    void OnGUI ()
    {
        if (needToBuild) {
            GUI.color = Color.red;
            GUILayout.Label ("Build project once for each check!", EditorStyles.wordWrappedLabel);
        }
        GUI.color = Color.white;

        if (GUILayout.Button ("Find")) {
            loadEditorLog ();
        }


        if (!needToBuild) {
            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.BeginVertical ();
            if (groupEnabled) {
                GUILayout.Label ("DEPENDENCIES");
                for (int i = 0; i < includedDependencies.Count; i++) {
                    EditorGUILayout.LabelField (i.ToString (), includedDependencies [i]);
                }
            }
            EditorGUILayout.EndVertical ();
            scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
            EditorGUILayout.BeginVertical ();

            if (groupEnabled) {
                if (unUsedArranged != null) {
                    foreach (KeyValuePair<string, List<Object>> objList in unUsedArranged) {
                        if (objList.Value.Count >= 1) {
                            GUILayout.Label (objList.Key.ToUpper ());
                            for (int i = 0; i < objList.Value.Count; i++) {
                                EditorGUILayout.ObjectField (objList.Value [i], typeof(Object), false);
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical ();
            EditorGUILayout.EndScrollView ();
            EditorGUILayout.EndHorizontal ();
        }

    }

    private void loadEditorLog ()
    {
        UsedAssets.GetLists (ref usedAssets, ref includedDependencies);

        if (usedAssets.Count == 0) {
            needToBuild = true;
        } else {
            compareAssetList (UsedAssets.GetAllAssets ());
            groupEnabled = true;
            needToBuild = false;
        }
    }

    private void compareAssetList (string[] assetList)
    {

        unUsed = new List<Object> ();

        unUsedArranged = new Dictionary<string, List<Object>> ();
        unUsedArranged.Add ("plugins", new List<Object> ());
        unUsedArranged.Add ("editor", new List<Object> ());
        unUsedArranged.Add ("some other folder", new List<Object> ());

        for (int i = 0; i < assetList.Length; i++) {
            if (!usedAssets.Contains (assetList [i])) {

                Object objToFind = AssetDatabase.LoadAssetAtPath (assetList [i], typeof(Object));
                unUsed.Add (objToFind);
                if (objToFind != null) {
                    unUsedArranged [getArrangedPos (objToFind)].Add (objToFind);
                }
            }
        }
    }

    private string getArrangedPos (Object value)
    {
        string path = AssetDatabase.GetAssetPath (value).ToLower ();

        if (path.Contains ("/plugins/")) {
            return "plugins";
        } else if (path.Contains ("/editor/")) {
            return "editor";
        } else {
            return "some other folder";
        }
    }
}
