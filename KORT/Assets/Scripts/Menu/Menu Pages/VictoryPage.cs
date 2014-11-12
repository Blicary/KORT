using UnityEngine;
using System.Collections;

public class VictoryPage : MenuPage 
{
    public GUISkin heading_skin;
    public GUISkin small_skin;

    public Combatant player;


    public void OnGUI()
    {
        GUI.skin = small_skin;
        EnableGUIScale();
        float t = TransitionPow();

        WindowFade(t);
        GUI.color = Color.clear;
        GUI.Window(0, new Rect(0, 0, default_screen.width, default_screen.height), Window, "");
        GUI.color = Color.white;
    }

    private void Window(int windowID)
    {
        float t = TransitionPow();

        GUILayout.BeginArea(new Rect(120, 120, 1000, 800));

        // header
        GUI.skin = heading_skin;
        House house = HouseManager.GetHouse(player.house_name);
        MenuHelper.GUILayoutHeader("House " + house.Name + " has triumphed", t);

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
        NextVerticalKeyboardControl("RETRY");
        if (GUILayout.Button("RETRY", GUILayout.Width(800)) || KBControlPressed("RETRY"))
        {
            GameManager.LoadGame();
        }
        if (LastControlHover("RETRY")) { SetKeyBoardFocus("RETRY"); }
        NextVerticalKeyboardControl("MAIN MENU");
        if (GUILayout.Button("MAIN MENU", GUILayout.Width(800)) || KBControlPressed("MAIN MENU"))
        {
            GameManager.LoadMainMenu();
        }
        if (LastControlHover("MAIN MENU")) { SetKeyBoardFocus("MAIN MENU"); }
        GUILayout.EndArea();

        EndKeyboardControlSetup();
    }

    private void WindowFade(float t)
    {
        GUI.color = Color.black;
        MenuHelper.SetGUIAlpha(t / 2f);
        GUI.Box(default_screen, "");
        GUI.color = Color.white;
    }

    private string ConstructInfoText()
    {
        House house = HouseManager.GetHouse(player.house_name);
        CombatantStats dead_stats = house.GetCurrentCombatantStats();

        string text = "!!! =)";

        return text;
    }

}
