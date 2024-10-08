using Godot;
using System;

namespace ComparativeAdvantage { namespace mobile {

/// <summary>
/// Displays the user's learned terms in a list. Pulls term
/// descriptions from Main.Glossary.
/// </summary>
public class Terms : NinePatchRect
{
    [Export] protected NodePath __TermsList;
    [Export] protected NodePath __TermDetails;

    public Control TermEntries
    {
        get { return _TermEntries; }
        protected set { _TermEntries = value; }
    }

    public TermDetails TermDetails
    {
        get { return _TermDetails; }
        protected set { _TermDetails = value; }
    }
    
    protected PackedScene _TermEntry;
    private Control _TermEntries;
    private TermDetails _TermDetails;

    public override void _Ready()
    {
        TermEntries = GetNode<Control>( __TermsList );
        TermDetails = GetNode<TermDetails>( __TermDetails );

        _TermEntry = GD.Load("res://Scenes/Mobile/Definition.tscn") as PackedScene;
    }

    /// <summary>
    /// Appends a term to the Terms's list. Ignores duplicate terms.
    /// </summary>
    /// <param name="termIndex">The key value of the term in the Glossary.</param>
    public void AddTerm( String termIndex )
    {
        // Do not add duplicate terms.
        foreach ( Node n in TermEntries.GetChildren() )
            if ( n.Name == termIndex )
                return;

        var termEntry = _TermEntry.Instance() as Definition;
        TermEntries.AddChild( termEntry );
        termEntry.Name = termIndex;
        termEntry.TermLabel.Text = termIndex.Capitalize();

        termEntry.Connect( "ButtonPressed", TermDetails, nameof(TermDetails.DisplayDetailsForTerm),
            new Godot.Collections.Array { termIndex }, 0 );
    }

    public void ClearTerms()
    {
        foreach( Node n in TermEntries.GetChildren() )
            n.QueueFree();
    }
}

} // namespace mobile
} // namespace ComparativeAdvantage