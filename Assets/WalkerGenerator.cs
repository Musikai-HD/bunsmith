using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WalkerGenerator : MonoBehaviour
{
    public enum Grid
    {
        FLOOR,
        WALL,
        EMPTY
    }

    public Grid[,] gridHandler;
    public List<WalkerObject> walkers;
    public Tilemap floorMap, wallMap;
    public RuleTile wall;
    public Tile floor;
    public Tile emptyWall;
    public int mapWidth = 50, mapHeight = 50;

    public int minWalkers = 10;
    public int maxWalkers = 30;
    public int tileCount = default;
    public float fillPercentage = 0.4f;
    public float waitTime = 0.05f;

    public GameObject chestPrefab, exitPrefab;
    public EnemyPack enemyPack;
    public int roomPower, roomMaxPower;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        gridHandler = new Grid[mapWidth, mapHeight];
        floorMap.transform.parent.position = new Vector3(-mapWidth/2 - 0.5f, -mapHeight/2 - 0.5f);
        
        //set out layer of empty walls
        for (int i = -1; i < gridHandler.GetLength(0) + 1; i++)
        {
            wallMap.SetTile(new Vector3Int(i, -1, 0), emptyWall);
            wallMap.SetTile(new Vector3Int(i, gridHandler.GetLength(1), 0), emptyWall);
        }
        for (int o = -1; o < gridHandler.GetLength(1) + 1; o++)
        {
            wallMap.SetTile(new Vector3Int(-1, o, 0), emptyWall);
            wallMap.SetTile(new Vector3Int(gridHandler.GetLength(0), o, 0), emptyWall);
        }
        
        for (int i = 0; i < gridHandler.GetLength(0); i++)
        {
            for (int o = 0; o < gridHandler.GetLength(1); o++)
            {
                gridHandler[i,o] = Grid.WALL;
                wallMap.SetTile(new Vector3Int(i, o, 0), wall);
            }
        }

        walkers = new List<WalkerObject>();

        Vector3Int tileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        WalkerObject curWalker = new WalkerObject(new Vector2(tileCenter.x, tileCenter.y), GetDirection(), 0.5f);
        //gridHandler[tileCenter.x, tileCenter.y] = Grid.FLOOR;
        //floorMap.SetTile(tileCenter, floor);
        //CreateWalls();
        for (int i = -1; i < 2; i++)

        {
            for (int o = -1; o < 2; o++)
            {
                Vector3Int pos = new Vector3Int(tileCenter.x + i, tileCenter.y + o, 0);
                gridHandler[tileCenter.x + i, tileCenter.y + o] = Grid.FLOOR;
                floorMap.SetTile(pos, floor);
                wallMap.SetTile(pos, null);
            }
        }
        walkers.Add(curWalker);

        tileCount++; 

        StartCoroutine(CreateFloors());
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    IEnumerator CreateFloors()
    {
        while ((float)tileCount / (float)gridHandler.Length < fillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.position.x, (int)curWalker.position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    floorMap.SetTile(curPos, floor);
                    tileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    wallMap.SetTile(curPos, null);
                    hasCreatedFloor = true;
                }
            }

            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(waitTime);
            }
        }

        FinishGeneration();
    }

    void ChanceToRemove()
    {
        int updatedCount = walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < walkers[i].changeChance && walkers.Count > 1)
            {
                walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            if (UnityEngine.Random.value < walkers[i].changeChance)
            {
                WalkerObject curWalker = walkers[i];
                curWalker.direction = GetDirection();
                walkers[i] = curWalker;
            }
        }
    }

    void ChanceToCreate()
    {
        int updatedCount = walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < walkers[i].changeChance && walkers.Count < maxWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = walkers[i].position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                walkers.Add(newWalker);
            }
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            WalkerObject foundWalker = walkers[i];
            foundWalker.position += foundWalker.direction;
            foundWalker.position.x = Mathf.Clamp(foundWalker.position.x, 1, gridHandler.GetLength(0) - 2);
            foundWalker.position.y = Mathf.Clamp(foundWalker.position.y, 1, gridHandler.GetLength(1) - 2);
            walkers[i] = foundWalker;
        }
    }

    void CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                //gridHandler[x, y] = Grid.WALL;
                wallMap.SetTile(new Vector3Int(x, y, 0), wall);
            }
        }   
        /*int[] gridLength = new int[2];
        gridLength[0] = gridHandler.GetLength(0);
        gridLength[1] = gridHandler.GetLength(1);
        for (int x = 1; x < gridLength[0] - 1; x++)
        {
            for (int y = 1; y < gridLength[1] - 1; y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool hasCreatedWall = false;

                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x + 1, y, 0), wall);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x - 1, y, 0), wall);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y + 1] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x, y + 1, 0), wall);
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x, y - 1, 0), wall);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                }
            }
        }
        for (int x = 1; x < gridLength[0] - 1; x++)
        {
            for (int y = 1; y < gridLength[1] - 1; y++)
            {
                if (gridHandler[x, y] == Grid.WALL)
                {
                    bool hasCreatedWall = false;

                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x + 1, y, 0), wall);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x - 1, y, 0), wall);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y + 1] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x, y + 1, 0), wall);
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        wallMap.SetTile(new Vector3Int(x, y - 1, 0), wall);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                }
            }
        }*/
    }

    public void FinishGeneration()
    {
        FixTiles(wallMap);
        AstarPath.active?.Scan();
        
        if (chestPrefab != null)
        {
            Instantiate(chestPrefab, GetRandomTilePos(floorMap), Quaternion.identity);
        }

        if (exitPrefab != null)
        {
            Instantiate(exitPrefab, GetRandomTilePos(floorMap), Quaternion.identity);
        }

        if (enemyPack != null) PopulateWithEnemies(enemyPack);
    }
    
    Vector3 GetRandomTilePos(Tilemap map)
    {
        TileBase[] floorTiles = floorMap.GetTilesBlock(floorMap.cellBounds);

        List<Vector3> availablePlaces = new List<Vector3>();

        for (int n = map.cellBounds.xMin; n < map.cellBounds.xMax; n++)
        {
            for (int p = map.cellBounds.yMin; p < map.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = new Vector3Int(n, p, 0);
                Vector3 place = map.CellToWorld(localPlace);
                if (map.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                }
            }
        }
        int randomChoice = UnityEngine.Random.Range(0, availablePlaces.Count - 1);
        return availablePlaces[randomChoice] + new Vector3(0.5f, 0.5f, 0f);
    }

    void PopulateWithEnemies(EnemyPack ep)
    {
        ep.enemyPack.Sort();
        int leastPower = ep.enemyPack[0].enemy.enemyPower;
        while (roomPower <= roomMaxPower - leastPower)
        {
            List<PickableEnemy> possibleEnemies = new List<PickableEnemy>();
            foreach (PickableEnemy ec in ep.enemyPack)
            {
                if (ec.enemy.enemyPower <= (roomMaxPower - roomPower))
                {
                    possibleEnemies.Add(ec);
                }
            }
            EnemyContainer randomEnemy = PickRandomEnemy(possibleEnemies);
            GameObject enemyPrefabToPlace = randomEnemy.enemyPrefab;
            Instantiate(enemyPrefabToPlace, GetRandomTilePos(floorMap), Quaternion.identity);
            roomPower += randomEnemy.enemyPower;
        }
    }

    EnemyContainer PickRandomEnemy(List<PickableEnemy> ep)
    {
        int totalChance = ep.Sum(enemy => enemy.chance);
        int randomValue = Random.Range(0, totalChance);
        int chanceThreshold = 0;
        foreach (PickableEnemy enemy in ep)
        {
            chanceThreshold += enemy.chance;
            if (randomValue <= chanceThreshold)
            {
                return enemy.enemy;
            }
        }
        return null;
    }

    void FixTiles(Tilemap map)
    {
        BoundsInt bounds = map.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    TileBase tile = map.GetTile(pos);

                    if (tile != null)
                    {
                        map.SetTile(pos, null);
                        map.SetTile(pos, tile);
                    }
                }
            }
        }
        map.RefreshAllTiles();
    }
}