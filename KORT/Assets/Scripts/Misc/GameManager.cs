using UnityEngine;
using System.Collections;

public enum SceneType { Arena, InterArenaCorridor, VictoryRoom }

public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();

                if (_instance == null) Debug.LogError("Missing GameManger");
                else DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    // scene management
    private static string[] arena_sequence = { "test_scene", "test_scene" };
    private static string transition_scene = "transition_scene";
    private static string dead_scene = "game_over_scene";
    private static string game_over_scene = "game_over_scene";
    private static string victory_scene = "victory_scene";

    private static int current_arena = 0;
    public static SceneType Scene { get; private set; }



    public void Awake()
    {
        // if this is the first instance, make this the singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            // destroy other instances that are not the already existing singleton
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }
    public void Start()
    {
        Scene = SceneType.Arena;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            NextCombatant();
    }

    public static void ClearArena()
    {
        if (Scene == SceneType.Arena)
        {
            current_arena += 1;
            if (current_arena < arena_sequence.Length)
            {
                Scene = SceneType.InterArenaCorridor;
                Application.LoadLevel(transition_scene);
            }
            else
            {
                Application.LoadLevel(victory_scene);
            }
        }
        else if (Scene == SceneType.InterArenaCorridor)
        {
            Scene = SceneType.Arena;
            Application.LoadLevel(arena_sequence[current_arena]);
        }
    }
    public static void DeadScreen()
    {
        Debug.Log("dead screen");
        //Application.LoadLevel(dead_scene);
    }
    public static void GameOverScreen()
    {
        Application.LoadLevel(game_over_scene);
    }
    public static void NextCombatant()
    {
        HouseManager.NextCombatant();

        //Application.LoadLevel(arena_sequence[current_arena]);
    }

    // PUBLIC ACCESSORS


}
