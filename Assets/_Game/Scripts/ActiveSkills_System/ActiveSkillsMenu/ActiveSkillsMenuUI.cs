using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillsMenuUI : Game_UI
{


    protected override void OnEnable()
    {
        base.OnEnable();

        ActiveSkillsMenuDisplayer.OnDisplayActiveSkillsMenu += OnDisplayActiveSkillsMenu;
        ActiveSkillsMenu_ButtonExit.OnActiveSkillsExitButtonPressed += OnActiveSkillsExitButtonPressed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        ActiveSkillsMenuDisplayer.OnDisplayActiveSkillsMenu -= OnDisplayActiveSkillsMenu;
        ActiveSkillsMenu_ButtonExit.OnActiveSkillsExitButtonPressed -= OnActiveSkillsExitButtonPressed;
    }

    private void Start()
    {
        ToggleUI(false);
    }

    private void OnActiveSkillsExitButtonPressed()
    {
        CloseUI();
    }

    private void OnDisplayActiveSkillsMenu()
    {
        OpenUI();
    }

}
