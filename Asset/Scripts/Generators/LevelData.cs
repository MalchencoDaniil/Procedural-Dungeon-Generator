using UnityEngine;
using System.Collections.Generic;

public class Walker
{
	public Vector3 _position;
	public Vector3 _direction;
}

[CreateAssetMenu(fileName = "New Level", menuName = "Generators/LevelData")]
 
public class LevelData : ScriptableObject
{
	public enum GridSpace
	{
		Empty,
		Floor,
		Wall
	};

	internal GridSpace[,] gridSpace;
	internal List<Walker> walkerList;

	internal float chanceWalkerChangeDirection = 0.5f; 
	internal float chanceWalkerSpawn = 0.05f, chanceWalkerDestoy = 0.05f;

	internal int maxWalkers = 10, iterations = 0;
	internal int roomDepth, roomWidth;

	internal float blockSize = 1.0f;

	[Header("Room Size")]
	[Range(0, 1)] public float percentToFill = 0.2f;
	public Vector3 minRoomSizeWorldUnits = new Vector3(30, 30, 30);
	public Vector3 maxRoomSizeWorldUnits = new Vector3(30, 30, 30);

	[Header("Walls and Floors")]
	public GameObject[] walls;
	public GameObject[] floors;

	[Space(15)]	[Header("Environment")]
	public Material sky;
	public GameObject player;
}