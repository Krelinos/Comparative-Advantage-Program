using ComparativeAdvantage;
using Godot;
using System;
using System.Collections.Generic;

public class Dialog : NinePatchRect
{
    [Signal]
    public delegate void DialogVisualsEvent( String visualsId );

    protected ScrollContainer DialogScroll;
    protected Control DialogContainer;
    protected Button ContinueButton;
    protected Timer ScrollTimer;

    protected String NextDialogID;
    protected bool IsDialogPaused;

    protected const String CONTINUE_CONTINUE = "Click here or press space to continue.";
    protected const String CONTINUE_CONTINUE_MOBILE = "Tap here to continue.";
    protected const String CONTINUE_PAUSE    = "Solve the above question to continue.";
    protected const String CONTINUE_END      = "- End of scenario -";

    protected PackedScene _DialogBasic;
    protected PackedScene _DialogMCQuestion;

    public override void _Ready()
    {
        DialogScroll = GetNode<ScrollContainer>("VBoxContainer/MarginContainer/ScrollContainer");
        DialogContainer = GetNode<Control>("VBoxContainer/MarginContainer/ScrollContainer/VBoxContainer");
        ContinueButton = GetNode<Button>("VBoxContainer/MarginContainer2/Continue");
        ScrollTimer = GetNode<Timer>("ScrollTimer");
        
        NextDialogID = "";
        IsDialogPaused = false;

        _DialogBasic = GD.Load<PackedScene>("res://Scenes/Dialog/Basic.tscn");
        _DialogMCQuestion = GD.Load<PackedScene>("res://Scenes/Dialog/MCQuestion.tscn");

        Main.Scenarios.Connect( nameof(Scenarios.ScenarioLoaded), this, nameof(OnScenarioLoaded) );

        Restart();
    }

    public void ProceedToNextDialog()
    {
        Godot.Collections.Dictionary dialog = Main.Scenarios.PopDialog();
        
        // flags
        // try
        // {
        //     NextDialogID = dialog["flags"] as String;
        // }
        // catch( KeyNotFoundException ) { }

        if ( dialog.Contains("flags") && dialog["flags"] is String flags )
        {
            var flagsList = flags.Split(' ');
            foreach ( String flag in flagsList )
                switch( flag )
                {
                    case "end":
                        Pause( true );
                        break;
                }
        }

        // text
        try
        {
            String label = dialog["text"] as String;
            DialogBasic dialogBasic = _DialogBasic.Instance() as DialogBasic;
            
            DialogContainer.AddChild( dialogBasic );
            // await ToSignal( dialogBasic, "ready" );
            dialogBasic.Text = label;
        }
        catch( KeyNotFoundException ) { }

        // question
        try
        {
            String questionId = dialog["question"] as String;
            Godot.Collections.Dictionary question = GameService.Scenario.GetQuestion( questionId );
            question["id"] = questionId;

            if ( GameService.Save.GetSolvedQuestionsOfScenario( GameService.Scenario.CurrentScenario ).Contains( questionId ) )
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

        // visuals
        try
        {
            String visualsId = dialog["visuals"] as String;
            EmitSignal(nameof(DialogVisualsEvent), visualsId);
        }
        catch( KeyNotFoundException ) { }

        // concept
        try
        {
            String conceptId = dialog["concept"] as String;
            GameService.DefinitionsList.AppendDefinition( conceptId );
        }
        catch( KeyNotFoundException ) { }

        ScrollToBottom();
    }

    public void Pause( bool isEndOfScenario = false )
    {
        IsDialogPaused = true;
        ContinueButton.Disabled = true;

        if ( isEndOfScenario )
            ContinueButton.Text = CONTINUE_END;
        else
            ContinueButton.Text = CONTINUE_PAUSE;
    }

    public void Resume()
    {
        IsDialogPaused = false;
        ContinueButton.Disabled = false;
        if ( Main.IsClientOnDesktop )
            ContinueButton.Text = CONTINUE_CONTINUE;
        else
            ContinueButton.Text = CONTINUE_CONTINUE_MOBILE;
    }

    public void Restart()
    {
        foreach( Node child in DialogContainer.GetChildren() )
            child.QueueFree();
        
        Resume();
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
            GameService.Save.UserSolvedQuestionOfScenario( GameService.Scenario.CurrentScenario, question.QuestionID );
            Resume();
            question.Disconnect("AnswerSubmitted", this, nameof(AnswerSubmitted));
        }
    }

    private void OnScenarioLoaded( String scenarioName )
    {
        Restart();
        ProceedToNextDialog();
    }

    public void _OnContinuePressed()
    {
        ProceedToNextDialog();
    }
}
