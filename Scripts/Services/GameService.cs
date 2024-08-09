using Godot;
using System;
using System.Dynamic;

namespace ComparativeAdvantage
{
public class GameService : Node
{
	[Export] protected NodePath ScenarioVisualsPath;
	[Export] protected NodePath ScenarioUIPath;
	[Export] protected NodePath DialogPath;
	[Export] protected NodePath DefinitionsPath;

	public static Godot.RandomNumberGenerator RNG { get; private set; }
	public static SaveService Save { get; private set; }
	public static ScenarioService Scenario { get; private set; }
	public static VariablesService Variables { get; private set; }
	public static Node ScenarioVisualsContainer { get; private set; }
	public static Node ScenarioUIContainer { get; private set; }
	public static Dialog Dialog { get; private set; }
	public static DefinitionsList DefinitionsList { get; private set; }

    public GameService()
	{
		RNG = new RandomNumberGenerator
		{
			Seed = (ulong)$"until you call again, vivienne".GetHashCode(),
			State = 0
		};

		Save = new SaveService();
		Scenario = new ScenarioService();
		Variables = new VariablesService();
	}

	public override void _Ready()
	{
		ScenarioVisualsContainer = GetNode( ScenarioVisualsPath );
		ScenarioUIContainer = GetNode( ScenarioUIPath );
		
		Save.LoadSaveFile();

		Dialog = GetNode<Dialog>( DialogPath );
		DefinitionsList = GetNode<DefinitionsList>( DefinitionsPath );

		DefinitionsList.Initialize();

        OnScenarioButtonPressed("0Preface");
	}

	public static JSONParseResult ParseJSON( String fileName, String filePath )
	{
		// fileName = fileName.ToLower();
		File JsonFile = new File();
		
		// 28 Sept 2023 - ModeFlag is Case-Sensitive; .READ will error, but .Read is fine
		// Probably a C# thing. This may apply to other Enums as well.
		Error e = JsonFile.Open($"{filePath}{fileName}", File.ModeFlags.Read);
		
		if ( e != Error.Ok )
			GD.PushError( String.Format("Tried to open file '{0}', but received error '{1}'.", fileName, e.ToString()) );
		
		JSONParseResult json = JSON.Parse( JsonFile.GetAsText() );
		JsonFile.Close();
		
		if ( json.Error != Error.Ok )
			GD.PushError( String.Format("Tried to parse JSON of file '{0}', but received error '{1}'.", fileName, json.Error.ToString()) );
	
		return json;
	}

	private void OnScenarioButtonPressed( String scenarioName )
	{
		Scenario.Restart();
		Scenario.LoadScenario("Scenario" + scenarioName);

		var visuals = Scenario.CurrentScenarioVisuals as Scenario1aOutputVisuals;
		if ( visuals != null )
			visuals.Initialize();
		
		var ui = Scenario.CurrentScenarioUI as Scenario1aOutputUI;
		if ( ui != null )
			ui.Initialize();

		Dialog.Restart();
		Dialog.ProceedToNextDialog();
	}
    
}
}