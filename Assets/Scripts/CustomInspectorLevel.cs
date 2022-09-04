#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class CustomInspectorLevel : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Level level = (Level)target;

        if(GUILayout.Button("Randomize board"))
        {
           level.Randomize();
        }
    }
}
#endif
