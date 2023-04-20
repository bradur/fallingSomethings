
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeArray : IEnumerable<Node>
{
    private int width;
    private int height;
    private Node[,] array;
    public NodeArray(int nWidth, int nHeight, BrushConfig config = null)
    {
        width = nWidth;
        height = nHeight;
        array = new Node[height, width];
        for (int y = 0; y < height; y += 1)
        {
            for (int x = 0; x < width; x += 1)
            {
                if (config != null)
                {
                    bool isEmpty = UnityEngine.Random.Range(0f, 1f) > 0.5f;
                    array[y, x] = isEmpty ? Node.NewNode(x, y) : Node.NewNode(x, y, config);
                }
                else
                {
                    array[y, x] = Node.NewNode(x, y);
                }
            }
        }
    }

    public IEnumerator<Node> GetEnumerator()
    {
        /*
        for (int x = 0; x < width; x += 1)
        {
            for (int y = height - 1; y >= 0; y -= 1)
            {
                yield return array[y, x];
            }
        }
        */
        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                yield return array[y, x];
            }
        }

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public Color[] GetColors()
    {
        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y += 1)
        {
            for (int x = 0; x < width; x += 1)
            {
                colors[y * width + x] = array[y, x].Color;
            }
        }
        return colors;
    }

    public void MoveToNext(Node node)
    {
        Node oldNode = array[node.NextY, node.NextX];
        if (node.Type == NodeType.Sand && oldNode.Type == NodeType.Water)
        {
            oldNode.NextX = -1;
            oldNode.NextY = -1;
            oldNode.X = node.X;
            oldNode.Y = node.Y;
            oldNode.IsQueueTarget = false;
            array[node.Y, node.X] = oldNode;
        }
        else
        {
            array[node.Y, node.X] = Node.NewNode(node.X, node.Y);
        }
        node.IsQueueTarget = false;
        array[node.NextY, node.NextX] = node;
        node.X = node.NextX;
        node.Y = node.NextY;
        node.NextX = -1;
        node.NextY = -1;
    }

    public Node Get(int x, int y)
    {
        if (y >= height || y < 0 || x >= width || x < 0)
        {
            return null;
        }
        return array[y, x];
    }

    public void Set(int x, int y, Node node)
    {
        if (y >= height || y < 0 || x >= width || x < 0)
        {
            return;
        }
        array[y, x] = node;
    }
    public void SetIfEmpty(int x, int y, Node node)
    {
        Node oldNode = Get(x, y);
        if (oldNode != null && oldNode.IsEmpty())
        {
            Set(x, y, node);
        }
    }
}