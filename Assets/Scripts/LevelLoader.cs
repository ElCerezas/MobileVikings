using AYellowpaper.SerializedCollections;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] TileGrid tileGrid;
    [SerializeField] Quaternion rotation;

    [Header("Dictionaries")]
    public SerializedDictionary<string, GameObject> terrainDictionary;
    public SerializedDictionary<string, GameObject> enemiesDictionary;

    public LevelData LoadLevelFromResources(string levelsPath, int id)
    {
        TextAsset jsonData = Resources.Load<TextAsset>(levelsPath);
        if (jsonData == null)
        {
            Debug.LogError("No existeix JSON valid");
        }

        LevelList allLevels = JsonUtility.FromJson<LevelList>(FixJsonArray(jsonData.text));
        LevelData level = allLevels.levels.FirstOrDefault(l => l.ID == id);
        return level;
    }
    public void GenerateLevel(LevelData level)
    {
        tileGrid.Initialize(level.dimensions[0], level.dimensions[1]);
        tileGrid.SetGridPosition(level);
        for (int y = 0; y < level.dimensions[1]; y++)
        {
            string row = level.grid[y];
            for (int x = 0; x < level.dimensions[0]; x++)
            {
                string slotKey = row[x].ToString();
                if (terrainDictionary.ContainsKey(slotKey))
                {
                    Vector3 pos = new Vector3(x, 0, -y - 1);
                    GameObject i = Instantiate(terrainDictionary[slotKey], pos, rotation, tileGrid.transform);
                    i.transform.localPosition = pos;
                }
                else
                {
                    Debug.Log($"Key no trobada -> {slotKey}");
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