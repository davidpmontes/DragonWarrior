using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public struct MapAndCoordinate
    {
        public string tilemap;
        public Vector2 coordinate;

        public MapAndCoordinate(string tilemap, Vector2 coordinate)
        {
            this.tilemap = tilemap;
            this.coordinate = coordinate;
        }
    }

    public static Grid Instance;

    public Tilemap Overworld;
    public Tilemap Brecconary;

    public Tilemap currentMap;

    private List<string> MoveableTiles;
    private Dictionary<string, Tilemap> StringToTilemap;
    private Dictionary<MapAndCoordinate, MapAndCoordinate> TeleportLookup;
    private Dictionary<string, float> MoveDelay;

    void Start()
    {
        currentMap = Brecconary;

        Instance = this;

        //create a struct for each tile to handle movement slowdown, damage, passability

        MoveableTiles = new List<string>();
        MoveableTiles.Add("barrier");
        MoveableTiles.Add("bricks");
        MoveableTiles.Add("bridge");
        MoveableTiles.Add("cave");
        MoveableTiles.Add("castle");
        MoveableTiles.Add("chest");
        MoveableTiles.Add("desert");
        MoveableTiles.Add("downStart");
        MoveableTiles.Add("hills");
        MoveableTiles.Add("plain");
        MoveableTiles.Add("swamp");
        MoveableTiles.Add("town");
        MoveableTiles.Add("trees");
        MoveableTiles.Add("upStair");
        MoveableTiles.Add("stairs_outdoors");

        MoveDelay = new Dictionary<string, float>();
        MoveDelay.Add("hills", 0.1f);

        PopulateTeleportLookup();

        StringToTilemap = new Dictionary<string, Tilemap>();
        StringToTilemap.Add("Overworld", Overworld);
        StringToTilemap.Add("Brecconary", Brecconary);
    }

    void PopulateTeleportLookup()
    {
        TeleportLookup = new Dictionary<MapAndCoordinate, MapAndCoordinate>();

        TeleportLookup.Add(new MapAndCoordinate("BreconnaryInvisible", new Vector2(-9, -7)), new MapAndCoordinate("Overworld", new Vector2(4, 1)));
    }

    public float GetMoveDelay(Vector3 pos)
    {
        if (MoveDelay.ContainsKey(GetTile(pos)))
        {
            return MoveDelay[GetTile(pos)];
        }
        return 0;
    }

    public string GetTile(Vector3 pos)
    {
        return currentMap.GetTile(new Vector3Int(Mathf.FloorToInt(pos.x),
                                                 Mathf.FloorToInt(pos.y), 0)).name;
    }

    public bool TileIsMoveable(Vector3 pos)
    {
        return MoveableTiles.Contains(GetTile(pos));
    }


    public void TeleportChangeMap(Tilemap newtilemap)
    {
        newtilemap.gameObject.SetActive(true);
        var oldMap = currentMap;
        currentMap = newtilemap;
        oldMap.gameObject.SetActive(false);
    }
}
