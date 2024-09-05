using ComparativeAdvantage;
using Godot;
using System;

namespace ComparativeAdvantage { namespace mobile {

public class TermDetails : NinePatchRect
{
    [Export] protected NodePath __BackButton;
    [Export] protected NodePath __Backdrop;
    [Export] protected NodePath __TermName;
    [Export] protected NodePath __TermDescription;

    public Button BackButton
    {
        get { return _BackButton; }
        protected set { _BackButton = value; }
    }
    
    public Button Backdrop
    {
        get { return _Backdrop; }
        protected set { _Backdrop = value; }
    }
    
    public Label TermName
    {
        get { return _TermName; }
        protected set { _TermName = value; }
    }
    
    public RichTextLabel TermDescription
    {
        get { return _TermDescription; }
        protected set { _TermDescription = value; }
    }

    private Button _BackButton;
    private Button _Backdrop;
    private Label _TermName;
    private RichTextLabel _TermDescription;

    protected bool IsOpen;
    
    protected const float OPEN_AND_CLOSE_DURATION = 0.25f;

    public override void _Ready()
    {
        BackButton = GetNode<Button>( __BackButton );
        Backdrop = GetNode<Button>( __Backdrop );
        TermName = GetNode<Label>( __TermName );
        TermDescription = GetNode<RichTextLabel>( __TermDescription );

        BackButton.Connect( "pressed", this, nameof(CloseMenu) );
        Backdrop.Connect( "pressed", this, nameof(CloseMenu) );
    }

    public void DisplayDetailsForTerm( String term )
    {
        GD.Print("yeet");
        TermName.Text = term.Capitalize();
        TermDescription.BbcodeText = Main.Glossary.TermDescriptions[ term ] as String;
        OpenMenu();
    }

    protected void OpenMenu()
    {
        Tween tween = new Tween();
            tween.InterpolateProperty(this, "rect_position:x",
                RectSize.x,
                0,
                OPEN_AND_CLOSE_DURATION,
                Tween.TransitionType.Quad,
                Tween.EaseType.Out );
            AddChild(tween);
            tween.Start();
        
        Backdrop.MouseFilter = MouseFilterEnum.Stop;
    }

    protected void CloseMenu()
    {
        Tween tween = new Tween();
            tween.InterpolateProperty(this, "rect_position:x",
                0,
                RectSize.x,
                OPEN_AND_CLOSE_DURATION,
                Tween.TransitionType.Quad,
                Tween.EaseType.Out );
            AddChild(tween);
            tween.Start();
        
        Backdrop.MouseFilter = MouseFilterEnum.Ignore;
    }
}

} // namespace mobile
} // namespace ComparativeAdvantage