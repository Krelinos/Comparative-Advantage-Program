using ComparativeAdvantage;
using Godot;
using System;

public class MenuDialog : PanelContainer
{
    private Button Menu;
    private LineEdit Seed;
    private Button ResetScenario;
    private Button ResetGlossary;
    private Button ResetQuestions;
    private Button ApplyClose;
    private Button Close;

    private bool ResetScenarioToggle;
    private bool ResetGlossaryToggle;
    private bool ResetQuestionsToggle;

    public override void _Ready()
    {
        Menu = GetNode<Button>("../HBoxContainer/VBoxContainer/ScenariosMenu/MarginContainer/VBoxContainer/Menu");
        Node options = GetNode("MarginContainer/MarginContainer/VBoxContainer");

        Seed = options.GetNode<LineEdit>("RandomizingSeed/HBoxContainer/LineEdit");
        ResetScenario = options.GetNode<Button>("ResetScenarioQuestions/Button");
        ResetGlossary = options.GetNode<Button>("ResetGlossary/Button");
        ResetQuestions = options.GetNode<Button>("ResetQuestionCounter/Button");

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

    private void ResetQuestionsPressed()
    {
        ResetQuestionsToggle = true;
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

        ResetScenario.Text = "Reset Scenario Progress";
        ResetGlossary.Text = "Reset Glossary";
    }
}
