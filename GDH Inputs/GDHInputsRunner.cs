#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
/// <summary>
/// Automatically update GDHInputs each frame
/// </summary>
public class GDHInputRunner : MonoBehaviour
{
    private void Awake() => DontDestroyOnLoad(gameObject);
    private void Update() => GDHInputs.Update();
}
#if UNITY_EDITOR
[CustomEditor(typeof(GDHInputRunner))]
public class GDHInputRunnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var Rect = GUILayoutUtility.GetRect(40, 40);
        EditorGUI.DrawRect(Rect, new Color(0.15f, 0.15f, 0.25f));

        var Style = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.cyan },
            fontStyle = FontStyle.Bold
        };

        GUI.Label(Rect, "Join The Discord", Style);

        if (Event.current.type == EventType.MouseDown && Rect.Contains(Event.current.mousePosition))
            Application.OpenURL("https://discord.gg/gorillasdevhub");

        GUILayout.Space(10);
        GUILayout.Label("Gorillas Dev Hub", Style);
        GUILayout.Space(6);
        GUILayout.Label("Made By ThatOneGorilla", Style);
        GUILayout.Space(10);
    }
}
#endif