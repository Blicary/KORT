using UnityEngine;
using System.Collections;

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
    private static string game_over_scene = "game_over_scene";
    private static string victory_scene = "victory_scene";

    private static int current_arena = 0;
    private static bool in_transition_scene = false;



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


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }

    public static void ClearArena()
    {
        if (!in_transition_scene)
        {
            current_arena += 1;
            if (current_arena < arena_sequence.Length)
            {
                in_transition_scene = true;
                Application.LoadLevel(transition_scene);
            }
            else
            {
                Application.LoadLevel(victory_scene);
            }
        }
        else
        {
            in_transition_scene = false;
            Application.LoadLevel(arena_sequence[current_arena]);
        }
    }
    public static void GameOver()
    {
        Application.LoadLevel(game_over_scene);
    }
    public static void RestartGame()
    {
        current_arena = 0;
        in_transition_scene = false;
        Application.LoadLevel(arena_sequence[0]);
    }
}
