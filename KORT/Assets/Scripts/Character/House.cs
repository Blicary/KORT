using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum HouseName { Stark }

public class House
{
    public HouseName Name { get; private set; }

    private CombatantStats[] combatant_stats;
    private int current_combatant = 0;

    public int TotalKills { get; private set; }
    public int KillsCurrentArena { get; private set; }


    // PUBLIC MODIFIERS

    public House(HouseName name, int num_combatants)
    {
        combatant_stats = new CombatantStats[num_combatants];
        CreateNewCombatant();
    }

    public void NextCombatant()
    {
        ++current_combatant;
        CreateNewCombatant();
    }
    public void ResetArenaKills()
    {
        KillsCurrentArena = 0;
    }
    public int CombatantsLeft()
    {
        return combatant_stats.Length - current_combatant - 1;
    }
    public void RecordKill()
    {
        ++KillsCurrentArena;
        ++TotalKills;
    }


    // PRIVATE MODIFIERS

    private void CreateNewCombatant()
    {
        combatant_stats[current_combatant] = new CombatantStats();
        combatant_stats[current_combatant].name = "No Name";
    }


    // PUBLIC ACCESSORS

    public CombatantStats GetCurrentCombatantStats()
    {
        return combatant_stats[current_combatant];
    }
    public CombatantStats GetLastCombatantStats()
    {
        if (current_combatant == 0) return null;
        return combatant_stats[current_combatant-1];
    }
}

