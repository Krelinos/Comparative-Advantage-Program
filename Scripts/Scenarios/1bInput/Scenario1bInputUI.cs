using Godot;
using System;

namespace ComparativeAdvantage {

public class Scenario1bInputUI : MarginContainer, IScenarioVisualsOrUI
{
    [Export] private NodePath _minsTable;
    [Export] private NodePath _opTable;

    [Export] private NodePath _minsCell11;
    [Export] private NodePath _minsCell12;
    [Export] private NodePath _minsCell21;
    [Export] private NodePath _minsCell22;

    [Export] private NodePath _opCell11;
    [Export] private NodePath _opCell12;
    [Export] private NodePath _opCell21;
    [Export] private NodePath _opCell22;

    private const String MINUTES_SUFFIX = " mins";

    public override void _Ready()
    {
        GetNode<Label>( _minsCell11 ).Text = Main.Variables["baker_pumpkin"].ToString() + MINUTES_SUFFIX;
        GetNode<Label>( _minsCell21 ).Text = Main.Variables["baker_cheese"].ToString() + MINUTES_SUFFIX;
        GetNode<Label>( _minsCell12 ).Text = Main.Variables["apprentice_pumpkin"].ToString() + MINUTES_SUFFIX;
        GetNode<Label>( _minsCell22 ).Text = Main.Variables["apprentice_cheese"].ToString() + MINUTES_SUFFIX;
        GetNode<Label>( _opCell11 ).Text = Main.Variables["baker_pumpkin_opcost"].ToString();
    }

    public async void OnDialogVisualsEvent( String visualsId )
    {
        switch ( visualsId )
        {
            case "show_food":
                break;
            case "show_minutes_table":
                var tween = new Tween();
                tween.InterpolateProperty( GetNode<Control>(_minsTable), "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 1 );
                AddChild( tween );
                tween.Start();
                await ToSignal( tween, "tween_completed" );
                tween.QueueFree();
                break;
            case "show_minutes_table_apprentice":
                var tween2 = new Tween();
                tween2.InterpolateProperty( GetNode<Label>(_minsCell12), "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 1 );
                AddChild( tween2 );
                tween2.Start();
                var tween3 = new Tween();
                tween3.InterpolateProperty( GetNode<Label>(_minsCell22), "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 1 );
                AddChild( tween3 );
                tween3.Start();

                await ToSignal( tween2, "tween_completed" );
                tween2.QueueFree();
                tween3.QueueFree();
                break;
            case "move_actors_to_counters":
                break;
            case "baker_create_pumpkin":
                break;
            case "show_opcost_table":
                var tween4 = new Tween();
                    tween4.InterpolateProperty( GetNode<Control>(_opTable), "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 1 );
                    AddChild( tween4 );
                    tween4.Start();

                    await ToSignal( tween4, "tween_completed" );
                    tween4.QueueFree();
                break;
            case "reveal_opcost_of_baker_cheese":
                GetNode<Label>( _opCell21 ).Text = Main.Variables["baker_cheese_opcost"].ToString();
                break;
            case "reveal_opcost_of_apprentice":
                GetNode<Label>( _opCell12 ).Text = Main.Variables["apprentice_pumpkin_opcost"].ToString();
                GetNode<Label>( _opCell22 ).Text = Main.Variables["apprentice_cheese_opcost"].ToString();
                break;
        }
    }

}

} // namespace ComparativeAdvantage