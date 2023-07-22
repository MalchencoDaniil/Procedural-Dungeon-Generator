using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    private Vector3 roomSize;

    private int playerCount = 1;

    [SerializeField] 
    private NavMeshSurface navMeshSurface;

    [SerializeField] 
    private LevelData levelData;

    private void Start()
    {
        RenderSettings.skybox = levelData.sky;

        roomSize = new Vector3(Random.Range(levelData.minRoomSizeWorldUnits.x, levelData.maxRoomSizeWorldUnits.x), Random.Range(levelData.minRoomSizeWorldUnits.y, levelData.maxRoomSizeWorldUnits.y), Random.Range(levelData.minRoomSizeWorldUnits.z, levelData.maxRoomSizeWorldUnits.z));

        Debug.Log(roomSize);

        MyInput();
    }

    private void MyInput()
    {
        levelData.roomDepth = Mathf.RoundToInt(roomSize.x / levelData.blockSize);
        levelData.roomWidth = Mathf.RoundToInt(roomSize.y / levelData.blockSize);

        SetupLevel();
        CreateFloors();
        CreateWalls();
        RemoveSingleWalls();
        SpawnLevel();

        SpawnAllObjects();

        navMeshSurface.BuildNavMesh();
    }

    private void SpawnAllObjects()
    {
        for (int x = 0; x < levelData.roomWidth; x++)
        {
            for (int z = 0; z < levelData.roomDepth; z++)
            {
                PlayerSpawn(x, z);
            }
        }
    }

    private void SetupLevel()
    {
        levelData.gridSpace = new LevelData.GridSpace[levelData.roomWidth, levelData.roomDepth];

        for (int x = 0; x < levelData.roomWidth - 1; x++)
        {
            for (int y = 0; y < levelData.roomDepth - 1; y++)
                levelData.gridSpace[x, y] = LevelData.GridSpace.Empty;
        }

        levelData.walkerList = new List<Walker>();
        Walker newWalker = new Walker();

        newWalker._direction = RandomDirection();

        Vector2 spawnPosition = new Vector2(Mathf.RoundToInt(levelData.roomWidth / 2.0f), Mathf.RoundToInt(levelData.roomDepth / 2.0f));
        newWalker._position = spawnPosition;

        levelData.walkerList.Add(newWalker);
    }

    private void CreateFloors()
    {
        do
        {
            foreach (Walker myWalker in levelData.walkerList)
                levelData.gridSpace[(int)myWalker._position.x, (int)myWalker._position.y] = LevelData.GridSpace.Floor;

            int numberChecks = levelData.walkerList.Count;

            for (int i = 0; i < numberChecks; i++)
            {
                if (Random.value < levelData.chanceWalkerDestoy && levelData.walkerList.Count > 1)
                {
                    levelData.walkerList.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < levelData.walkerList.Count; i++)
            {
                if (Random.value < levelData.chanceWalkerChangeDirection)
                {
                    Walker thisWalker = levelData.walkerList[i];
                    thisWalker._direction = RandomDirection();
                    levelData.walkerList[i] = thisWalker;
                }
            }

            numberChecks = levelData.walkerList.Count;

            for (int i = 0; i < numberChecks; i++)
            {
                if (Random.value < levelData.chanceWalkerSpawn && levelData.walkerList.Count < levelData.maxWalkers)
                {
                    Walker newWalker = new Walker();
                    newWalker._direction = RandomDirection();
                    newWalker._position = levelData.walkerList[i]._position;
                    levelData.walkerList.Add(newWalker);
                }
            }

            for (int i = 0; i < levelData.walkerList.Count; i++)
            {
                Walker thisWalker = levelData.walkerList[i];
                thisWalker._position += thisWalker._direction;
                levelData.walkerList[i] = thisWalker;
            }

            for (int i = 0; i < levelData.walkerList.Count; i++)
            {
                Walker thisWalker = levelData.walkerList[i];

                thisWalker._position.x = Mathf.Clamp(thisWalker._position.x, 1, levelData.roomWidth - 2);
                thisWalker._position.y = Mathf.Clamp(thisWalker._position.y, 1, levelData.roomDepth - 2);

                levelData.walkerList[i] = thisWalker;
            }

            if ((float)NumberOfFloors() / (float)levelData.gridSpace.Length > levelData.percentToFill)
                break;

            levelData.iterations++;
        } while (levelData.iterations < 100000);
    }

    private void CreateWalls()
    {
        for (int x = 0; x < levelData.roomWidth - 1; x++)
        {
            for (int y = 0; y < levelData.roomDepth - 1; y++)
            {
                if (levelData.gridSpace[x, y] == LevelData.GridSpace.Floor)
                {
                    if (levelData.gridSpace[x, y + 1] == LevelData.GridSpace.Empty)
                        levelData.gridSpace[x, y + 1] = LevelData.GridSpace.Wall;

                    if (levelData.gridSpace[x, y - 1] == LevelData.GridSpace.Empty)
                        levelData.gridSpace[x, y - 1] = LevelData.GridSpace.Wall;

                    if (levelData.gridSpace[x + 1, y] == LevelData.GridSpace.Empty)

                        levelData.gridSpace[x + 1, y] = LevelData.GridSpace.Wall;

                    if (levelData.gridSpace[x - 1, y] == LevelData.GridSpace.Empty)
                        levelData.gridSpace[x - 1, y] = LevelData.GridSpace.Wall;
                }
            }
        }
    }

    private void RemoveSingleWalls()
    {
        for (int x = 0; x < levelData.roomWidth - 1; x++)
        {
            for (int y = 0; y < levelData.roomDepth - 1; y++)
            {
                if (levelData.gridSpace[x, y] == LevelData.GridSpace.Wall)
                {
                    bool allFloors = true;

                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > levelData.roomWidth - 1 || y + checkY < 0 || y + checkY > levelData.roomDepth - 1)
                                continue;

                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                                continue;

                            if (levelData.gridSpace[x + checkX, y + checkY] != LevelData.GridSpace.Floor)
                                allFloors = false;
                        }
                    }
                    if (allFloors)
                        levelData.gridSpace[x, y] = LevelData.GridSpace.Floor;
                }
            }
        }
    }

    private void PlayerSpawn(int x, int z)
    {
        if (levelData.gridSpace[x, z] == LevelData.GridSpace.Floor && playerCount > 0)
        {
            playerCount--;

            Spawn(x, 0, z, levelData.player);
        }
    }

    private void SpawnLevel()
    {
        for (int x = 0; x < levelData.roomWidth; x++)
        {
            for (int y = 0; y < levelData.roomDepth; y++)
            {
                switch (levelData.gridSpace[x, y])
                {
                    case LevelData.GridSpace.Empty:
                        break;
                    case LevelData.GridSpace.Floor:
                        int randomFloor = Random.Range(0, levelData.floors.Length);
                        Spawn(x, -1, y, levelData.floors[randomFloor]);
                        break;
                    case LevelData.GridSpace.Wall:
                        int randomWall = Random.Range(0, levelData.walls.Length);
                        Spawn(x, 0, y, levelData.walls[randomWall]);
                        break;
                }
            }
        }
    }

    private Vector2 RandomDirection()
    {
        int choice = Mathf.FloorToInt(Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }

    private int NumberOfFloors()
    {
        int count = 0;

        foreach (LevelData.GridSpace space in levelData.gridSpace)
            if (space == LevelData.GridSpace.Floor)
                count++;

        return count;
    }

    private void Spawn(float x, float y, float z, GameObject toSpawn)
    {
        Vector3 offset = roomSize / 2f;
        Vector3 spawnPosition = new Vector3(x, y, z) * levelData.blockSize - offset;

        Instantiate(toSpawn, spawnPosition, Quaternion.identity);
    }
}