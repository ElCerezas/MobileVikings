using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
enum LevelPhase
{
    Generate, SelectClass, SelectSubClass, PlaceHeroes, Combat, EndLevel
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] LevelPhase phase = LevelPhase.Generate;

    [Header("CustomUpdate")]
    [SerializeField] float timeUntilTurn = 1f;
    [SerializeField] float actualTime = 1f;
    public static event Action<float> OnGameUpdate;

    [Header("Lists of Characters")]
    [SerializeField] GameObject grid;
    [SerializeField] List<CharacterBase> enemmies;
    [SerializeField] List<CharacterBase> heroes;
    TileController[,] levelTiles;

    [Header("Dictionaries")]
    public SerializedDictionary<string, GameObject> terrainDictionary;
    public SerializedDictionary<string, GameObject> enemiesDictionary;
    public SerializedDictionary<string, GameObject> heroesDictionary;

    [Header("Generation")]
    public string levelsPath = "Levels"; //Resources/...
    [SerializeField] Quaternion rotation;

    [Header("SpawnAnimation")]
    [SerializeField] float spawnTime;
    [SerializeField] float spawnIncrease;
    [SerializeField] AnimationCurve spawnCurve;
    [SerializeField] float gridOffsetY = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LoadLevel();
    }
    void Update()
    {
        if (phase == LevelPhase.Combat)
        {
            actualTime += Time.deltaTime;
            if (actualTime > timeUntilTurn)
            {
                actualTime -= timeUntilTurn;
            }
        }
        
    }
    public void LoadLevel()
    {
        int id = 2; //TO FIX
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
        float delay = spawnIncrease;
        levelTiles = new TileController[level.dimensions[0], level.dimensions[1]];
        grid.transform.position = new Vector3(-(level.dimensions[1] / 2f), 0, (level.dimensions[1] + gridOffsetY));
        for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.grid[y];
            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string slotKey = row[x].ToString();
                if (terrainDictionary.ContainsKey(slotKey))
                {
                    delay = spawnIncrease * Mathf.Min(level.dimensions[0] - x, level.dimensions[1] - y);
                    Vector3 pos = new Vector3(x, -1, -y-1);
                    GameObject i = Instantiate(terrainDictionary[slotKey], pos, rotation, grid.transform);   
                    i.transform.localPosition = pos;
                    levelTiles[x, y] = i.GetComponent<TileController>();
                    levelTiles[x,y].SetCoord(x, y);
                    if (y > level.dimensions[1] - level.playerSpawnRows)
                    {
                        levelTiles[x, y].PlayerCanSpawn = true;
                        StartCoroutine(levelTiles[x, y].SummonTile(spawnTime, delay, spawnCurve));
                    }
                    
                }
                else
                {
                    Debug.Log($"Key no trobada -> {slotKey}");
                }
            }
        }
        for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.enemies[y];
            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string enemyKey = row[x].ToString();
                if (enemiesDictionary.ContainsKey(enemyKey))
                {
                    GameObject i = Instantiate(enemiesDictionary[enemyKey], Vector3.zero, Quaternion.identity, levelTiles[x, y].transform);
                    i.transform.position = levelTiles[x, y].terrainFloor.position;
                    enemmies.Add(i.GetComponent<CharacterBase>());
                    levelTiles[x, y].SetCharacter(i.GetComponent<CharacterBase>()); 
                }
                else
                {
                    Debug.Log(enemyKey + "-> Not Found");
                }
            }
        }
    }
    private string FixJsonArray(string rawJson)
    {
        if (rawJson.TrimStart().StartsWith("{")) return rawJson;
        return "{\"levels\":" + rawJson + "}";
    }
}