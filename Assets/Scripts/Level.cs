using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelObject")]
public class Level : ScriptableObject
{
    [SerializeField]
    public ArrayLayout board;

    public string levelName;
    public int maxTime = 60;
}
