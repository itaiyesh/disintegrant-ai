﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Events;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //TODO: Make singleton
    public Texture2D crosshair;
    GameState state;
    public bool isTutorial = false;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject gameWonMenu;

    public List<GameObject> botPrefabs;
    public int numBots = 10;
    public Dictionary<int, GameObject> bots = new Dictionary<int, GameObject>();

    public List<Vector3> PlayerSpawnPositions = new List<Vector3>();
    private UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>> characterAttributeEventListener;

    void Awake()
    {
        //If no specified spawn positions, generate random positions on map;
        if (PlayerSpawnPositions.Count == 0)
        {
            Mesh mesh = new Mesh();
            NavMeshTriangulation navmeshData = NavMesh.CalculateTriangulation();
            mesh.SetVertices(navmeshData.vertices);
            mesh.SetIndices(navmeshData.indices, MeshTopology.Triangles, 0);
            Vector3 minSpawnableArea = mesh.bounds.min;
            Vector3 maxSpawnableArea = mesh.bounds.max;

            for (int i = 0; i < numBots; i++)
            {
                while (true)
                {
                    // Generate random point
                    Vector3 randomPoint = new Vector3((Random.Range(minSpawnableArea.x, maxSpawnableArea.x)), 0.5f, (Random.Range(minSpawnableArea.z, maxSpawnableArea.z)));
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
                    {
                        PlayerSpawnPositions.Add(hit.position);
                        break;
                    }
                }
            }
        }

        //Spawn players
        for (int i = 0; i < numBots; i++)
        {
            //Better have more spawn points than players
            GameObject bot = Instantiate(botPrefabs[i % botPrefabs.Count], PlayerSpawnPositions[i % PlayerSpawnPositions.Count], Quaternion.identity);
            bots[bot.GetInstanceID()] = bot;
        }
        characterAttributeEventListener = new UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventHandler);
        //set the cursor origin to its centre. (default is upper left corner)
        Vector2 cursorOffset = new Vector2(crosshair.width / 2, crosshair.height / 2);
        //Sets the cursor to the Crosshair sprite with given offset 
        //and automatic switching to hardware default if necessary
        Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
    }

    // Start is called before the first frame update
    void Start()
    {
        Switch(GameState.PLAYING);
    }
    void OnEnable()
    {
        EventManager.StartListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);
    }
    void characterAttributeEventHandler(
    GameObject triggeringPlayer,
    Dictionary<string, object> oldAttributes,
    Dictionary<string, object> newAttributes
)
    {
        if (newAttributes.ContainsKey("Health") && (float)newAttributes["Health"] <= 0.005f)
        {
            Die(triggeringPlayer);
            int playerID = triggeringPlayer.GetInstanceID();
            if (bots.ContainsKey(playerID))
            {
                bots.Remove(playerID);
                int totalPlayers = bots.Count + 1;
                EventManager.TriggerEvent<CountDownEvent, int>(totalPlayers);
            }
            if (triggeringPlayer.name == "Player") { Switch(GameState.GAMEOVER); }
        }

    }

    void Die(GameObject player)
    {
        player.SetActive(false);
        player.GetComponent<CharacterAttributes>().characterAttributes.IsAlive = false;
        GameObject ragdollPrefab = player.GetComponent<Ragdoll>().ragdollPrefab;
        GameObject ragdoll = Instantiate(ragdollPrefab, player.transform.position, player.transform.rotation);
        Destroy(ragdoll, 3);
        AudioClip[] deathClips = player.GetComponent<CharacterAttributes>().characterAttributes.VoicePack.death;
        if (deathClips.Length > 0)
        {
            int index = Random.Range(0, deathClips.Length - 1);
            EventManager.TriggerEvent<VoiceEvent, AudioClip, Vector3>(deathClips[index], player.transform.position);
        }

    }


    public void Switch(GameState newState)
    {
        Debug.Log(state + " -> " + newState);

        switch (newState)
        {
            case GameState.PLAYING:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.PAUSE:
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.UNPAUSE:
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                Switch(GameState.PLAYING);
                return; // need to return here or state is set to unpause incorrectly
            case GameState.GAMEOVER:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0.3f;
                gameOverMenu.SetActive(true);
                break;
            case GameState.WIN:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                gameWonMenu.SetActive(true);
                break;
            case GameState.QUIT:
                QuitGame();
                break;
        };
        state = newState;
    }
    void Update()
    {
        //PAUSE - UNPAUSE
        if ((state == GameState.PLAYING || state == GameState.PAUSE) && Input.GetKeyUp(KeyCode.Escape))
        {
            Switch(state == GameState.PAUSE ? GameState.UNPAUSE : GameState.PAUSE);
        }

        if (state != GameState.WIN && bots.Count == 0 && !isTutorial)
        {
            Switch(GameState.WIN);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            bots.Clear();
        }
    }
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}

public enum GameState
{
    // START,
    PAUSE,
    UNPAUSE,
    GAMEOVER,
    WIN,
    PLAYING,
    QUIT
}
