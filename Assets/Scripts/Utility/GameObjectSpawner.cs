using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameObjectSpawner : MonoBehaviour
{
	
	// Collectables to spawn
	public List<GameObject> gameObjects = new List<GameObject>();
	
	// How often to spawn a game object
	public bool spawnOnFrequency = true;
	public float spawnFrequency = 1.0f;
	
	// Spawn certain number on start
	public bool spawnOnStart = true;
	
	// How many to spawn on start
	public int spawnOnStartQuantity = 200;
	
	private float lastSpawnTime = 0.0f;
	
	private Mesh mesh;
	Vector3 minSpawnableArea;
	Vector3 maxSpawnableArea;
	
	void Awake()
	{
		// Get bounds of navmesh
		mesh = new Mesh();
		NavMeshTriangulation navmeshData = NavMesh.CalculateTriangulation();
		mesh.SetVertices(navmeshData.vertices);
		mesh.SetIndices(navmeshData.indices, MeshTopology.Triangles, 0);
		minSpawnableArea = mesh.bounds.min;
		maxSpawnableArea = mesh.bounds.max;
		
		lastSpawnTime = Time.time;
	}
	
    void Start()
	{
	    if (spawnOnStart) 
	    {
	    	for (int i = 0; i < spawnOnStartQuantity; i++) 
	    	{
	    		SpawnGameObject();
	    	}
	    }
    }

    void Update()
    {
	    // Every x amount of time, spawn collectables
	    if (spawnOnFrequency && lastSpawnTime + spawnFrequency <= Time.time) 
	    {
	    	lastSpawnTime = Time.time;
	    	SpawnGameObject();
	    }
    }
    
	private void SpawnGameObject()
	{
		Vector3 point;
		
		// Get valid point on navmesh
		RandomPoint(out point);
		
		// Spawn collectable at point
		GameObject collectable = Instantiate(
			gameObjects[Random.Range(0, gameObjects.Count)],
			point,
			Quaternion.identity
		);
	}
    
	// Sample random point on navmesh
	private bool RandomPoint(out Vector3 result, Vector3 origin = default(Vector3), float range = 200f)
	{
		while (true) 
		{
			// Generate random point
			Vector3 randomPoint = new Vector3 ((Random.Range(minSpawnableArea.x, maxSpawnableArea.x)), 0.5f, (Random.Range(minSpawnableArea.z, maxSpawnableArea.z)));
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas)) {
				
				result = hit.position;
				return true;
			}
		}
	}
}
