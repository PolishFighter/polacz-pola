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
            for(int i = 0; i < level.board.rows.Length; i++)
            {
                for(int j = 0; j < level.board.rows[i].row.Length; j++)
                {
                    level.board.rows[i].row[j] = (SquareColor) Random.Range(0, System.Enum.GetValues(typeof(SquareColor)).Length);
                }
            }
        }
    }
}
#endif
