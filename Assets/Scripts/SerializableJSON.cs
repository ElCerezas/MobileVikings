using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int ID;
    public int playerSpawnRows;
    public int selectableHeroes;

    public float placingTime;

    public int[] dimensions;
    public List<string> grid;
    public List<string> enemies;
}

[System.Serializable]
public class LevelList
{
    public List<LevelData> levels;
}
