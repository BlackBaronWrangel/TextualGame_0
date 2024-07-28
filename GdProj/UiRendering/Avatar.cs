using Godot;
using System;

public partial class Avatar : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var parent = GetParent() as Control;
        if (parent is not null)
        {
            Vector2 parentposition = parent.GlobalPosition;
            Vector2 parentSize = parent.Size;
            Vector2 newPosition = parentposition + (parentSize) / 2;
            this.Position = newPosition;
        }
    }
}
