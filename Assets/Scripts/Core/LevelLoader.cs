using AYellowpaper.SerializedCollections;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GridSystem tileGrid;
    [SerializeField] string path;

    [Header("Dictionaries")]
    public SerializedDictionary<string, Vector2Int> terrainDictionary;
    public SerializedDictionary<string, GameObject> enemiesDictionary;

    [Header("Tiles")]
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Vector2Int atlasSize;

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
        Vector2 tileScale = new Vector2(1f / atlasSize.x, 1f / atlasSize.y);

        for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.grid[y];

            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string slotKey = row[x].ToString();

                if (!terrainDictionary.ContainsKey(slotKey))
                {
                    Debug.Log($"Key no trobada -> {slotKey}");
                    continue;
                }

                Vector3 pos = new Vector3(x, 0, -y - 1);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                Tile t = tile.GetComponent<Tile>();
                t.SetTileCoords(x, y);
                t.name = t.x + " , " + t.y;
                tileGrid.RegisterTile(t);

                Renderer renderer = tile.GetComponent<Renderer>();
                Material mat = renderer.material;

                Vector2Int coord = terrainDictionary[slotKey];

                Vector2 offset = new Vector2(coord.x * tileScale.x,coord.y * tileScale.y);

                mat.mainTextureScale = tileScale;
                mat.mainTextureOffset = offset;
                int h = level.dimensions[1];
                int spawn = level.playerSpawnRows;

                if (y < spawn) t.SetOwner(TileOwner.Enemy);
                else if (y >= h - spawn) t.SetOwner(TileOwner.Player);
                else t.SetOwner(TileOwner.Neutral);

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