using Godot;
using System;

public class MenuDialog : PanelContainer
{
    private Button Menu;
    private LineEdit Seed;
    private Button ResetScenario;
    private Button ResetQuestions;
    private Button ApplyClose;
    private Button Close;

    public override void _Ready()
    {
        Menu = GetNode<Button>("../HBoxContainer/VBoxContainer/ScenariosMenu/MarginContainer/VBoxContainer/Menu");
        Node options = GetNode("MarginContainer/MarginContainer/VBoxContainer");
        Seed = options.GetNode<LineEdit>("RandomizingSeed/HBoxContainer/LineEdit");
        ResetScenario = options.GetNode<Button>("ResetScenarioQuestions/Button");
        ResetQuestions = options.GetNode<Button>("ResetQuestionCounter/Button");
        ApplyClose = options.GetNode<Button>("Exit/ApplyClose");
        Close = options.GetNode<Button>("Exit/Close");

        Menu.Connect("pressed", this, nameof(OpenMenu));
        Close.Connect("pressed", this, nameof(CloseMenu));
    }

    private void OpenMenu()
    {
        Visible = true;
    }

    private void CloseMenu()
    {
        Visible = false;
    }
}
