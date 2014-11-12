using UnityEngine;
using System.Collections;

public class ArenaDetails : MonoBehaviour
{
    private static ArenaDetails _instance;
    public int required_kills = 20;

	
    public void Awake()
    {
        _instance = this;
    }
    public static int GetRequiredKills()
    {
        if (_instance == null) Debug.LogError("No ArenaDetails instance");
        return _instance.required_kills;
    }
}
