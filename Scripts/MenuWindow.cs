using Godot;
using System;

namespace ComparativeAdvantage
{

public class MenuWindow : PanelContainer
{
    private Button Menu;
    private LineEdit Seed;
    private Button ResetScenario;
    private Button ResetGlossary;
    private Button ResetAnswersCounter;
    private Button ApplyClose;
    private Button Close;

    private bool ResetScenarioToggle;
    private bool ResetGlossaryToggle;
    private bool ResetAnswersCounterToggle;

    public override void _Ready()
    {
        Menu = GetNode<Button>("../MarginContainer/HBoxContainer/VBoxContainer/ScenariosMenu/MarginContainer/VBoxContainer/Menu");
        Node options = GetNode("MarginContainer/MarginContainer/VBoxContainer");

        Seed = options.GetNode<LineEdit>("RandomizingSeed/HBoxContainer/LineEdit");
        ResetScenario = options.GetNode<Button>("ResetScenarioQuestions/MenuButton");
        ResetGlossary = options.GetNode<Button>("ResetGlossary/MenuButton");
        ResetAnswersCounter = options.GetNode<Button>("ResetAnswersCounter/MenuButton");

        ApplyClose = options.GetNode<Button>("Exit/ApplyClose");
        Close = options.GetNode<Button>("Exit/Close");

        ResetScenario.Connect("pressed", this, nameof(ResetScenarioPressed));
        ResetGlossary.Connect("pressed", this, nameof(ResetGlossaryPressed));

        ApplyClose.Connect("pressed", this, nameof(Apply));

        Menu.Connect("pressed", this, nameof(OpenMenu));
        Close.Connect("pressed", this, nameof(CloseMenu));
    }

    private void ResetScenarioPressed()
    {
        ResetScenarioToggle = true;
        ResetScenario.Text = "Press Apply to confirm";
    }
    private void ResetGlossaryPressed()
    {
        ResetGlossaryToggle = true;
        ResetGlossary.Text = "Press Apply to confirm";
    }

    private void ResetAnswersCounterPressed()
    {
        ResetAnswersCounterToggle = true;
        ResetAnswersCounter.Text = "Press Apply to confirm";
    }

    private void Apply()
    {
        GameService.Save.SaveData["seed"] = Seed.Text;

        if ( ResetScenarioToggle )
            GameService.Save.ResetScenarioProgress();
        if ( ResetGlossaryToggle )
        {
            GameService.DefinitionsList.Reset();
            GameService.Save.ResetGlossary();
        }
        // if ( ResetQuestionsToggle )
            // GameService.Save.ResetQuestionCount();
        GameService.Save.Save();

        CloseMenu();
    }

    private void OpenMenu()
    {
        Visible = true;
    }

    private void CloseMenu()
    {
        Visible = false;
        ResetScenarioToggle = false;
        ResetGlossaryToggle = false;
        ResetAnswersCounterToggle = false;

        ResetScenario.Text = "Reset Scenario Progress";
        ResetGlossary.Text = "Reset Glossary";
        ResetAnswersCounter.Text = "Reset Answer Counter";
    }
}

}