using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralMapGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap wallMap;

    [Header("Tiles")]
    [SerializeField] private RuleTile wall;
    [SerializeField] private RuleTile floor;
    [SerializeField] private Tile shadowFloor;

    [Header("Settings")]
    [SerializeField] private int width = 200;
    [SerializeField] private int height = 200;
    [SerializeField] private int offsetX = -100;
    [SerializeField] private int offsetY = -100;
    [SerializeField] private int brushSize = 5;
    [SerializeField] private int iterations = 1000;
    [SerializeField] private int pathes = 10;

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
        floorMap.ClearAllTiles();
        for (int forx = 0; forx < width; forx++)
        {
            for (int fory = 0; fory < height; fory++)
            {
                floorMap.SetTile(new Vector3Int(forx + offsetX, fory + offsetY), shadowFloor);
                wallMap.SetTile(new Vector3Int(forx + offsetX, fory + offsetY), wall);
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
                    wallMap.SetTile(walkerPos + new Vector3Int(a, 0), null);
                    wallMap.SetTile(walkerPos + new Vector3Int(0, a), null);
                    floorMap.SetTile(walkerPos + new Vector3Int(a, 0), floor);
                    floorMap.SetTile(walkerPos + new Vector3Int(0, a), floor);
                }
            }
        } 
    }
}
