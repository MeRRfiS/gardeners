using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class ProceduralMapGenerator : MonoBehaviour
{
    public Tilemap map;
    public int width = 100;
    public int height = 100;
    public int offsetX = -50;
    public int offsetY = -50;
    public int brushSize = 5;
    public int iterations = 300;
    public int pathes = 3;
    public Tile wall;
    public Tile floor;

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateMap();
        }
    }

    public void GenerateMap()
    {
        map.ClearAllTiles();
        for (int forx = 0; forx < width; forx++)
        {
            for (int fory = 0; fory < height; fory++)
            {
                map.SetTile(new Vector3Int(forx+offsetX, fory+offsetY), wall);
            }
        }
        for (int path = 0; path < pathes; path++)
        {
            Vector3Int walkerPos = new Vector3Int(0, 0, 0);
            for (int i = 0; i < iterations; i++)
            {
                int x = Random.Range(-1, 2);
                int y = Random.Range(-1, 2);
                if (x == y)
                {
                    y = 0;
                }
                walkerPos += new Vector3Int(x, y);
                for (int a = 0; a < brushSize; a++)
                {
                    map.SetTile(walkerPos + new Vector3Int(a, 0), floor);
                    map.SetTile(walkerPos + new Vector3Int(0, a), floor);
                }
            }
        } 
    }
}
