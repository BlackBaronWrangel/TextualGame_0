using GdProj.Services;
using GlobalServices.Entities;
using GlobalServices.Enums;
using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

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
        Action currentEventType = sp.StateMachine.CurrentState.BaseEvent.EventType switch
        {
            EventType.Default => ProcessDefaultEventsButtons,
            EventType.Custom => ProcessDefaultEventsButtons,
            EventType.Transition => ProcessDefaultEventsButtons,
            EventType.Confrontation => ProcessConfrontationEventsButtons,
            EventType.Ending => ProcessDefaultEventsButtons,
            EventType.Dialogue => ProcessDefaultEventsButtons,
            _ => null
        };
        currentEventType?.Invoke();

    }
    private void ProcessDefaultEventsButtons()
    {
        var possibleNextEvents = sp.StateMachine.CurrentState.BaseEvent.PossibleNextEvents;
        if (possibleNextEvents.Any(e => sp.EventService.GetEvent(e.Value).EventType == EventType.Confrontation)) //if next events contains confrontation
        {
            var nextEvents = possibleNextEvents;
            var nextEventConfrontationPair = nextEvents.FirstOrDefault(ne => sp.EventService.GetEvent(ne.Value).EventType == EventType.Confrontation);
            var button = CreateNextEventButton(nextEventConfrontationPair);
            AddChild(button);
        }
        else //default
        {
            var nextEvents = possibleNextEvents;
            foreach (var nextEventEntryPair in nextEvents)
            {
                var button = CreateNextEventButton(nextEventEntryPair);
                if (button is not null)
                    AddChild(button);
            }
        }

        var currentEventItems = sp.StateMachine.CurrentState.BaseEvent.ItemIds;
        foreach (var itemId in currentEventItems)
        {
            var button = CreateItemButton(itemId);
            if (button is not null)
                AddChild(button);
        }
    }
    private void ProcessConfrontationEventsButtons()
    {
        var nextEvents = sp.StateMachine.CurrentState.BaseEvent.PossibleNextEvents; //to exit from confrontation
        var isConfrontationActive = sp.StateMachine.CurrentState.SubStateMachine.Status is SubStateStatus.Running;

        if (isConfrontationActive)
        {
            var targets = sp.StateMachine.CurrentState.SubStateMachine.Characters;
            foreach (var trgt in targets)
            {
                var target = trgt as ConfrontationCharacter;
                if (target.Status is not CharacterConfrontationStatus.Yield)
                {
                    var button = CreateConfrontationIntentStrikeButton(target.BaseCharacter.Id);
                    if (button is not null)
                        AddChild(button);
                }
            }
        }
        else //confrontation ended
        {
            foreach (var nextEventEntryPair in nextEvents)
            {
                var button = CreateNextEventButton(nextEventEntryPair);
                if (button is not null)
                    AddChild(button);
            }

            var currentEventItems = sp.StateMachine.CurrentState.BaseEvent.ItemIds;
            foreach (var itemId in currentEventItems)
            {
                var button = CreateItemButton(itemId);
                if (button is not null)
                    AddChild(button);
            }
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
    private Button CreateItemButton(string itemId)
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
    private Button CreateConfrontationIntentStrikeButton(string targetId)
    {
        var target = sp.CharacterService.GetCharacter(targetId);

        var button = new Button();
        button.Text = $"Strike {target.BodyType} {target.Species} {target.Gender}";
        button.Pressed += () => ButtonStrikePressed(button, target);
        return button;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    private void ButtonEventPressed(string value)
    {
        sp.StateMachine.NextState(value);
    }
    private void ButtonItemPressed(Button button, Item item)
    {
        button.QueueFree();
        sp.StateMachine.CurrentState.BaseEvent.ItemIds.Remove(item.Id);
        _mainTextContent.UpdateText();

        item.Persistence = GlobalServices.Enums.ItemPersistence.Permanent;
        var mainCharacter = sp.CharacterService.GetPlayer();
        mainCharacter.Items.Add(item);
    }
    private void ButtonStrikePressed(Button button, Character target)
    {
        button.QueueFree();
        var playerConfrontationCharacter = sp.StateMachine.CurrentState.SubStateMachine.Characters.FirstOrDefault(c => c.BaseCharacter.Id == sp.CharacterService.GetPlayer().Id) as ConfrontationCharacter;
        playerConfrontationCharacter.Intent = new Intent()
        {
            IntentType = IntentType.Strike,
            Target = target.Id
        };
        _mainTextContent.UpdateText();
        sp.StateMachine.CurrentState.SubStateMachine.NextTurn();
    }
    private void CleanButtons()
    {
        foreach (Node child in GetChildren())
            if (child is Button)
                child.QueueFree();
    }
}
