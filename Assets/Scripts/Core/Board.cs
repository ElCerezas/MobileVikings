using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Board : MonoBehaviour
{
    public int w;
    public int h;

    public Tile[,] tiles {  get; private set; }
    [SerializeField] Tile tile;


    public int player1Power = 3;
    public int player2Power = 3;

    private void Start()
    {
        GenerateLevel();
    }
    public bool IsExistingTile(int gridX, int gridY)
    {
        return gridX >= 0 && gridX < w && gridY >= 0 && gridY < h;
    }

    public Tile GetTile(int gridX, int gridY)
    {
        if (!IsExistingTile(gridX, gridY)) 
            return null;

        return tiles[gridX, gridY];
    }

    public void GenerateLevel(LevelData level = null)
    {
        bool white = true;
        //No IA
        if (level == null)
        {
            //Tiles
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Vector2 pos = new Vector2(x, -y - 1);
                    GameObject i = Instantiate(tile.gameObject, pos, Quaternion.identity);
                    i.name = ($"Tile: {x}{y}");
                    i.GetComponent<SpriteRenderer>().color = white ? Color.azure : Color.beige;

                    if (white)
                        white = false;
                    else
                        white = true;
                }
            }
        }
        else
        {
            //Tiles
            for (int y = 0; y < level.dimensions[1]; y++)
            {
                for (int x = 0; x < level.dimensions[0]; x++)
                {
                    Vector2 pos = new Vector2(x, -y - 1);
                    GameObject i = Instantiate(tile.gameObject, pos, Quaternion.identity);
                    i.name = ($"Tile: {x}{y}");
                }
            }
            /*
            for (int y = 0; y < level.dimensions[1]; y++)
            {
                string row = level.enemies[y];
                for (int x = 0; x < level.dimensions[0]; x++)
                {
                    string enemyKey = row[x].ToString();
                    if (enemiesDictionary.ContainsKey(enemyKey))
                    {
                        Tile tile = tileGrid.GetTile(x, y);
                        GameObject i = Instantiate(enemiesDictionary[enemyKey], Vector3.zero, Quaternion.identity, tile.transform);
                        i.transform.position = tile.GetGroundPos();

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
    }
    private string FixJsonArray(string rawJson)
    {
        if (rawJson.TrimStart().StartsWith("{")) return rawJson;
        return "{\"levels\":" + rawJson + "}";
    }
}

[System.Serializable]
public class LevelList
{
    public List<LevelData> levels;
}
