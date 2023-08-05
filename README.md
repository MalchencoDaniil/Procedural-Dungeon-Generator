# Procedural Dungeon Generator

The Unity Dungeon Generator in c# is easy to set up and edit. Almost every variable used in this generator is configurable and accessible from the inspector, but to create a more advanced generator, you will have to think, there will be a small guide at the bottom of the page to improve the code (programming is required).

Feel free to use it in your own projects if you want!😊

# How to create a new level
    1.Download and add the files to your assets folder
![Снимок экрана (264)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/46aa0532-8434-4252-a375-43520e7e692a)
#
    2.Add Empty Objects LevelGenerator, CursorManager and NavMeshSurface
![Снимок экрана (282)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/6c247ae0-63c2-4841-aa5f-15b7d02a78cc)
#    
    3.Add components on objects
![Снимок экрана (267)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/9d9813bd-7163-4ffb-b0cf-103e261b49a6)
![Снимок экрана (268)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/30d45c0a-7357-4e25-a691-755a86b9e5f5)
![Снимок экрана (280)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/8b75bc17-a71a-4fe6-8aab-c52e6c09ec57)
#
    4.Creating a new LevelData in the Datas folder
![Снимок экрана (273)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/94c615ea-a0f7-4ec0-944b-79a31e9d8f0f)
![Снимок экрана (276)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/acbc6f29-d860-4ad7-bdb2-5f56fb54e227)
#
    5.Configuring components
![Снимок экрана (281)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/01630c16-c921-48c2-9766-cf26b4d5e258)
![Снимок экрана (277)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/045fc330-c26b-4efc-9ee0-a4b0beb9612a)
#
    6.Press Start!
![Снимок экрана (279)](https://github.com/MalchencoDaniil/ProceduralDungeonGenerator/assets/109500163/31ebc87c-79e5-4e62-a1b9-72d7f681cea8)

# Dungeon Generator code tutorial!
    
Level Data Script

# 
    [Space(15)] 
    [Header("Chance Spawn Objects")]     
    [SerializeField, Range(0, 1)] public float ToWall = 2.0f;     
    [SerializeField, Range(0, 1)] public float ToFloor = 2.0f; 
         
    [Space(15)] 
    [Header("Prefabs of Objects")]    
    public GameObject[] floorObjects;     
    public GameObject[] wallObjects
    

Level Generator Script

#
    private void SpawnObjectOnFloor(int x, int z)     
    {         
        if (levelData.gridSpace[x, z] == LevelData.GridSpace.Floor && Random.value < levelData.ToFloor)         
        {             
            GameObject _randomObject = levelData.floorObjects[Random.Range(0, levelData.floorObjects.Length)];              
            Spawn(x, position.y?, z, _randomObject);         
        }     
    }    
    private void SpawnObjectOnWall(int x, int z)      
    {              
        if (levelData.gridSpace[x, z] == LevelData.GridSpace.Wall && Random.value < levelData.ToWall)             
        {                      
            GameObject _randomObject = levelData.wallObjects[Random.Range(0, levelData.wallObjects.Length)];                       
            Spawn(x, position.y?, z, _randomObject);              
        }     
    }
    for (int x = 0; x < levelData.roomWidth; x++)           
    {                       
        for (int z = 0; z < levelData.roomDepth; z++)            
        {                                   
            PlayerSpawn(x, z);                                   
            SpawnObjectOnFloor(x, z);                  
            SpawnObjectOnWall(x, z);                           
        }              
    }
