using GdProj.Services;
using GdProj.UiRendering.Enums;
using Godot;
using System;

public partial class GlobalStateController : Node
{
    private readonly string _tabMenuName = "%TabMenu";
    private readonly string _startButtonName = "%StartButton";
    private readonly int _gameplayTabIndex = 1;

    private TabMenu _tabMenu;
    private StartButton _startButton;

    public GlobalGameState GameState { get; protected set; }

    public override void _Ready()
    {
        _tabMenu = GetNode<TabMenu>(_tabMenuName);
        _startButton = GetNode<StartButton>(_startButtonName);

        InitDefaultGameState();
    }
    private void InitDefaultGameState()
    {
        GameState = GlobalGameState.GameNotStarted;
        _tabMenu.SetTabDisabled(_gameplayTabIndex, true);
    }
    public void StartGame()
    {
        GameServiceProvider.Instance.StateMachine.RunScene("TestingScene_0");
        _tabMenu.SetTabDisabled(_gameplayTabIndex, false);
        _tabMenu.CurrentTab = _gameplayTabIndex;
        _startButton.Visible = false;
        GameState = GlobalGameState.GameStarted;
    }
    public void LoadGame()
    {
        throw new NotImplementedException();
    }
}
