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
        // JSON doesn't like BBcode tags using []s and newlines together, so I used <>s temporarily then decode that here.
        for ( int i = 0; i < Pages.Count; i++ )
            Pages[i] = (Pages[i] as String).Replace('<', '[').Replace('>', ']');

        Prev.Connect( "pressed", this, nameof(ButtonPressed), new Godot.Collections.Array { true } );
        Next.Connect( "pressed", this, nameof(ButtonPressed), new Godot.Collections.Array { false } );
    
        UpdateCreditsAndPageIndicator();
    }

    private void ButtonPressed( bool prev )
    {
        if ( prev )
            CurrentPage = (CurrentPage - 1) % Pages.Count;
        else
            CurrentPage = (CurrentPage + 1) % Pages.Count;

        UpdateCreditsAndPageIndicator();
    }

    private void UpdateCreditsAndPageIndicator()
    {
        Credits.BbcodeText = Pages[CurrentPage] as String;
        PageIndicator.Text = String.Format( "{0} / {1}", CurrentPage+1, Pages.Count );
    }
}

} // namespace ComparativeAdvantage