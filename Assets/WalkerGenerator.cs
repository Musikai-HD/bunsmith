using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public Tile wall;
    public Tile floor;
    public int mapWidth = 50, mapHeight = 50;

    public int minWalkers = 10;
    public int maxWalkers = 30;
    public int tileCount = default;
    public float fillPercentage = 0.4f;
    public float waitTime = 0.05f;

    public GameObject chestPrefab;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        gridHandler = new Grid[mapWidth, mapHeight];
        floorMap.transform.parent.position = new Vector3(-mapWidth/2 - 0.5f, -mapHeight/2 - 0.5f);
        for (int i = 0; i < gridHandler.GetLength(0); i++)
        {
            for (int o = 0; o < gridHandler.GetLength(1); o++)
            {
                gridHandler[i,o] = Grid.EMPTY;
            }
        }

        walkers = new List<WalkerObject>();

        Vector3Int tileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        WalkerObject curWalker = new WalkerObject(new Vector2(tileCenter.x, tileCenter.y), GetDirection(), 0.5f);
        //gridHandler[tileCenter.x, tileCenter.y] = Grid.FLOOR;
        //floorMap.SetTile(tileCenter, floor);
        for (int i = -1; i < 2; i++)

        {
            for (int o = -1; o < 2; o++)
            {
                gridHandler[tileCenter.x + i, tileCenter.y + o] = Grid.FLOOR;
                floorMap.SetTile(new Vector3Int(tileCenter.x + i, tileCenter.y + o, 0), floor);
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

        StartCoroutine(CreateWalls());
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

    IEnumerator CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
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
        FinishGeneration();
    }

    public void FinishGeneration()
    {
        AstarPath.active?.Scan();
        
        if (chestPrefab != null)
        {
            Instantiate(chestPrefab, GetRandomTilePos(floorMap), Quaternion.identity);
        }
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
}
