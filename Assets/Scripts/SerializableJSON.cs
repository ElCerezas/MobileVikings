using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int ID;
    public int[] dimensions;
    public List<string> grid;
}

[System.Serializable]
public class LevelList
{
    public List<LevelData> levels;
}
