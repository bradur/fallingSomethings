using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int NextX;
    public int NextY;
    public Color Color;
    public int X;
    public int Y;
    public NodeType Type;
    public bool IsQueueTarget = false;
    public int FlowDirection = -1;

    public Node(int x, int y, Color nodeColor, NodeType nodeType)
    {
        NextX = -1;
        NextY = -1;
        X = x;
        Y = y;
        Color = nodeColor;
        Type = nodeType;
        FlowDirection = GameManager.main.RandomChoice(-1, 1);
    }

    public void ChangeDirection()
    {
        FlowDirection *= -1;
    }

    public void QueueMove(Node node)
    {
        NextX = node.X;
        NextY = node.Y;
        node.IsQueueTarget = true;
    }

    public bool IsEmpty()
    {
        return Type == NodeType.Empty;
    }

    public bool IsWater()
    {
        return Type == NodeType.Water;
    }

    public static Node NewNode(int x, int y, NodeType nodeType)
    {
        if (nodeType == NodeType.Sand)
        {
            return Sand(x, y);
        }
        else if (nodeType == NodeType.Empty)
        {
            return Empty(x, y);
        }
        else if (nodeType == NodeType.Water)
        {
            return Water(x, y);
        }
        else if (nodeType == NodeType.Wood)
        {
            return Wood(x, y);
        }
        return null;
    }

    private static Node Sand(int x, int y)
    {
        return new Node(x, y, GameManager.main.RandomBrushColor(NodeType.Sand), NodeType.Sand);
    }
    private static Node Empty(int x, int y)
    {
        return new Node(x, y, Color.clear, NodeType.Empty);
    }
    private static Node Water(int x, int y)
    {
        return new Node(x, y, GameManager.main.RandomBrushColor(NodeType.Water), NodeType.Water);
    }

    private static Node Wood(int x, int y)
    {
        return new Node(x, y, GameManager.main.RandomBrushColor(NodeType.Wood), NodeType.Wood);
    }
}

public enum NodeType
{
    Empty,
    Sand,
    Water,
    Wood
}