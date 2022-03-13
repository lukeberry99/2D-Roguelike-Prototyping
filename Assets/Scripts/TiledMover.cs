using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TiledMover : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;
    bool isMoving;

    void Update()
    {
        float h = System.Math.Sign(Input.GetAxisRaw("Horizontal"));
        float v = System.Math.Sign(Input.GetAxisRaw("Vertical"));

        if(!isMoving)
        {
            if(Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
            {
                if (Mathf.Abs(h) > 0)
                {
                    targetPos = new Vector3(transform.position.x + h , transform.position.y, transform.position.z);
                }
                else if(Mathf.Abs(v) > 0)
                {
                    targetPos = new Vector3(transform.position.x, transform.position.y + v, transform.position.z);
                }

                var tiles = TileManager.instance.tiles;
                GameTile tile;

                if(tiles.TryGetValue(targetPos, out tile))
                {
                    if(tile.tileType != GameTile.TileType.Wall)
                    {
                        StartCoroutine(Move());
                    }
                }
            }
        }
    }

    IEnumerator Move()
    {
        isMoving = true;
        while(Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            Debug.DrawLine(transform.position, targetPos, Color.red);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
}
