using Godot;
using System;

public partial class HUiSplit : HSplitContainer
{
    [Export]
    public double SplitRatio { get; set; } = 0.5;

    public override void _Ready()
    {
        base._Ready();
        Dragged += (long offset) => _on_Container_Dragged(offset);
        Resized += () => _on_Container_Resized();
    }

    public override void _Draw()
    {
        base._Draw();
        SetSplitRatio(SplitRatio);
    }

    private void _on_Container_Dragged(long offset)
    {
        // Calculate the new ratio based on the offset and container size
        double totalHeight = Size.X;
        double newRatio = (offset - ((Control)GetChild(0)).GetMinimumSize().X) / (totalHeight - ((Control)GetChild(0)).GetMinimumSize().X - ((Control)GetChild(1)).GetMinimumSize().X);
        SplitRatio = Mathf.Clamp((float)newRatio, 0.0f, 1.0f);
    }

    private void _on_Container_Resized()
    {
        // Reapply the stored ratio when the container size changes
        SetSplitRatio(SplitRatio);
    }

    private void SetSplitRatio(double ratio)
    {
        ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

        // Get the minimum sizes of the children
        var minSizeFirst = ((Control)GetChild(0)).GetMinimumSize().X;
        var minSizeSecond = ((Control)GetChild(1)).GetMinimumSize().X;

        // Calculate the total usable size minus the minimum size required for both children
        var totalUsableSize = Size.X - minSizeFirst - minSizeSecond;
        int offset = (int)(minSizeFirst + totalUsableSize * ratio);

        // Set the split offset
        SplitOffset = offset;
    }
}
