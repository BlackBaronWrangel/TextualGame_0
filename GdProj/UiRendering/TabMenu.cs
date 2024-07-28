using Godot;
using System;

public partial class TabMenu : TabContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentTab = 0; //default menu
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
