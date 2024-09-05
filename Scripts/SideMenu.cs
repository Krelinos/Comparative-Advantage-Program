using Godot;
using System;

namespace ComparativeAdvantage { namespace mobile {

public class SideMenu : CanvasLayer
{
    [Export] protected NodePath __MenuButton;
    [Export] protected NodePath __Backdrop;
    [Export] protected NodePath __OffscreenWindow;
    [Export] protected NodePath __OnscreenWindow;
    [Export] protected NodePath __Menu;
    [Export] protected NodePath __BackButton;
    [Export] protected NodePath OtherSideMenu;
    [Export] protected readonly bool IsRightOriented;

    public Button MenuButton
    {
        get { return _MenuButton; }
        protected set { _MenuButton = value; }
    }
    
    public Button Backdrop
    {
        get { return _Backdrop; }
        protected set { _Backdrop = value; }
    }
    
    public Container OffscreenWindow
    {
        get { return _OffscreenWindow; }
        protected set { _OffscreenWindow = value; }
    }
    
    public Container OnscreenWindow
    {
        get { return _OnscreenWindow; }
        protected set { _OnscreenWindow = value; }
    }

    public Container Menu
    {
        get { return _Menu; }
        protected set { _Menu = value; }
    }

    public Button BackButton
    {
        get { return _BackButton; }
        protected set { _BackButton = value; }
    }

    private Button _MenuButton;
    private Button _Backdrop;
    private Container _OffscreenWindow;
    private Container _OnscreenWindow;
    private Container _Menu;
    private Button _BackButton;

    protected bool IsMenuOpen;

    protected const float OPEN_AND_CLOSE_DURATION = 0.25f;

    public override void _Ready()
    {
        MenuButton = GetNode<Button>( __MenuButton );
        Backdrop = GetNode<Button>( __Backdrop );
        OffscreenWindow = GetNode<Container>( __OffscreenWindow );
        OnscreenWindow = GetNode<Container>( __OnscreenWindow );
        Menu = GetNode<Container>( __Menu );
        BackButton = GetNode<Button>( __BackButton );

        MenuButton.Connect( "pressed", this, nameof(ToggleMenu) );
        Backdrop.Connect( "pressed", this, nameof(OnBackdropOrBackButtonPressed) );
        BackButton.Connect( "pressed", this, nameof(OnBackdropOrBackButtonPressed) );
    }

    protected void ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        var initial =  OffscreenWindow.RectSize.x * (IsRightOriented ? 1 : -1);

        if ( IsMenuOpen )
        {
            Tween tween = new Tween();
            tween.InterpolateProperty(this, "offset:x",
                initial,
                initial + Menu.RectSize.x * (IsRightOriented ? -1 : 1),
                OPEN_AND_CLOSE_DURATION,
                Tween.TransitionType.Quad,
                Tween.EaseType.Out );
            AddChild(tween);
            tween.Start();

            Tween tween2 = new Tween();
            tween2.InterpolateProperty(Backdrop, "modulate", new Color(1,1,1,0f), new Color(1,1,1,0.5f),
                OPEN_AND_CLOSE_DURATION,
                Tween.TransitionType.Quad,
                Tween.EaseType.Out );
            AddChild(tween2);
            tween2.Start();

            Backdrop.MouseFilter = Control.MouseFilterEnum.Stop;
        }
        else
        {
            Tween tween = new Tween();
            tween.InterpolateProperty(this, "offset:x",
                initial + Menu.RectSize.x * (IsRightOriented ? -1 : 1),
                initial,
                OPEN_AND_CLOSE_DURATION,
                Tween.TransitionType.Quad,
                Tween.EaseType.Out );
            AddChild(tween);
            tween.Start();

            Tween tween2 = new Tween();
            tween2.InterpolateProperty(Backdrop, "modulate", new Color(1,1,1,0.5f), new Color(1,1,1,0f), OPEN_AND_CLOSE_DURATION, Tween.TransitionType.Quad, Tween.EaseType.Out );
            AddChild(tween2);
            tween2.Start();

            Backdrop.MouseFilter = Control.MouseFilterEnum.Ignore;
        }
    }

    protected void OnBackdropOrBackButtonPressed()
    {
        if ( IsMenuOpen )
            ToggleMenu();
    }
}

} }