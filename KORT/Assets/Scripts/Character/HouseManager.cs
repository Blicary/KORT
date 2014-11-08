using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HouseManager : MonoBehaviour 
{
    private static HouseManager _instance;
    public static HouseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<HouseManager>();

                if (_instance == null) Debug.LogError("Missing GameManger");
                else DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    //[System.NonSerialized]
    public HouseName[] houses_in_play;
    public Combatant combatant_prefab;

    private static Dictionary<HouseName, House> houses;
    private static Dictionary<HouseName, ExitDoor> doors;


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
        houses = new Dictionary<HouseName, House>();
        doors = new Dictionary<HouseName, ExitDoor>();
        for (int i = 0; i < houses_in_play.Length; ++i)
        {
            houses[houses_in_play[i]] = new House(houses_in_play[i], 20);
        }
        SetupInNewArena();
    }

    public void OnLevelWasLoaded(int level)
    {
        if (this != _instance) return;
        SetupInNewArena();
    }

    public static void RecordKill(HouseName house_name)
    {
        houses[house_name].RecordKill();
        //Debug.Log("House " + house_name + " has " + houses[house_name].KillsCurrentArena + " kills in this arena.");

        if (houses[house_name].KillsCurrentArena >= 1 && !doors[house_name].IsOpen())
        {
            // open the door for this house
            doors[house_name].Open();
            Debug.Log("The door for house " + house_name + " is now open.");
        }
    }
    public static void RecordDeath(HouseName house_name, Combatant combatant)
    {
        if (houses[house_name].CombatantsLeft() > 0)
        {
            houses[house_name].NextCombatant();

            Debug.Log("House " + house_name + " has " + houses[house_name].CombatantsLeft() + " remaining combatants.");
         
            GameManager.DeadScreen();
        }
        else
        {
            // death of house
            Debug.Log("House " + house_name + " has perished.");
            if (combatant.player_controlled)
            {
                GameManager.GameOverScreen();
            }
        }
    }

    public static void NextCombatant()
    {
        Instantiate(_instance.combatant_prefab);
        PlayerCam cam = Camera.main.GetComponent<PlayerCam>();
        cam.FindPlayer();
    }

    private static void SetupInNewArena()
    {
        if (GameManager.Scenestate == SceneState.Arena)
        {
            FindDoors();
            ResetCurrentArenaKills();
        }
    }
    private static void FindDoors()
    {
        ExitDoor[] doors_array = FindObjectsOfType<ExitDoor>();

        if (doors_array.Length < _instance.houses_in_play.Length)
            Debug.LogError("Not enough exit doors in scene.");

        for (int i = 0; i < _instance.houses_in_play.Length; ++i)
        {
            doors[_instance.houses_in_play[i]] = doors_array[i];
        }
    }
    private static void ResetCurrentArenaKills()
    {
        foreach (HouseName house_name in _instance.houses_in_play)
        {
            houses[house_name].ResetArenaKills();
            //Debug.Log("House " + house_name + " has " + houses[house_name].KillsCurrentArena + " kills in this arena.");
        }
    }
}
