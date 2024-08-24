using Godot;
using System;

namespace ComparativeAdvantage {

public class SideMenu : CanvasLayer
{
    [Signal] protected delegate void OffscreenWindowVisibilityChanged( bool IsVisible );

    [Export] protected NodePath __MenuButton;
    [Export] protected NodePath __Backdrop;
    [Export] protected NodePath __OffscreenWindow;
    [Export] protected NodePath __OnscreenWindow;
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

    private Button _MenuButton;
    private Button _Backdrop;
    private Container _OffscreenWindow;
    private Container _OnscreenWindow;

    protected bool IsMenuOpen;

    public override void _Ready()
    {
        MenuButton = GetNode<Button>( __MenuButton );
        Backdrop = GetNode<Button>( __Backdrop );
        OffscreenWindow = GetNode<Container>( __OffscreenWindow );
        OnscreenWindow = GetNode<Container>( __OnscreenWindow );

        MenuButton.Connect( "pressed", this, nameof(ToggleMenu) );
        Backdrop.Connect( "pressed", this, nameof(OnBackdropPressed) );
        GetNode<SideMenu>( OtherSideMenu ).Connect( "OffscreenWindowVisibilityChanged", this, nameof(OnOtherMenuOffscreenWindowVisibilityChanged) );
    }

    protected async void ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        EmitSignal( nameof(OffscreenWindowVisibilityChanged), IsMenuOpen );

        if ( IsMenuOpen )
        {
            Tween tween = new Tween();
            tween.InterpolateProperty(this, "offset:x", OffscreenWindow.RectSize.x * (IsRightOriented ? 1 : -1), 0, 1f, Tween.TransitionType.Quad, Tween.EaseType.Out );
            AddChild(tween);
            tween.Start();

            Tween tween2 = new Tween();
            tween2.InterpolateProperty(Backdrop, "modulate", new Color(1,1,1,0f), new Color(1,1,1,0.5f), 1f, Tween.TransitionType.Quad, Tween.EaseType.Out );
            AddChild(tween2);
            tween2.Start();

            Backdrop.MouseFilter = Control.MouseFilterEnum.Stop;
            Layer++;
        }
        else
        {
            Tween tween = new Tween();
            tween.InterpolateProperty(this, "offset:x", 0, OffscreenWindow.RectSize.x * (IsRightOriented ? 1 : -1), 1f, Tween.TransitionType.Quad, Tween.EaseType.Out );
            AddChild(tween);
            tween.Start();

            Tween tween2 = new Tween();
            tween2.InterpolateProperty(Backdrop, "modulate", new Color(1,1,1,0.5f), new Color(1,1,1,0f), 1f, Tween.TransitionType.Quad, Tween.EaseType.Out );
            AddChild(tween2);
            tween2.Start();

            Backdrop.MouseFilter = Control.MouseFilterEnum.Ignore;
            await ToSignal( tween, "tween_completed" );
            Layer--;
        }
    }

    protected void OnOtherMenuOffscreenWindowVisibilityChanged( bool IsVisible )
    {
        Visible = !IsVisible;
        // OnscreenWindow.MouseFilter = IsVisible ? Control.MouseFilterEnum.Ignore : Control.MouseFilterEnum.Stop;
    }

    protected void OnBackdropPressed()
    {
        if ( IsMenuOpen )
            ToggleMenu();
    }
}

}