using Godot;
using System;
using System.Runtime.Serialization.Json;

namespace ComparativeAdvantage
{

public class SaveService
{
    public Godot.Collections.Dictionary SaveData { get; private set; }

    public Godot.Collections.Dictionary LoadSaveFile()
    {
        Godot.File saveFile = new Godot.File();
        if ( saveFile.FileExists("user://userprogress.json") )
        {
            saveFile.Open("user://userprogress.json", File.ModeFlags.Read);

            SaveData = Game.ParseJSON( "userprogress.json", "user://" ).Result as Godot.Collections.Dictionary;
            GD.Print(SaveData);

            saveFile.Close();
        }
        else
        {
            var scenInfo = new Godot.Collections.Dictionary
            {
                { "Scenario1aOutput", new Godot.Collections.Array() },
                { "Scenario1bInput", new Godot.Collections.Array() }
            };

            SaveData = new Godot.Collections.Dictionary
            {
                { "solved", scenInfo }
            };

            saveFile.Open("user://userprogress.json", File.ModeFlags.Write);
            saveFile.StoreString( JSON.Print(SaveData, "\t") );
            saveFile.Close();
        }

        return SaveData;
    }

    public void Save()
    {
        Godot.File saveFile = new Godot.File();
        saveFile.Open("user://userprogress.json", File.ModeFlags.Write);

        String saveJSON = JSON.Print( SaveData, "\t" );
        saveFile.StoreString( saveJSON );
        saveFile.Close();
    }

    public void UserSolvedQuestionOfScenario( String scenarioName, String questionId )
    {
        var solved = GetSolvedQuestionsOfScenario( scenarioName );
        if ( !solved.Contains( questionId ) )
        {
            solved.Add( questionId );
            Save();
        }
    }

    public Godot.Collections.Array GetSolvedQuestionsOfScenario( String scenarioName )
    {
        var solved = SaveData["solved"] as Godot.Collections.Dictionary;
        return solved[ scenarioName ] as Godot.Collections.Array;
        // It is important to note that despite the casts and unwrapping, the return
        // is pass by reference and altering it still effects SaveData. 
    }
}

}