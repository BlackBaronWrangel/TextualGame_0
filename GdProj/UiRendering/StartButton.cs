using GdProj.Services;
using Godot;
using System;

public partial class StartButton : Button
{
	private string _stateControllerName = "%GlobalStateController";

	public override void _Ready()
	{
		this.Pressed += () => StartGame();
	}
	private void StartGame() => GetNode<GlobalStateController>(_stateControllerName).StartGame();
}
