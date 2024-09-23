using Godot;
using System;
using System.Collections.Generic;

namespace ComparativeAdvantage
{

public class MCQuestion : MarginContainer, IHasDialogLabel
{
    [Signal]
    public delegate void AnswerSubmitted( MCQuestion question, bool solved );

    private RichTextLabel Label;
    private Control Answers;
    private Button Submit;
    private RichTextLabel Feedback;

    public String QuestionID { get; private set; }
    private int CorrectIndex;       // Index of the correct answer
    private int SelectedIndex;      // User selected answer
    private bool Solved;
    private ButtonGroup ButtonGroup;
    private List<String> FeedbackList;

    private PackedScene _Choice;

    public override void _Ready()
    {
        Label = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/RichTextLabel");
        Answers = GetNode<Control>("MarginContainer/VBoxContainer/MarginContainer/Answers");
        Submit = GetNode<Button>("MarginContainer/VBoxContainer/Submit");
        Feedback = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Feedback");

        CorrectIndex = -1;
        SelectedIndex = -1;
        Solved = false;
        ButtonGroup = new ButtonGroup();
        FeedbackList = new List<String>();

        _Choice = GD.Load<PackedScene>("res://Scenes/Dialog/Choice.tscn");
    }

    public void Initialize( Godot.Collections.Dictionary questionData )
    {
        Label.BbcodeText = Main.Variables.Format( questionData["text"] as String );
        
        QuestionID = questionData["id"] as String;
        CorrectIndex = Convert.ToUInt16( questionData["correct"] );
        
        Godot.Collections.Array list = questionData["content"] as Godot.Collections.Array;
        for (int i = 0; i < list.Count; i++)
        {
            String answer = list[i] as String;
            Choice choice = _Choice.Instance() as Choice;
            Answers.AddChild( choice );

            choice.ButtonGroup = ButtonGroup;
            choice.LabelText = answer;

            Godot.Collections.Array args = new Godot.Collections.Array { i };
            choice.CheckBox.Connect("pressed", this, nameof(_OnChoiceSelected), args );
        }

        foreach( String feedback in questionData["feedback"] as Godot.Collections.Array )
            FeedbackList.Add( Main.Variables.Format( feedback ) );
        
        Solved = (bool)questionData["previouslySolved"];
        if ( Solved )
            Feedback.Visible = true;
    }

    public void SetLabel( String label )
    {
        Label.Text = Main.Variables.Format( label );
    }

    public void _OnChoiceSelected( int index )
    {
        SelectedIndex = index;
        GD.Print( index );
    }

    public void _OnSubmitButtonPressed()
    {
        if ( SelectedIndex == CorrectIndex )
        {
            Solved = true;
            Feedback.Modulate = new Color( 0.9f, 1f, 0.9f, 1f );
        }
        else
            Feedback.Modulate = new Color( 1f, 0.9f, 0.9f, 1f );

        Feedback.Visible = true;
        Feedback.Text = FeedbackList[ SelectedIndex ];
        EmitSignal("AnswerSubmitted", this, Solved);
    }
}

}