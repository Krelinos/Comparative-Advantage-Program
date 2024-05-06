using ComparativeAdvantage;
using Godot;
using System;

public class Scenario1aOutputVisuals : Node
{
    private Scenario1aOutputUI UI;

    private FarmCrops LightCrops;
    private FarmCrops DarkCrops;

    public override void _Ready()
    {
        LightCrops = GetNode<FarmCrops>("LightCrops");
    }

    public void Initialize()
    {
        UI = GameService.Scenario.CurrentScenarioUI as Scenario1aOutputUI;
        UI.Connect( "LightCropChanged", this, nameof(UpdateLightCrops) );

        LightCrops.PlantCrops();
    }

    private void UpdateLightCrops( float oldVal, float newVal )
    {
        LightCrops.UpdateCrops( newVal );
    }
    
}
