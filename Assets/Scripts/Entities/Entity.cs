using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public EntityData data;
    public Tile currentTile;

    public void Initialize(EntityData d)
    {
        data = d;
    }
    public void OnTick(int tick)
    {

    }
}
