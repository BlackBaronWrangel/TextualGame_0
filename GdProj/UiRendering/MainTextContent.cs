using GdProj.Services;
using Godot;
using System;
using System.Linq;

public partial class MainTextContent : RichTextLabel
{
    // Called when the node enters the scene tree for the first time.
    GameServiceProvider sp;
    public override void _Ready()
    {
        sp = GameServiceProvider.Instance;
        sp.StateMachine.StateChanged += (sender, e) => UpdateText();
    }
    public void UpdateText()
    {
        var text = string.Empty;
        var currentGameEvent = sp.StateMachine.CurrentState;

        var locId = sp.StateMachine.CurrentState.LocationId;
        var currentLocation =sp.LocationService.GetLocation(locId);
        if (currentLocation is null)
        {
            sp.Logger.LogWarning($"Can't read location for preparing display text content. Location:{locId}");
            return;
        }

        var locationName = currentLocation.Name;
        var locationDescription = currentLocation.Description;
        var sceneDescription = currentGameEvent.EventeDescription;

        text += $"[b]{locationName}[/b]\n\n{locationDescription}\n";
        text += $"\n{sceneDescription}\n";


        var characters = sp.CharacterService.Characters.Where(c => currentGameEvent.CharacterIds.Contains(c.Id)).ToList();
        foreach ( var character in characters ) text += $"\nYou see a {character.Gender} {character.BodyType} {character.Species} that looks like [i]{character.Type}[/i]";


        Text = text;
    }
}
