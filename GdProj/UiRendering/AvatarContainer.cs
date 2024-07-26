using Godot;
using System;
using System.Linq;

public partial class AvatarContainer : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        // Get all children nodes, assuming they are Control nodes (change type as necessary)
        var avatar = GetChildren().FirstOrDefault();
        var children = avatar.GetChildren().Cast<AnimatedSprite2D>().ToList();

        float maxX = 0;
        float maxY = 0;

        foreach (var child in children)
        {
            if (child != null)
            {
                var x = child.SpriteFrames.GetFrameTexture("default", 0).GetWidth() * child.Scale.X;
                var y = child.SpriteFrames.GetFrameTexture("default", 0).GetHeight() * child.Scale.Y;

                if (x > maxX)
                    maxX = x;

                if (y > maxY)
                    maxY = y;
            }
        }

        CustomMinimumSize = new Vector2(maxX, maxY);
    }
}
