using Godot;
using System;

namespace ComparativeAdvantage {

public class Scenario0Preface : Control
{
    private Button Prev;
    private Button Next;
    private Label PageIndicator;
    private RichTextLabel Credits;

    private int CurrentPage;
    private Godot.Collections.Array Pages;

    public override void _Ready()
    {
        Prev = GetNode<Button>("VBoxContainer/HBoxContainer/Previous");
        Next = GetNode<Button>("VBoxContainer/HBoxContainer/Next");
        PageIndicator = GetNode<Label>("VBoxContainer/HBoxContainer/PageIndicator");
        Credits = GetNode<RichTextLabel>("VBoxContainer/Credits");
    
        Pages = Main.ParseJSON("credits.json", "res://Dialog/").Result as Godot.Collections.Array;

        Prev.Connect( "pressed", this, nameof(ButtonPressed), new Godot.Collections.Array { true } );
        Next.Connect( "pressed", this, nameof(ButtonPressed), new Godot.Collections.Array { false } );
    
        UpdateCreditsAndPageIndicator();
    }

    private void ButtonPressed( bool prev )
    {
        if ( prev )
            CurrentPage += 1;
        else
            CurrentPage -= 1;
        
        // 6 Oct 2024 - C# and C++ % operator is remainder and not modulo.
        // Remainder allows for negative results, modulo only allows positive.
        // https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
        CurrentPage = ( CurrentPage %= Pages.Count ) < 0 ? CurrentPage + Pages.Count : CurrentPage;

        UpdateCreditsAndPageIndicator();
    }

    private void UpdateCreditsAndPageIndicator()
    {
        Credits.BbcodeText = Pages[CurrentPage] as String;
        PageIndicator.Text = String.Format( "{0} / {1}", CurrentPage+1, Pages.Count );
    }
}

} // namespace ComparativeAdvantage