using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Base")]
    public string UnitId;
    public bool isPlayer1;
    [Range(0, 3)] public int tier = 1;

    [Header("Stats")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    [Header("BoardReference")]
    public Tile currentTile;

    [Header("Components")]
    public Ability ability;
}
