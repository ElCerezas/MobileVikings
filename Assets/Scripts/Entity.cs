using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public EntityData data;
    public Tile currentTile;
    public void Initialize(EntityData data)
    {
        this.data = data;
    }
    public void OnTick(int tick)
    {

    }
}