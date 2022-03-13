using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBlueprint : MonoBehaviour
{
    TileManager tileManager;
    private void Awake()
    {
        tileManager = GetComponent<TileManager>();
    }

    public void RoomWalker()
    {
        Vector3Int currentPos = Vector3Int.zero;

        tileManager.floorList.Add(currentPos);
        for (int x = -tileManager.floorSize; x <= tileManager.floorSize; x++)
        {
            for (int y = -tileManager.floorSize; y <= tileManager.floorSize; y++)
            {
                Vector3Int offset = new Vector3Int(x, y, 0);
                if(!tileManager.InFloorList(currentPos + offset))
                {
                    tileManager.floorList.Add(currentPos + offset);
                }
            }
        }

        while(tileManager.floorList.Count < tileManager.totalFloorCount)
        {
            currentPos = Walker(currentPos);
            RandomRoom(currentPos);
        }
        StartCoroutine(tileManager.CreateBlueprint());
    }

    Vector3Int Walker(Vector3Int myPos)
    {
        Vector3Int walkDirection = tileManager.RandomDirection();
        int walkLength = Random.Range(6 - tileManager.floorSize, 10 - tileManager.floorSize);

        for(int i = 0; i < walkLength; i++)
        {
            myPos += walkDirection;
            for(int a = -tileManager.floorSize; a <= tileManager.floorSize; a++)
            {
                for (int b = -tileManager.floorSize; b <= tileManager.floorSize; b++)
                {
                    Vector3Int offset = new Vector3Int(a, b, 0);
                    if(!tileManager.InFloorList(myPos + offset))
                    {
                        tileManager.floorList.Add(myPos + offset);
                    }
                }
            }
        }

        return myPos;
    }

    void RandomRoom(Vector3Int myPos)
    {
        int width = Random.Range(2  + tileManager.floorSize, 10 + tileManager.floorSize);
        int height = Random.Range(2  + tileManager.floorSize, 10 + tileManager.floorSize);

        for (int w = -width; w <= width; w++)
        {
            for (int h = -height; h <= height; h++)
            {
                Vector3Int offset = new Vector3Int(w, h, 0);
                if(!tileManager.InFloorList(myPos + offset))
                {
                    tileManager.floorList.Add(myPos + offset);
                }
            }
        }
    }
}
