using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public int seed;
    public bool randomSeed;

    [Range(1, 6)]
    public int floorSize;
    public int totalFloorCount;

    public enum BlueprintType { Caverns, Dungeon };
    public BlueprintType blueprintType;

    public List<Vector3Int> floorList = new List<Vector3Int>();

    public Tile[] floorTile, wallTile;

    CavernsBlueprint cavernsBlueprint;
    DungeonBlueprint dungeonBlueprint;

    public static TileManager instance;
    public Tilemap tilemap;
    public Dictionary<Vector3, GameTile> tiles;

    void Start()
    {
        instance = this;

        if (randomSeed) { seed = Random.Range(0,99999); }

        Random.InitState(seed);

        cavernsBlueprint = GetComponent<CavernsBlueprint>();
        dungeonBlueprint = GetComponent<DungeonBlueprint>();

        switch(blueprintType)
        {
            case BlueprintType.Caverns: cavernsBlueprint.RandomWalker(); break;
            case BlueprintType.Dungeon: dungeonBlueprint.RoomWalker(); break;
        }
    }

    public void AddTile(Vector3Int tilePos, Tile tileBase, GameTile.TileType tileType)
    {
        tilemap.SetTile(tilePos, tileBase);

        var tile = new GameTile
        {
            LocalPosition = tilePos,
            WorldPosition = tilemap.CellToWorld(tilePos),

            TileBase = tilemap.GetTile(tilePos),
            TilemapMember = tilemap,

            TileName = tilePos.x + "," + tilePos.y,
            tileType = tileType
        };
        
        GameTile foundTile;

        if(tiles.TryGetValue(tilePos, out foundTile))
        {
            tiles.Remove(tilePos);
        }
        tiles.Add(tilePos, tile);
    }

    public GameTile GetTIle(Vector3Int tilePos)
    {
        GameTile tile;
        tiles.TryGetValue(tilePos, out tile);

        return tile;
    }

    public Vector3Int RandomDirection()
    {
        switch (Random.Range(1, 5))
        {
            case 1: return new Vector3Int(0, 1 + (floorSize * 2), 0);
            case 2: return new Vector3Int(1 + (floorSize * 2), 0, 0);
            case 3: return new Vector3Int(0, -1 - (floorSize * 2), 0);
            case 4: return new Vector3Int(-1 -(floorSize * 2), 0, 0);
        }
        return Vector3Int.zero;
    }

    public bool InFloorList(Vector3Int myPos)
    {
        for (int i = 0; i < floorList.Count; i++) 
        {
            if(Vector3Int.Equals(myPos, floorList[i])) { return true; }
        }
        return false;
    }

    public IEnumerator CreateBlueprint()
    {
        tiles = new Dictionary<Vector3, GameTile>();

        int i = 0;
        while(i < floorList.Count)
        {
            var localPos = new Vector3Int(floorList[i].x, floorList[i].y, floorList[i].z);
            AddTile(localPos, floorTile[Random.Range(0, floorTile.Length)], GameTile.TileType.Floor);

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    Vector3Int wallPos = new Vector3Int(localPos.x + x, localPos.y + y, localPos.z);
                    if (tilemap.HasTile(wallPos)) continue;
                    AddTile(wallPos, wallTile[Random.Range(0, wallTile.Length)], GameTile.TileType.Wall);
                }
            }
            i++;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
