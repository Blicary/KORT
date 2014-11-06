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
    private static string[] arena_sequence = { "test_scene2", "test_scene2" };
    private static string transition_scene = "transition_scene";
    private static string game_over_scene = "test_scene2";


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
        if (Input.GetKeyDown(KeyCode.N))
            ClearArena();
    }

    public static void ClearArena()
    {
        Application.LoadLevel(transition_scene);
    }
    public static void GameOver()
    {
        Application.LoadLevel(game_over_scene);
    }
}
