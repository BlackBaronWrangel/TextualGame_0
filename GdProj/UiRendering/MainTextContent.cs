using GdProj.Services;
using GlobalServices.Entities;
using GlobalServices.Enums;
using Godot;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public partial class MainTextContent : RichTextLabel
{
    GameServiceProvider sp;
    public override void _Ready()
    {
        sp = GameServiceProvider.Instance;
        sp.StateMachine.StateChanged += (sender, e) => UpdateText();
        this.MetaClicked += (content) => LabelClicked(content.Obj.ToString());
    }
    public void UpdateText()
    {
        var text = string.Empty;
        var currentGameEvent = sp.StateMachine.CurrentState.BaseEvent;

        var locId = currentGameEvent.LocationId;
        var currentLocation = sp.LocationService.GetLocation(locId);
        if (currentLocation is null)
        {
            sp.Logger.LogWarning($"Can't read location for preparing display text content. Location:{locId}");
            return;
        }

        var locationName = currentLocation.Name;
        var locationDescription = currentLocation.Description;

        text += $"[b]{locationName}[/b]\n\n{locationDescription}\n";

        Func<string> currentEventType = currentGameEvent.EventType switch
        {
            EventType.Default => ProcessDefaultEventsText,
            EventType.Custom => ProcessDefaultEventsText,
            EventType.Transition => ProcessDefaultEventsText,
            EventType.Confrontation => ProcessConfrontationEventsText,
            EventType.Ending => ProcessDefaultEventsText,
            EventType.Dialogue => ProcessDefaultEventsText,
            _ => ProcessDefaultEventsText
        };

        var sceneDescription = currentEventType?.Invoke();
        text += $"\n{sceneDescription}\n";

        Text = text;
    }

    private string ProcessDefaultEventsText()
    {
        var currentGameEvent = sp.StateMachine.CurrentState.BaseEvent;
        var text = currentGameEvent.EventeDescription;

        var characters = sp.CharacterService.Characters.Where(c => currentGameEvent.CharacterIds.Contains(c.Id)).ToList();
        foreach (var character in characters)
            text += $"\nYou see a [url={character.Id}]{character.Gender} {character.BodyType} {character.Species}[/url] that looks like [i]{character.Type}[/i]";

        var itemIds = sp.StateMachine.CurrentState.BaseEvent.ItemIds;
        foreach (var itemid in itemIds)
        {
            var item = sp.ItemService.GetItem(itemid);
            if (item is not null)
            {
                text += $"\nYou see something that looks like {item.Type}. It is [i]{item.Name}[/i]";
            }
        }
        return text;
    }
    private string ProcessConfrontationEventsText()
    {
        var currentGameEvent = sp.StateMachine.CurrentState.BaseEvent;

        var text = "Confrontation in progress";
        var characters = sp.StateMachine.CurrentState.SubStateMachine.Characters;
        foreach (var character in characters)
        {
            if (character.BaseCharacter.ControlType == CharacterControlType.Player)
            { 
                text += $"\n[url={character.BaseCharacter.Id}]You[/url] are under attack"; 
            }
            else
            {
                if (character.Intent is null) 
                    text += $"\n[url={character.BaseCharacter.Id}]{character.BaseCharacter.Gender} {character.BaseCharacter.BodyType} {character.BaseCharacter.Species}[/url] prepares for battle";
                else 
                    text += $"\n[url={character.BaseCharacter.Id}]{character.BaseCharacter.Gender} {character.BaseCharacter.BodyType} {character.BaseCharacter.Species}[/url] is going to [i]{character.Intent}[/i]";
            }
        }
        foreach (var log in sp.StateMachine.CurrentState.SubStateMachine.SsmLogs) { text += log; }
        return text;
    }

    private void LabelClicked(string input)
    {
    }


}
