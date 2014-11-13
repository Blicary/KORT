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
    public static string victory_scene = "victory_scene";
    private static string main_menu_scene = "main_menu";

    private static int current_arena = 0;
    public static SceneState Scenestate { get; private set; }
    public SceneState initial_scene_state = SceneState.Arena;

    // camera
    public static ScreenFadeInOut screen_fade;
    public Camera cam_in_game_menu, cam_main;

    // menu
    private static bool loading_game = false;
    public MenuPage dead_screen, start_screen, gg_page, victory_page; // in-game menus


    public void Awake()
    {
        // if this is the first instance, make this the singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);

            Scenestate = initial_scene_state;
        }
        else
        {
            // get new instance variables
            _instance.cam_main = cam_main;
            _instance.cam_in_game_menu = cam_in_game_menu;
            _instance.dead_screen = dead_screen;
            _instance.start_screen = start_screen;
            _instance.gg_page = gg_page;
            _instance.victory_page = victory_page;


            if (cam_main == null) Debug.LogError("Missing main camera in new scene.");
            if (Scenestate != SceneState.Menu && Scenestate != SceneState.VictoryRoom && cam_in_game_menu == null)
                Debug.LogError("Missing in game menu camera in new scene.");


            // destroy other instances that are not the already existing singleton
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }
    public void Start()
    {
        screen_fade = GetComponent<ScreenFadeInOut>();
    }

    public void OnLevelWasLoaded(int level)
    {
        if (this != _instance) return;

        // start page
        if (loading_game)
        {
            loading_game = false;

            HouseManager.Initialize();

            Debug.Log(_instance.cam_in_game_menu);  

            _instance.cam_main.gameObject.SetActive(false);
            _instance.cam_in_game_menu.gameObject.SetActive(true);

            Time.timeScale = 0;
            _instance.start_screen.TransitionIn(null);

            screen_fade.InstantBlack();
            screen_fade.FadeToClear();
        }
        else if (Scenestate == SceneState.Arena)
        {
            _instance.cam_in_game_menu.gameObject.SetActive(false);
            _instance.cam_main.gameObject.SetActive(true);
            start_screen.SetOut();
        }
        else if (Scenestate == SceneState.Menu)
        {
            screen_fade.InstantBlack();
            screen_fade.FadeToClear();
        }
    }

    public void Update()
    {
        if (Scenestate == SceneState.VictoryRoom)
        {
            if (Input.GetButtonDown("Submit"))
            {
                Time.timeScale = 0;
                victory_page.TransitionIn(null);
            }
        }
    }
    
    public static void LoadGame()
    {
        if (arena_sequence.Length == 0) Debug.LogError("Cannot start game, arena sequence not specified.");

        loading_game = true;
        current_arena = 0;
        Scenestate = SceneState.Arena;
        Application.LoadLevel(arena_sequence[current_arena]);
    }
    public static void BeginGame()
    {
        Time.timeScale = 1;
        screen_fade.InstantBlack();
        screen_fade.FadeToClear();

        _instance.cam_in_game_menu.gameObject.SetActive(false);
        _instance.cam_main.gameObject.SetActive(true);
    }
    public static void DeadScreen()
    {
        Scenestate = SceneState.DeadScreen;
        screen_fade.InstantClear();
        _instance.cam_main.gameObject.SetActive(false);
        _instance.cam_in_game_menu.gameObject.SetActive(true);

        _instance.dead_screen.SetIn();
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
    public static void GameOverScreen()
    {
        Scenestate = SceneState.GameOverScreen;
        screen_fade.InstantClear();
        _instance.cam_main.gameObject.SetActive(false);
        _instance.cam_in_game_menu.gameObject.SetActive(true);

        _instance.gg_page.SetIn();
    }
    public static void LoadMainMenu()
    {
        Scenestate = SceneState.Menu;
        Application.LoadLevel(main_menu_scene);
    }
    public static void NextCombatant()
    {
        Scenestate = SceneState.Arena;
        _instance.cam_in_game_menu.gameObject.SetActive(false);
        _instance.cam_main.gameObject.SetActive(true);
        screen_fade.InstantBlack();
        screen_fade.FadeToClear();

        HouseManager.CreatePlayerCombatantObject();
    }

    // PUBLIC ACCESSORS

    public static Camera GetCamMain()
    {
        return _instance.cam_main;
    }
    public static Camera GetCamInGameMenu()
    {
        return _instance.cam_in_game_menu;
    }
}
