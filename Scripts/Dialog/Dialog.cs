using ComparativeAdvantage;
using Godot;
using System;
using System.Collections.Generic;

public class Dialog : NinePatchRect
{
    private ScrollContainer DialogScroll;
    private Control DialogContainer;
    private Button ContinueButton;
    private Timer ScrollTimer;

    private String NextDialogID;
    private bool IsDialogPaused;

    private const String CONTINUE_CONTINUE = "Click here or press space to continue.";
    private const String CONTINUE_PAUSE    = "Solve the above question to continue.";
    private const String CONTINUE_END      = "- End of scenario -";

    private PackedScene _DialogBasic;
    private PackedScene _DialogMCQuestion;

    public override void _Ready()
    {
        DialogScroll = GetNode<ScrollContainer>("MarginContainer/ScrollContainer");
        DialogContainer = GetNode<Control>("MarginContainer/ScrollContainer/VBoxContainer");
        ContinueButton = GetNode<Button>("MarginContainer2/Continue");
        ScrollTimer = GetNode<Timer>("ScrollTimer");

        NextDialogID = "";
        IsDialogPaused = false;

        _DialogBasic = GD.Load<PackedScene>("res://Scenes/Dialog/Basic.tscn");
        _DialogMCQuestion = GD.Load<PackedScene>("res://Scenes/Dialog/MCQuestion.tscn");

        Restart();
    }

    public void ProceedToNextDialog()
    {
        Godot.Collections.Dictionary dialog = Game.Scenario.GetDialog( NextDialogID );
        
        // next
        try
        {
            NextDialogID = dialog["next"] as String;
            GD.Print( NextDialogID );
        }
        catch( KeyNotFoundException )
        {
            NextDialogID = "END";
            Pause();
        }

        // text
        try
        {
            String label = dialog["text"] as String;
            DialogBasic dialogBasic = _DialogBasic.Instance() as DialogBasic;
            
            DialogContainer.AddChild( dialogBasic );
            // await ToSignal( dialogBasic, "ready" );
            dialogBasic.SetLabel( label );
        }
        catch( KeyNotFoundException ) { }

        // question
        try
        {
            String questionId = dialog["question"] as String;
            Godot.Collections.Dictionary question = Game.Scenario.GetQuestion( questionId );
            question["id"] = questionId;

            if ( Game.Save.GetSolvedQuestionsOfScenario( Game.Scenario.CurrentScenario ).Contains( questionId ) )
                question["previouslySolved"] = true;
            else
            {
                question["previouslySolved"] = false;
                Pause();
            }

            MCQuestion mcQuestion = _DialogMCQuestion.Instance() as MCQuestion;
            DialogContainer.AddChild( mcQuestion );
            mcQuestion.Initialize( question );

            if ( !(bool)question["previouslySolved"] )
                mcQuestion.Connect("AnswerSubmitted", this, nameof(AnswerSubmitted), null, 0);    
        }
        catch( KeyNotFoundException ) { }

        ScrollToBottom();
    }

    public void Pause()
    {
        IsDialogPaused = true;
        ContinueButton.Disabled = true;

        if ( NextDialogID == "END" )
            ContinueButton.Text = CONTINUE_END;
        else
            ContinueButton.Text = CONTINUE_PAUSE;
    }

    public void Resume()
    {
        IsDialogPaused = false;
        ContinueButton.Disabled = false;
        ContinueButton.Text = CONTINUE_CONTINUE;
    }

    public void Restart()
    {
        foreach( Node child in DialogContainer.GetChildren() )
            child.QueueFree();
        
        NextDialogID = Game.Scenario.GetStartID();
        IsDialogPaused = false;
    }

    private async void ScrollToBottom()
    {
        ScrollTimer.Start();
        await ToSignal( ScrollTimer, "timeout" );
        DialogScroll.ScrollVertical = (int)DialogScroll.GetVScrollbar().MaxValue;
    }

    private void AnswerSubmitted( MCQuestion question, bool solved )
    {
        ScrollToBottom();

        if ( solved )
        {
            Game.Save.UserSolvedQuestionOfScenario( Game.Scenario.CurrentScenario, question.QuestionID );
            Resume();
            question.Disconnect("AnswerSubmitted", this, nameof(AnswerSubmitted));
        }
    }

    public void _OnContinuePressed()
    {
        ProceedToNextDialog();
    }
}
