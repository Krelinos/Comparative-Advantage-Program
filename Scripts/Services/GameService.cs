using Godot;
using System;
using System.Dynamic;

namespace ComparativeAdvantage
{
public static class Game
{
	public static Godot.RandomNumberGenerator RNG { get; private set; }
	public static SaveService Save { get; private set; }
	public static ScenarioService Scenario { get; private set; }
	public static VariablesService Variables { get; private set; }

	private static Node ScenarioVisualsContainer;
	private static Node ScenarioUIContainer;

    static Game()
	{
		RNG = new RandomNumberGenerator
		{
			Seed = (ulong)$"vivienne, until you call again".GetHashCode(),
			State = 0
		};

		Save = new SaveService();
		Scenario = new ScenarioService();
		Variables = new VariablesService();

		// ScenarioVisualsContainer = Get

		Save.LoadSaveFile();
        Scenario.LoadScenario("Scenario1aOutput");
	}

    public static JSONParseResult ParseJSON( String fileName, String filePath )
	{
		// fileName = fileName.ToLower();
		File JsonFile = new File();
		
		// 28 Sept 2023 - ModeFlag is Case-Sensitive; .READ will error, but .Read is fine
		// Probably a C# thing. This may apply to other Enums as well.
		Error e = JsonFile.Open($"{filePath}{fileName}", File.ModeFlags.Read);
		
		if ( e != Error.Ok )
			GD.PushError( String.Format("Tried to open scenario file '{0}', but received error '{1}'.", fileName, e.ToString()) );
		
		JSONParseResult json = JSON.Parse( JsonFile.GetAsText() );
		JsonFile.Close();
		
		if ( json.Error != Error.Ok )
			GD.PushError( String.Format("Tried to parse JSON of scenario '{0}', but received error '{1}'.", fileName, json.Error.ToString()) );
	
		return json;
	}
    
}
}