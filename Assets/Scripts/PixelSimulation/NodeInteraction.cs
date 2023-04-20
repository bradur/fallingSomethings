public static class NodeInteraction
{
    public static void PerformMove(NodeArray array, Node node)
    {
        Node oldNode = array.Get(node.NextX, node.NextY);
        if (node.Type == NodeType.Sand && oldNode.Type == NodeType.Water)
        {
            oldNode.NextX = -1;
            oldNode.NextY = -1;
            oldNode.X = node.X;
            oldNode.Y = node.Y;
            oldNode.IsQueueTarget = false;
            array.Set(node.X, node.Y, oldNode);
        }
        else
        {
            array.Set(node.X, node.Y, Node.NewNode(node.X, node.Y));
        }
        node.IsQueueTarget = false;
        array.Set(node.NextX, node.NextY, node);
        node.X = node.NextX;
        node.Y = node.NextY;
        node.NextX = -1;
        node.NextY = -1;
    }

    public static bool Determine(Node node, NodeChunk chunk)
    {
        if (node.Type == NodeType.Sand)
        {
            return CalculateInteractionForSand(node, chunk);
        }
        else if (node.Type == NodeType.Water)
        {
            return CalculateInteractionForWater(node, chunk);
        }
        else if (node.Type == NodeType.Fire)
        {
            return CalculateInteractionForFire(node, chunk);
        }
        return false;
    }

    private static bool CalculateInteractionForWater(Node node, NodeChunk chunk)
    {
        Node southNeighbor = chunk.GetNeighbor(node, -1, 0);
        if (WaterCanPassThrough(southNeighbor))
        {
            node.QueueMove(southNeighbor);
            return true;
        }

        int diagonalDir = node.FlowDirection;
        Node diagonalNeighbor = chunk.GetNeighbor(node, -1, diagonalDir);
        if (WaterCanPassThrough(diagonalNeighbor))
        {
            node.QueueMove(diagonalNeighbor);
            return true;
        }
        node.ChangeDirection();

        int horizDirection = node.HorizontalFlowDirection;
        Node horizontalNeighbor = chunk.GetNeighbor(node, 0, horizDirection);
        if (WaterCanPassThrough(horizontalNeighbor))
        {
            node.QueueMove(horizontalNeighbor);
            return true;
        }
        node.ChangeHorizontalDirection();

        return false;
    }

    private static bool CalculateInteractionForSand(Node node, NodeChunk chunk)
    {
        Node southNeighbor = chunk.GetNeighbor(node, -1, 0);
        if (SandCanPassThrough(southNeighbor))
        {
            node.QueueMove(southNeighbor);
            return true;
        }
        int nextX = GameManager.main.RandomChoice(-1, 1);
        Node nextNeighbor = chunk.GetNeighbor(node, -1, nextX);
        if (SandCanPassThrough(nextNeighbor))
        {
            node.QueueMove(nextNeighbor);
            return true;
        }
        int lastX = nextX == -1 ? 1 : -1;
        Node lastNeighbor = chunk.GetNeighbor(node, -1, lastX);
        if (SandCanPassThrough(lastNeighbor))
        {
            node.QueueMove(lastNeighbor);
            return true;
        }
        return false;
    }

    private static bool CalculateInteractionForFire(Node node, NodeChunk chunk)
    {
        Node southNeighbor = chunk.GetNeighbor(node, -1, 0);
        if (FireCanPassThrough(southNeighbor))
        {
            node.QueueMove(southNeighbor);
            return true;
        }
        return false;
    }

    private static bool WaterCanPassThrough(Node node)
    {
        return node != null && !node.IsQueueTarget && node.IsEmpty();
    }

    private static bool SandCanPassThrough(Node node)
    {
        return node != null && (node.IsEmpty() || node.IsWater());
    }

    private static bool FireCanPassThrough(Node node)
    {
        return node != null && !node.IsQueueTarget && node.IsEmpty();
    }
}