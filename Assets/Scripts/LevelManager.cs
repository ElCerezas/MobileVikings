using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
public class LevelManager : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] int levelIDToLoad = 1;
    public SerializedDictionary<string, GameObject> prefabDictionary;
    public string levelsPath = "Levels"; //Resources/...

    [SerializeField] Quaternion rotation;

    [Header("SpawnAnimation")]
    [SerializeField] float spawnTime;
    [SerializeField] float spawnIncrease;
    [SerializeField] AnimationCurve spawnCurve;

    [Header("Management")]
    bool levelLoaded = false;
    [SerializeField] GameObject grid;
    TileController[,] levelTiles;

    void Start()
    {
        //LoadLevel(levelIDToLoad);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !levelLoaded) LoadLevel(1);
        if(Input.GetKeyDown(KeyCode.Alpha2) && !levelLoaded) LoadLevel(2);
        if(Input.GetKeyDown(KeyCode.Alpha3) && !levelLoaded) LoadLevel(3);
        if(Input.GetKeyDown(KeyCode.Delete) && levelLoaded) DestroyLevel();
    }
    #region Generation
    void LoadLevel(int id)
    {
        levelLoaded = true;
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
        float delay = spawnIncrease;
        levelTiles = new TileController[level.dimensions[0], level.dimensions[1]];
        for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.grid[y];
            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string slotKey = row[x].ToString();
                if (prefabDictionary.ContainsKey(slotKey))
                {
                    delay = spawnIncrease * Mathf.Min(level.dimensions[0] - x, level.dimensions[1] - y);
                    Vector3 pos = new Vector3(x, -1, -y);
                    GameObject i = Instantiate(prefabDictionary[slotKey], pos, rotation, grid.transform);
                    levelTiles[x, y] = i.GetComponent<TileController>();
                    StartCoroutine(i.GetComponent<TileController>().SummonTile(spawnTime, delay, spawnCurve));
                    
                }
                else
                {
                    Debug.Log($"Key no trobada -> {slotKey}");
                }
            }
        }
        #endregion
    }
    void DestroyLevel()
    {
        foreach (var level in levelTiles)
        {
            level.StopAllCoroutines();
            Destroy(level.gameObject);
        }
        levelLoaded = false;
    }
    #endregion
    private string FixJsonArray(string rawJson)
    {
        if (rawJson.TrimStart().StartsWith("{")) return rawJson;
        return "{\"levels\":" + rawJson + "}";
    }
}
