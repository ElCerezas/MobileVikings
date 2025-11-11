using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
public class LevelManager : MonoBehaviour
{
    [SerializedDictionary("Character Key", "Tile Prefab")]
    public SerializedDictionary<string, GameObject> prefabDictionary;

    public string levelsPath = "Levels"; //Resources/...
    [SerializeField] int levelIDToLoad = 1;

    [SerializeField]GameObject grid;
    GameObject[,] levelTiles;

    void Start()
    {
        LoadLevel(levelIDToLoad);
    }
    public void LoadLevel(int id)
    {
        #region LoadJSON
        TextAsset jsonData = Resources.Load<TextAsset>(levelsPath);
        if (jsonData == null)
        {
            Debug.LogError("No existeix JSON valid");
            return;
        }

        LevelList allLevels = JsonUtility.FromJson<LevelList>(FixJsonArray(jsonData.text));
        LevelData level = allLevels.levels.FirstOrDefault(l => l.ID == id);

        if (level == null)
        {
            Debug.LogError("No existeix el nivell: " + id);
            return;
        }
        #endregion
        #region Generation
        levelTiles = new GameObject[level.dimensions[0], level.dimensions[1]];
        for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.grid[y];
            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string slotKey = row[x].ToString();
                if (prefabDictionary.ContainsKey(slotKey))
                {
                    Vector3 pos = new Vector3(x, 0, -y);
                    Debug.Log($"{x},{y}: {slotKey} -> {prefabDictionary[slotKey].name}");
                    GameObject i = Instantiate(prefabDictionary[slotKey], pos, Quaternion.identity, grid.transform);
                    levelTiles[x, y] = i;
                }
                else
                {
                    Debug.Log($"Key no trobada -> {slotKey}");
                }
            }
        }
        #endregion
    }
    private string FixJsonArray(string rawJson)
    {
        if (rawJson.TrimStart().StartsWith("{")) return rawJson;
        return "{\"levels\":" + rawJson + "}";
    }
}
