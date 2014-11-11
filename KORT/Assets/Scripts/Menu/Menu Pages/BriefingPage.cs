using UnityEngine;
using System.Collections;

public class BriefingPage : MenuPage 
{
    public GUISkin heading_skin;
    public GUISkin small_skin;
    public MenuPage main_menu_page;


    public void OnGUI()
    {
        //if (name == "Briefing Page") Debug.Log(base.tran_state);

        
        EnableGUIScale();
        float t = TransitionPow();

        GUILayout.BeginArea(new Rect(120, 120, 1000, 800));

        // header
        GUI.skin = heading_skin;
        MenuHelper.GUILayoutHeader("Briefing", t);

        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(120 + (-1100 * (1 - t)), 120, 1000, 800));
        GUILayout.BeginVertical();
        GUILayout.Space(150);

        // text
        GUI.skin = small_skin;
        string text = "Kill stuff and win!!! \n";
        text +=       "\n";

        GUILayout.Label(text, GUILayout.Width(1000));

        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0 + (-1100 * (1 - t)), 800, 1000, 200));
        NextVerticalKeyboardControl("READY");
        if (GUILayout.Button("READY", GUILayout.Width(800)) || KBControlPressed("READY"))
        {
            if (!main_menu_page) return;
            GameManager.screen_fade.FadeToBlack(transition_seconds);
            this.TransitionOut(() => GameManager.StartGame());
        }
        if (LastControlHover("READY")) { SetKeyBoardFocus("READY"); }
        NextVerticalKeyboardControl("BACK");
        if (GUILayout.Button("BACK", GUILayout.Width(800)) || KBControlPressed("BACK"))
        {
            if (!main_menu_page) return;
            this.TransitionOut(null);
            main_menu_page.TransitionIn(null);
        }
        if (LastControlHover("BACK")) { SetKeyBoardFocus("BACK"); }
        GUILayout.EndArea();

        EndKeyboardControlSetup();
        
    }
    public new void Update()
    {
        base.Update();

    }
}
