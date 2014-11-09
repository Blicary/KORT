using UnityEngine;
using System.Collections;

public class ArenaDetails : MonoBehaviour
{
    private static ArenaDetails _instance;
    public int required_kills = 20;
    public int num_combatants_per_house = 5;

	
    public void Awake()
    {
        _instance = this;
    }
    public static int GetRequiredKills()
    {
        if (_instance == null) Debug.LogError("No ArenaDetails instance");
        return _instance.required_kills;
    }
    public static int GetNumCombatantsPerHouse()
    {
        if (_instance == null) Debug.LogError("No ArenaDetails instance");
        return _instance.num_combatants_per_house;
    }
}
