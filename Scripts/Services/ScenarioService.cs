using Godot;
using System;
using System.Collections.Generic;

namespace ComparativeAdvantage
{

public class ScenarioService
{
    private Godot.Collections.Dictionary Dialog;
    private Godot.Collections.Dictionary Questions;
    public String CurrentScenario { get; private set; }
    public Node CurrentScenarioVisuals { get; private set; }
	public Node CurrentScenarioUI { get; private set; }
    
    public void LoadScenario( String scenarioName )
    {
        CurrentScenario = scenarioName;

        Godot.Collections.Dictionary scenarioInfo = GameService.ParseJSON( $"{scenarioName}.json", "res://Dialog/" ).Result as Godot.Collections.Dictionary;
        Dialog = scenarioInfo[ "dialog" ] as Godot.Collections.Dictionary;
        Questions = scenarioInfo[ "questions" ] as Godot.Collections.Dictionary;

        CurrentScenarioVisuals = GD.Load<PackedScene>($"res://Scenes/Scenarios/Visuals/{scenarioName}.tscn").Instance();
        CurrentScenarioUI = GD.Load<PackedScene>($"res://Scenes/Scenarios/UI/{scenarioName}.tscn").Instance();
    
        GameService.ScenarioVisualsContainer.AddChild( CurrentScenarioVisuals );
        GameService.ScenarioUIContainer.AddChild( CurrentScenarioUI );

        GD.Print($"Loaded scenario '{scenarioName}'");
    }

    public void Restart()
    {
        Dialog = null;
        Questions = null;
        CurrentScenario = null;
        CurrentScenarioVisuals?.QueueFree();
        CurrentScenarioUI?.QueueFree();
    }

    public Godot.Collections.Dictionary GetDialog( String dialogId )
    {
        return Dialog[ dialogId ] as Godot.Collections.Dictionary;
    }

    public Godot.Collections.Dictionary GetQuestion( String questionId )
    {
        return Questions[ questionId ] as Godot.Collections.Dictionary;
    }

    public String GetStartID()
    {
        return Dialog[ "start" ] as String;
    }
}

}