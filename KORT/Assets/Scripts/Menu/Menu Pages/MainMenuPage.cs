using UnityEngine;
using System.Collections;

public class MainMenuPage : MenuPage 
{
    public GUISkin skin1;
    public MenuPage briefing_page;
    public YesNoPopup yes_no_pu;


    public void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();

        GiveControlToPopups(new MenuPage[] { yes_no_pu });
        GUILayout.BeginArea(new Rect(0, 230, 1920, 800));
        GUILayout.BeginVertical();

        // header
        MenuHelper.GUILayoutHeader("", t);
        
        // buttons
        NextVerticalKeyboardControl("START");
        if (GUILayout.Button("START", GUILayout.Width(800 * t)) || KBControlPressed("START"))
        {
            if (!briefing_page) return; 
            TransitionOut(null);
            briefing_page.TransitionIn(null);
        }
        if (LastControlHover("START")) { SetKeyBoardFocus("START"); }
        NextVerticalKeyboardControl("EXIT");
        if (GUILayout.Button("EXIT", GUILayout.Width(800 * t)) || KBControlPressed("EXIT"))
        {
            if (!yes_no_pu) return;
            yes_no_pu.question_text = "Are you sure you want to exit?";
            yes_no_pu.TransitionIn(null);
            yes_no_pu.on_no = null;
            yes_no_pu.on_yes = () => Application.Quit();
        }
        if (LastControlHover("EXIT")) { SetKeyBoardFocus("EXIT"); }

        GUILayout.EndVertical();
        GUILayout.EndArea();


        EndKeyboardControlSetup();
        
    }
}
