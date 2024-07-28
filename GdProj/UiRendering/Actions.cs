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

        foreach (var nextEventId in nextEvents)
        {
            var nextEvent = sp.EventService.GetEvent(nextEventId);
            if (nextEvent is null)
            {
                sp.Logger.LogWarning("Can't read event for button.");
                continue;
            }
            var loc = sp.LocationService.GetLocation(nextEvent.LocationId);
            if (loc is null)
            {
                sp.Logger.LogWarning("Can't read location for button.");
                continue;
            }

            var button = new Button();

            if (string.IsNullOrEmpty(nextEvent.EntryDescription))
                button.Text = $"Generated button name"; //placeholder
            else
             button.Text = $"{nextEvent.EntryDescription}";

            button.Pressed += () => ButtonPressed(nextEvent.Id);
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
