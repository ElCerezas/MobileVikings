using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Entities/Enemies", order = 1)]
public class EntityData : ScriptableObject
{
    public string id;
    public GameObject prefab;

    public int hp, attack, turnsUntilAtack, turnsUntilMove;
    [Range(0f, 1f)] public float critChance;
}