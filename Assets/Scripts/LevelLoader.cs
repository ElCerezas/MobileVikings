using AYellowpaper.SerializedCollections;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GridSystem tileGrid;
    [SerializeField] Quaternion rotation;
    [SerializeField] string path;

    [Header("Dictionaries")]
    public SerializedDictionary<string, GameObject> terrainDictionary;
    public SerializedDictionary<string, GameObject> enemiesDictionary;

    public LevelData LoadLevelFromResources(int id)
    {
        TextAsset jsonData = Resources.Load<TextAsset>(path);
        if (jsonData == null)
        {
            Debug.LogError("No existeix JSON valid");
            return null;
        }

        LevelList allLevels = JsonUtility.FromJson<LevelList>(FixJsonArray(jsonData.text));
        LevelData level = allLevels.levels.FirstOrDefault(l => l.ID == id);
        return level;
    }
    public void GenerateLevel(LevelData level)
    {
        tileGrid.Initialize(level.dimensions[0], level.dimensions[1]);
        //Tiles
        for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.grid[y];
            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string slotKey = row[x].ToString();
                if (terrainDictionary.ContainsKey(slotKey))
                {
                    Vector3 pos = new Vector3(x, 0, -y - 1);
                    GameObject i = Instantiate(terrainDictionary[slotKey], pos, rotation);
                }
                else
                {
                    Debug.Log($"Key no trobada -> {slotKey}");
                }
            }
        }

        //Entities
        /*for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.enemies[y];
            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string enemyKey = row[x].ToString();
                if (enemiesDictionary.ContainsKey(enemyKey))
                {
                    Tile tile = tileGrid.GetTile(x, y);
                    GameObject i = Instantiate(enemiesDictionary[enemyKey], Vector3.zero, Quaternion.identity);
                    i.transform.position = tile.gameObject.transform.position;

                    Entity entity = i.GetComponent<Entity>();
                    if (entity == null)
                    {
                        Debug.LogError($"Prefab does not contain an Entity component.");
                        continue;
                    }

                    tile.occupant = entity;
                    entity.currentTile = tile;
                    tileGrid.enemies.Add(i.GetComponent<Entity>());
                }
                else
                {
                    Debug.Log(enemyKey + "-> Not Found");
                }
            }
        }*/
    }
    private string FixJsonArray(string rawJson)
    {
        if (rawJson.TrimStart().StartsWith("{")) return rawJson;
        return "{\"levels\":" + rawJson + "}";
    }
}