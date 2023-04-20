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
    public int HorizontalFlowDirection = -1;
    public int FlowDirection = -1;

    public Node(int x, int y, Color nodeColor, NodeType nodeType)
    {
        NextX = -1;
        NextY = -1;
        X = x;
        Y = y;
        Color = nodeColor;
        Type = nodeType;
        HorizontalFlowDirection = GameManager.main.RandomChoice(-1, 1);
        FlowDirection = GameManager.main.RandomChoice(-1, 1);
    }

    public void ChangeDirection()
    {
        FlowDirection *= -1;
    }
    public void ChangeHorizontalDirection()
    {
        HorizontalFlowDirection *= -1;
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

    public static Node NewNode(int x, int y, BrushConfig config = null)
    {
        if (config != null)
        {
            return new Node(x, y, config.RandomBrushColor(), config.Type);
        }
        return new Node(x, y, Color.clear, NodeType.Empty);
    }

    public override string ToString()
    {
        return $"[X: {X}][Y: {Y}] & [NextX: {NextX}][NextY: {NextY}]\n[Type: {Type}]\n[IsQueueTarget: {IsQueueTarget}]\n[HorizontalFlowDirection: {HorizontalFlowDirection}]\n[FlowDirection: {FlowDirection}]";
    }
}

public enum NodeType
{
    Empty,
    Sand,
    Water,
    Wood,
    Fire
}