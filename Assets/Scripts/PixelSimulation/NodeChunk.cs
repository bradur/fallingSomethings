using UnityEngine;
using System.Collections;

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
            CalculateGravity(node);
        }
        foreach (Node node in Pixels)
        {
            ApplyGravity(node);
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

    public void ApplyGravity(Node node)
    {
        if (node.NextX < 0 || node.NextY < 0)
        {
            return;
        }
        Pixels.MoveToNext(node);
    }

    public bool CalculateGravity(Node node)
    {
        if (node.Type == NodeType.Sand)
        {
            return CalculateGravityForSand(node);
        }
        else if (node.Type == NodeType.Water)
        {
            return CalculateGravityForWater(node);
        }
        return false;
    }

    private bool CalculateGravityForWater(Node node)
    {
        Node southNeighbor = GetNeighbor(node, -1, 0);
        if (WaterCanPassThrough(southNeighbor))
        {
            node.QueueMove(southNeighbor);
            return true;
        }
        int firstDir = node.FlowDirection;
        Node firstSouthNeighbor = GetNeighbor(node, -1, firstDir);
        if (WaterCanPassThrough(firstSouthNeighbor))
        {
            node.QueueMove(firstSouthNeighbor);
            return true;
        }
        int secondDir = firstDir * -1;
        Node secondSouthNeighbor = GetNeighbor(node, -1, secondDir);
        if (WaterCanPassThrough(secondSouthNeighbor))
        {
            node.QueueMove(secondSouthNeighbor);
            return true;
        }
        Node firstHorizontalNeighbor = GetNeighbor(node, 0, firstDir);
        if (WaterCanPassThrough(firstHorizontalNeighbor))
        {
            node.QueueMove(firstHorizontalNeighbor);
            return true;
        }
        node.ChangeDirection();
        /*Node secondHorizontalNeighbor = GetNeighbor(node, 0, secondDir);
        if (WaterCanPassThrough(secondHorizontalNeighbor))
        {
            node.QueueMove(secondHorizontalNeighbor);
            return true;
        }*/
        return false;
    }

    private bool CalculateGravityForSand(Node node)
    {
        Node southNeighbor = GetNeighbor(node, -1, 0);
        if (SandCanPassThrough(southNeighbor))
        {
            node.QueueMove(southNeighbor);
            return true;
        }
        int nextX = GameManager.main.RandomChoice(-1, 1);
        Node nextNeighbor = GetNeighbor(node, -1, nextX);
        if (SandCanPassThrough(nextNeighbor))
        {
            node.QueueMove(nextNeighbor);
            return true;
        }
        int lastX = nextX == -1 ? 1 : -1;
        Node lastNeighbor = GetNeighbor(node, -1, lastX);
        if (SandCanPassThrough(lastNeighbor))
        {
            node.QueueMove(lastNeighbor);
            return true;
        }
        return false;
    }

    private bool WaterCanPassThrough(Node node)
    {
        return node != null && !node.IsQueueTarget && (
            node.IsEmpty()
        );
    }

    private bool SandCanPassThrough(Node node)
    {
        return node != null && (node.IsEmpty() || node.IsWater());
    }

    public NodeChunk(int nodeWidth, int nodeHeight, NodeType nodeType)
    {
        width = nodeWidth;
        height = nodeHeight;
        colors = new Color[width * height];
        Pixels = new NodeArray(width, height, nodeType);
    }
}