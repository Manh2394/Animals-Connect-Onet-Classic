using UnityEngine;
using System.Collections;
using UnityEditor;

public class CameraZoom : EditorWindow
{
    private int m_start = 1;
    private int m_end = 10;
    float m_scale = 1.0f;
    private float originalOrthorSize = 3.2f;

    [MenuItem("Window/Camera Zoom")]
    static void ShowTimeScalerWindow()
    {
        EditorWindow.GetWindow (typeof (CameraZoom));
    }

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        m_scale = EditorGUILayout.Slider("Camera Zoom", m_scale, m_start, m_end);

        if (GUI.changed)
        {
            if (Camera.main != null) {
                Camera.main.orthographicSize = originalOrthorSize / m_scale;
            }
        }
    }
}
