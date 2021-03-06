﻿using UnityEngine;
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

                if (_instance == null) Debug.LogError("Missing HouseManager");
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
    private static Dictionary<HouseName, Transform> entrance_points;

    private static int num_combatants_per_house = 4;
    


    // PUBLIC MODIFIERS

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
        if (this == _instance)
            Initialize();
    }

    public static void Initialize()
    {
        houses = new Dictionary<HouseName, House>();
        doors = new Dictionary<HouseName, ExitDoor>();
        entrance_points = new Dictionary<HouseName, Transform>();
        for (int i = 0; i < _instance.houses_in_play.Length; ++i)
        {
            houses[_instance.houses_in_play[i]] = new House(_instance.houses_in_play[i], num_combatants_per_house);
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

        if (houses[house_name].KillsCurrentArena >= ArenaDetails.GetRequiredKills() && !doors[house_name].IsOpen())
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

            //Debug.Log("House " + house_name + " has " + houses[house_name].CombatantsLeft() + " remaining combatants.");
         
            GameManager.DeadScreen();
        }
        else
        {
            // death of house
            //Debug.Log("House " + house_name + " has perished.");
            if (combatant.player_controlled)
            {
                GameManager.GameOverScreen();
            }
        }
    }

    public static void CreatePlayerCombatantObject()
    {
        // create new player
        Debug.Log(entrance_points[_instance.combatant_prefab.house_name]);
        Instantiate(_instance.combatant_prefab, entrance_points[_instance.combatant_prefab.house_name].position,
            entrance_points[_instance.combatant_prefab.house_name].rotation);

        // connect new player to the main cam
        PlayerCam cam = GameManager.GetCamMain().GetComponent<PlayerCam>();
        if (!cam) Debug.LogError("Missing PlayerCam on main camera");
        cam.FindPlayer();
    }


    // PRIVATE MODIFIERS

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
        if (entrance_points == null || doors == null) return; // BUG FIX


        // get exit doors
        ExitDoor[] doors_array = FindObjectsOfType<ExitDoor>();
        if (doors_array.Length < _instance.houses_in_play.Length)
            Debug.LogError("Not enough exit doors in scene.");

        // get entrance doors
        Portcullis[] port_array = FindObjectsOfType<Portcullis>();

        // assign a door randomly to each house
        doors_array = GeneralHelpers.ShuffleArray<ExitDoor>(doors_array);

        List<Portcullis> entry_ports = new List<Portcullis>();
        for (int i = 0; i < port_array.Length; ++i)
        {
            if (port_array[i].combatant_entrance)
                entry_ports.Add(port_array[i]);
        }
        if (entry_ports.Count < _instance.houses_in_play.Length)
            Debug.LogError("Not enough entrance ports in scene.");


        // assign a door randomly to each house
        doors_array = GeneralHelpers.ShuffleArray<ExitDoor>(doors_array);


        // save exit doors and entrance transforms
        for (int i = 0; i < _instance.houses_in_play.Length; ++i)
        {

            doors[_instance.houses_in_play[i]] = doors_array[i];
            entrance_points[_instance.houses_in_play[i]] = entry_ports[i].GetSpawnPoint();
            
        }
    }
    private static void ResetCurrentArenaKills()
    {
        if (houses == null) return;

        foreach (HouseName house_name in _instance.houses_in_play)
        {
            houses[house_name].ResetArenaKills();
            //Debug.Log("House " + house_name + " has " + houses[house_name].KillsCurrentArena + " kills in this arena.");
        }
    }


    // PUBLIC MODIFIERS

    public static ExitDoor GetHouseDoor(HouseName house_name)
    {
        return doors[house_name];
    }
    public static House GetHouse(HouseName house_name)
    {
        return houses[house_name];
    }
}
