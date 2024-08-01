using GdProj.Services;
using GlobalServices;
using GlobalServices.Entities;
using Godot;
using System;
using System.Collections.Generic;

public partial class Actions : HFlowContainer
{
    private readonly string _mainTextContentName = "%MainTextContent";
    private MainTextContent _mainTextContent;

    GameServiceProvider sp;
    public override void _Ready()
    {
        _mainTextContent = GetNode<MainTextContent>(_mainTextContentName);
        sp = GameServiceProvider.Instance;
        sp.StateMachine.StateChanged += (sender, e) => UpdateButtons();
    }

    public void UpdateButtons()
    {
        CleanButtons();
        var nextEvents = sp.StateMachine.CurrentState.PossibleNextEvents;
        foreach (var nextEventEntryPair in nextEvents)
        {
            var button = CreateNextEventButton(nextEventEntryPair);
            if (button is not null)
                AddChild(button);
        }

        var currentEventItems = sp.StateMachine.CurrentState.ItemIds;
        foreach (var itemId in currentEventItems)
        {
            var button = CreateItemButtons(itemId);
            if (button is not null)
                AddChild(button);
        }
    }

    private Button CreateNextEventButton(KeyValuePair<string, string> nextEventEntryPair)
    {
        var nextEvent = sp.EventService.GetEvent(nextEventEntryPair.Value);
        if (nextEvent is null)
        {
            sp.Logger.LogWarning($"Can't read event for button. Event: {nextEventEntryPair.Value}");
            return null;
        }
        var loc = sp.LocationService.GetLocation(nextEvent.LocationId);
        if (loc is null)
        {
            sp.Logger.LogWarning($"Can't read location for button. Location: {nextEvent.LocationId}");
            return null;
        }

        var button = new Button();

        if (string.IsNullOrEmpty(nextEventEntryPair.Key))
        {
            sp.Logger.LogError($"Can't assign text for button, because it is null or emnpty.");
            button.Text = string.Empty;
        }
        else
            button.Text = $"{nextEventEntryPair.Key}";

        button.Pressed += () => ButtonEventPressed(nextEvent.Id);

        return button;
    }
    private Button CreateItemButtons(string itemId)
    {
        var item = sp.ItemService.GetItem(itemId);
        if (item is null)
        {
            sp.Logger.LogWarning($"Can't read item for button. item: {itemId}");
            return null;
        }
        var button = new Button();
        button.Text = $"Take {item.Name}";
        button.Pressed += () => ButtonItemPressed(button, item);
        return button;
    }
    private void ButtonEventPressed(string value)
    {
        sp.StateMachine.NextState(value);
    }
    private void ButtonItemPressed(Button button, Item item)
    {
        button.QueueFree();
        sp.StateMachine.CurrentState.ItemIds.Remove(item.Id);
        _mainTextContent.UpdateText();

        item.Persistence = GlobalServices.Enums.ItemPersistence.Permanent;
        var mainCharacter = sp.CharacterService.GetPlayer();
        mainCharacter.Items.Add(item);
    }
    private void CleanButtons()
    {
        foreach (Node child in GetChildren())
            if (child is Button)
                child.QueueFree();
    }
}
