using UnityEngine;

public class NodeChunk
{
    private int width;
    private int height;
    private NodeArray Pixels;
    Color[] colors;

    public void Calculate()
    {
        foreach (Node node in Pixels)
        {
            NodeInteraction.Determine(node, this);
        }
        foreach (Node node in Pixels)
        {
            ApplyInteraction(node);
        }
    }

    public void SetNode(int x, int y, Node node)
    {
        Pixels.Set(x, y, node);
    }

    public void Render(Texture2D texture)
    {
        texture.SetPixels(0, 0, width, height, Pixels.GetColors());
        texture.Apply(false);
    }

    public Node GetNode(int x, int y)
    {
        return Pixels.Get(x, y);
    }

    public Node GetNeighbor(Node node, int yOffset, int xOffset)
    {
        return GetNode(node.X + xOffset, node.Y + yOffset);
    }

    public void ApplyInteraction(Node node)
    {
        if (node.NextX < 0 || node.NextY < 0)
        {
            return;
        }
        Pixels.MoveToNext(node);
    }


    public NodeChunk(int nodeWidth, int nodeHeight, BrushConfig config = null)
    {
        width = nodeWidth;
        height = nodeHeight;
        colors = new Color[width * height];
        Pixels = new NodeArray(width, height, config);
    }
}