using UnityEngine;

[CreateAssetMenu(fileName = "TileConfig", menuName = "ScriptableObjects/PuzzleConfigSO", order = 1)]
public class PuzzleConfigSO : ScriptableObject
{
    public string tileType;
    public Sprite Image;
    public GameObject spawnItem;
}