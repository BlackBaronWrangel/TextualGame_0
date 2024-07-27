using GdProj.Services;
using GlobalServices;
using GlobalServices.Interfaces;
using Godot;
using System;

public partial class Actions : HFlowContainer
{
    GameServiceProvider sp;
    public override void _Ready()
    {
        sp = GameServiceProvider.Instance;
        sp.StateMachine.StateChanged += (sender, e) => UpdateButtons();
    }

    public void UpdateButtons()
    {
        var nextEvents = sp.StateMachine.CurrentState.PossibleNextEvents;

        foreach (var nextEvent in nextEvents)
        {
            var gameEvent = sp.EventService.GetEvent(nextEvent);
            if (gameEvent is null)
            {
                sp.Logger.LogWarning("Can't read event for preparing button.");
                continue;
            }
            var loc = sp.LocationService.GetLocation(gameEvent.LocationId);
            if (loc is null)
            {
                sp.Logger.LogWarning("Can't read location for preparing button.");
                continue;
            }

            var button = new Button();
            button.Text = $"Go to {loc.Name}";

            button.Pressed += () => ButtonPressed(gameEvent.LocationId);
            AddChild(button);
        }
    }

    private void ButtonPressed(String value)
    {
        CleanButtons();
        sp.StateMachine.NextState(value);
    }

    private void CleanButtons()
    {
        foreach (Node child in GetChildren())
            if (child is Button)
                child.QueueFree();
    }
}
