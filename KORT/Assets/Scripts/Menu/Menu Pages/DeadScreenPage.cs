using UnityEngine;
using System.Collections;

public class DeadScreenPage : MenuPage 
{
    public GUISkin heading_skin;
    public GUISkin small_skin;

    public Combatant player;


    public void OnGUI()
    {
        //if (name == "Briefing Page") Debug.Log(base.tran_state);


        EnableGUIScale();
        float t = TransitionPow();

        GUILayout.BeginArea(new Rect(120, 120, 1000, 800));

        // header
        GUI.skin = heading_skin;
        MenuHelper.GUILayoutHeader("DEAD", t);

        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(120 + (-1100 * (1 - t)), 120, 1000, 800));
        GUILayout.BeginVertical();
        GUILayout.Space(150);

        // text
        GUI.skin = small_skin;
        GUILayout.Label(ConstructInfoText(), GUILayout.Width(1000));

        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0 + (-1100 * (1 - t)), 800, 1000, 200));
        NextVerticalKeyboardControl("CONTINUE");
        if (GUILayout.Button("CONTINUE", GUILayout.Width(800)) || KBControlPressed("CONTINUE"))
        {
            GameManager.screen_fade.FadeToBlack(transition_seconds);
            this.TransitionOut(() => GameManager.NextCombatant());
        }
        if (LastControlHover("CONTINUE")) { SetKeyBoardFocus("CONTINUE"); }
        GUILayout.EndArea();

        EndKeyboardControlSetup();

    }

    private string ConstructInfoText()
    {
        House house = HouseManager.GetHouse(player.house_name);
        CombatantStats dead_stats = house.GetLastCombatantStats();
        CombatantStats stats = house.GetCurrentCombatantStats();

        string text = dead_stats.name + " lost his life after " + dead_stats.time_alive + " seconds in the arena. \n";
        text += (house.CombatantsLeft() + 1) + " combatants remain from House " + house.Name + ". \n";
        text += "\n";
        text += stats.name + " prepares to take up the fight...";

        return text;
    }

}
