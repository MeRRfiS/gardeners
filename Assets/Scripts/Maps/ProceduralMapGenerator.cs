using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralMapGenerator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap wallMap;
    [SerializeField] private GameObject spawner;

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

    public static List<GameObject> spawners = new List<GameObject>();

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (spawners.Count == 0)
        {
            TextController.GetInstance().OpenEndGamePanel(2);
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

        while (spawners.Count != 3)
        {
            var randX = Random.Range(0, width);
            var randY = Random.Range(0, height);
            //var floorTile = floorMap.GetTile(new Vector3Int(randX + offsetX, randY + offsetY));
            var wallTiles = new List<TileBase>();
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    wallTiles.Add(wallMap.GetTile(new Vector3Int(randX + offsetX + i, randY + offsetY + j)));
                }
            }

            bool notEmptyPlace = false;
            foreach (var wallTile in wallTiles)
            {
                if(wallTile != null)
                {
                    notEmptyPlace = true;
                    break;
                }
            }
            if (notEmptyPlace) continue;

            bool isNotFar = false;
            foreach (var spawn in spawners)
            {
                if(Vector2.Distance(spawn.transform.position, new Vector2Int(randX + offsetX, randY + offsetY)) <= 30)
                {
                    isNotFar = true;
                    break;
                }
            }
            if (isNotFar) continue;

            var spawnerObj = Instantiate(spawner);
            spawnerObj.transform.position = new Vector3Int(randX + offsetX, randY + offsetY);
            switch (spawners.Count)
            {
                case (0):
                    spawnerObj.GetComponent<EnemySpawner>().Type = EnemyTypesEnum.Spider;
                    break;
                case (1):
                    spawnerObj.GetComponent<EnemySpawner>().Type = EnemyTypesEnum.Apple;
                    break;
                case (2):
                    spawnerObj.GetComponent<EnemySpawner>().Type = EnemyTypesEnum.Ent;
                    break;
            }
            spawners.Add(spawnerObj);
        }

        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
