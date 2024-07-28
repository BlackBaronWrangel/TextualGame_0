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

        foreach (var nextEventEntryPair in nextEvents)
        {
            var nextEvent = sp.EventService.GetEvent(nextEventEntryPair.Value);
            if (nextEvent is null)
            {
                sp.Logger.LogWarning($"Can't read event for button. Event: {nextEventEntryPair.Value}");
                continue;
            }
            var loc = sp.LocationService.GetLocation(nextEvent.LocationId);
            if (loc is null)
            {
                sp.Logger.LogWarning($"Can't read location for button. Location: {nextEvent.LocationId}");
                continue;
            }

            var button = new Button();

            if (string.IsNullOrEmpty(nextEventEntryPair.Key))
            {
                sp.Logger.LogError($"Can't assign text for button, because it is null or emnpty.");
                button.Text = string.Empty;
            }
            else
                button.Text = $"{nextEventEntryPair.Key}";

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
