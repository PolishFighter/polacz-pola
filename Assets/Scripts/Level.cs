using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelObject")]
public class Level : ScriptableObject
{
    [SerializeField]
    public ArrayLayout board;

    public string levelName;
    public int maxTime = 60;
    public int maxColors = 3;

    public void Randomize()
    {
         for(int i = 0; i < board.rows.Length; i++)
        {
            for(int j = 0; j < board.rows[i].row.Length; j++)
            {
                board.rows[i].row[j] = (SquareColor) Random.Range(0, Mathf.Min(maxColors, System.Enum.GetValues(typeof(SquareColor)).Length));
            }
        }
    }
}
