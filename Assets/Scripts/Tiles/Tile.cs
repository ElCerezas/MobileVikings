using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tile : MonoBehaviour
{
    public int x, y;
    public GameObject groundPoint;
    bool walkable;
    public Entity occupant;
    public List<Tile> neighbors;
    public bool IsFree()
    {
        return walkable && occupant == null;
    }
    public Vector3 GetGroundPos()
    {
        return groundPoint.transform.position;
    }
}
