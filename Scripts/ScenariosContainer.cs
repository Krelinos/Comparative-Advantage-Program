using ComparativeAdvantage;
using ComparativeAdvantage.mobile;
using Godot;
using System;

public class ScenariosContainer : VBoxContainer
{
    [Export] private NodePath _SideMenu;

    private SideMenu SideMenu;

    public override void _Ready()
    {
        SideMenu = GetNode<SideMenu>( _SideMenu );

        Main.Scenarios.Connect(
            nameof(Scenarios.ScenarioLoaded),
            this, nameof(OnScenarioLoaded) );
    }

    protected void OnScenarioLoaded( String scenarioId )
    {
        SideMenu.CloseMenu();
    }
}
