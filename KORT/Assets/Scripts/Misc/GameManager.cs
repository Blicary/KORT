using UnityEngine;
using System.Collections;

public enum SceneState { Menu, Arena, DeadScreen, GameOverScreen, InterArenaCorridor, VictoryRoom }

[RequireComponent(typeof(ScreenFadeInOut))]

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
    public static string[] arena_sequence = { "test_scene", "test_scene" };
    public static string inter_arena_scene = "transition_scene";
    private static string game_over_scene = "game_over_scene";
    public static string victory_scene = "victory_scene";

    private static int current_arena = 0;
    public static SceneState Scenestate { get; private set; }
    public SceneState initial_scene_state = SceneState.Arena;

    public static ScreenFadeInOut screen_fade;


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
        screen_fade = GetComponent<ScreenFadeInOut>();
        Scenestate = initial_scene_state;
    }

    public void Update()
    {
        if (Scenestate == SceneState.DeadScreen && Input.GetKeyDown(KeyCode.Space))
        {
            NextCombatant();
            Scenestate = SceneState.Arena;
        }
            
    }

    public static void ClearArena()
    {
        if (Scenestate == SceneState.Arena)
        {
            current_arena += 1;
            if (current_arena < arena_sequence.Length)
            {
                Scenestate = SceneState.InterArenaCorridor;
                Application.LoadLevel(inter_arena_scene);
            }
            else
            {
                Scenestate = SceneState.VictoryRoom;
                Application.LoadLevel(victory_scene);
            }
        }
        else if (Scenestate == SceneState.InterArenaCorridor)
        {
            Scenestate = SceneState.Arena;
            Application.LoadLevel(arena_sequence[current_arena]);
        }
    }
    public static void StartGame()
    {
        if (arena_sequence.Length == 0) Debug.LogError("Cannot start game, arena sequence not specified.");

        Scenestate = SceneState.Arena;
        Application.LoadLevel(arena_sequence[current_arena]);
        screen_fade.InstantBlack();
        screen_fade.FadeToClear();
    }
    public static void DeadScreen()
    {
        screen_fade.InstantBlack();
        Scenestate = SceneState.DeadScreen;
    }
    public static void GameOverScreen()
    {
        Scenestate = SceneState.GameOverScreen;
        Application.LoadLevel(game_over_scene);
    }
    public static void NextCombatant()
    {
        HouseManager.CreateNewPlayerCombatant();
        screen_fade.FadeToClear();
        //Application.LoadLevel(arena_sequence[current_arena]);
    }

    // PUBLIC ACCESSORS


}
