using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ComparativeAdvantage
{

public class VariablesService
{
    public object this[ String key ]    // Whenever "VariablesService[ key ]" is used, this is executed.
    {
        get { return Variables[ key ]; }
        set { }
    }

    public readonly System.Collections.Generic.Dictionary< String, object > Variables;

    public VariablesService()
    {
        Variables = new System.Collections.Generic.Dictionary<string, object>();

        // Interpret all varibles into an array. Also allows basic arithmetic.
        Godot.Collections.Dictionary jsonVars = Game.ParseJSON("variables.json", "res://Dialog/").Result as Godot.Collections.Dictionary;
        foreach ( String key in jsonVars.Keys )
        {
            String s = jsonVars[ key ] as String;
            if ( s != null )	// This variable successfully casts into a String. If it couldn't, 's' would be null.
            {
                if ( s.BeginsWith("ri") ) // ri stands for "random [range of] integer(s)"
                {
                    int lBracket = s.IndexOf("[");
                    int rBracket = s.IndexOf("]");
                    // GD.Print( s.Substring( lBracket + 1, rBracket - lBracket - 1 ) );
                    
                    String[] nums = s.Substring( lBracket + 1, rBracket - lBracket - 1 ).Split(",");
                    Variables[ key ] = Game.RNG.RandiRange( Int32.Parse(nums[0]), Int32.Parse(nums[1]) );
                }
            }
            
            // Variables can also appear as a Godot Dictionary. Used for variables that depend on previous variables.
            Godot.Collections.Dictionary d = jsonVars[ key ] as Godot.Collections.Dictionary;
            if ( d != null )
            {
                Godot.Collections.Array dData = d["data"] as Godot.Collections.Array;
                List<object> nums = new List<object>();
                
                foreach ( String dVar in dData )
                    nums.Add( Variables[dVar] );
                
                String expression = String.Format( d["formula"] as String, nums.ToArray() );
                // GD.Print( expression );
                
                DataTable dt = new DataTable();
                double answer = (double)dt.Compute( expression, "" );
                Variables[ key ] = Math.Round( answer, 3 );	// AP test require rounding to the 3rd decimal, unless specified.
            }
            
            // GD.Print( $"{key} = {_variables[key]}" );
        }
    }

    // Similar to String.Format(), this replaces contents within curly brackets with data.
	// Ex: The string "Test {variable_name} example" becomes "Test 1 example"
	// 	if a variable with the ID/key of "variable_name" in the variables dictionary exists.
	// Variable keys/values that have curly brackets will break this, but that should not be an issue.
	public String Format( String str )
	{
		Int32 lBracket = str.IndexOf('{');
		Int32 rBracket = str.IndexOf('}');
		String varKey;
		
		while ( lBracket != -1 && rBracket != -1 )
		{
			varKey = str.Substring( lBracket + 1, rBracket - lBracket - 1 );
			str = str.Remove( lBracket, rBracket - lBracket + 1 );

            switch ( Variables[ varKey ] )
            {
                case double d:
                    str = str.Insert( lBracket, Math.Round( (double)Variables[ varKey ], 3 ).ToString() );
                    break;
                case int i:
                case String s:
                    GD.Print(varKey);
                    str = str.Insert( lBracket, Variables[ varKey ].ToString() );
                    break;
                default:
                    str = str.Insert( lBracket, "NULLVARTYPE" );
                    break;
            }
			
			lBracket = str.IndexOf('{');
			rBracket = str.IndexOf('}');
		}
		
		return str;
	}
}

}