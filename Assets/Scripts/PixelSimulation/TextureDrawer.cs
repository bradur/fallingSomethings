using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextureDrawer : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private GameObject renderedObject;

    private float fps = 15;

    private int width = 64;

    private int height = 36;
    private List<NodeChunk> chunks;

    private Texture2D texture;
    private float timer = 0f;
    private float interval = 1.0f;

    private int drawRadius = 1;

    private NodeType brushType = NodeType.Sand;

    [SerializeField]
    private LayerMask drawLayer;

    void Start()
    {
        Rebuild();
    }

    void Rebuild()
    {
        Scale();

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        material.mainTexture = texture;

        InitializeAndFill();
    }

    void Scale()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        height = Mathf.FloorToInt(width / aspectRatio);
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 scale = transform.localScale;
        scale.x = worldScreenWidth / 10;
        scale.y = 1;
        scale.z = worldScreenHeight / 10;

        renderedObject.transform.localScale = scale;
    }

    public void InitializeAndFill()
    {
        chunks = new List<NodeChunk>();
        NodeChunk chunk = new NodeChunk(width, height, brushType);
        chunk.Render(texture);
        chunks.Add(chunk);
    }

    public void Reset()
    {
        chunks = new List<NodeChunk>();
        NodeChunk chunk = new NodeChunk(width, height, NodeType.Empty);
        chunk.Render(texture);
        chunks.Add(chunk);
    }

    private Vector2Int GetNodeAtMousePosition()
    {
        Vector3 viewPortPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        int x = Mathf.FloorToInt(viewPortPoint.x * width);
        int y = Mathf.FloorToInt(viewPortPoint.y * height);
        return new Vector2Int(x, y);
    }

    public void SetOptions(SimulationOptions options)
    {
        drawRadius = options.BrushSize;
        fps = options.FPS;
        if (width != options.Size)
        {
            width = options.Size;
            Rebuild();
        }
    }

    public void ChangeType(NodeType type)
    {
        brushType = type;
    }


    private void DrawCircle(Node startNode, int radius, NodeType nodeType)
    {
        bool eraser = nodeType == NodeType.Empty;
        if (radius == 1)
        {
            chunks[0].SetNode(startNode.X, startNode.Y, Node.NewNode(startNode.X, startNode.Y, nodeType));
            return;
        }
        for (int x = -radius; x <= radius; x += 1)
        {
            for (int y = -radius; y <= radius; y += 1)
            {
                int xPos = startNode.X + x;
                int yPos = startNode.Y + y;
                double distance = Math.Sqrt(Math.Pow(xPos - startNode.X, 2) + Math.Pow(yPos - startNode.Y, 2));
                if (distance > radius)
                {
                    continue;
                }
                Node node = chunks[0].GetNode(xPos, yPos);
                if (eraser)
                {
                    if (node != null && !node.IsEmpty())
                    {
                        chunks[0].SetNode(xPos, yPos, Node.NewNode(xPos, yPos, nodeType));
                    }
                }
                else
                {
                    if (node != null && node.IsEmpty())
                    {
                        chunks[0].SetNode(xPos, yPos, Node.NewNode(xPos, yPos, nodeType));
                    }
                }
            }
        }
    }

    void Update()
    {
        if (GameManager.main.MenuOpen)
        {
            return;
        }
        ProcessHotkeys();
        ProcessMouseInput();
        timer += Time.deltaTime;
        interval = 1.0f / fps;
        if (timer > interval)
        {
            Calculate();
            Draw();
            timer = 0f;
        }
    }

    private void ProcessHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitializeAndFill();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Reset();
        }
    }

    private void ProcessMouseInput()
    {
        bool leftMouseButtonWasClicked = Input.GetMouseButton(0);
        bool rightMouseButtonWasClicked = Input.GetMouseButton(1);

        if (!leftMouseButtonWasClicked && !rightMouseButtonWasClicked)
        {
            return;
        }

        // if over UI 
        bool isOver = EventSystem.current.IsPointerOverGameObject();
        if (isOver)
        {
            return;
        }
        if (leftMouseButtonWasClicked)
        {
            Vector2Int nodePos = GetNodeAtMousePosition();
            Node node = chunks[0].GetNode(nodePos.x, nodePos.y);
            if (node != null)
            {
                DrawCircle(node, drawRadius, brushType);
            }
        }
        if (rightMouseButtonWasClicked)
        {
            Vector2Int nodePos = GetNodeAtMousePosition();
            Node node = chunks[0].GetNode(nodePos.x, nodePos.y);
            if (node != null)
            {
                DrawCircle(node, drawRadius, NodeType.Empty);
            }
        }
    }

    void Calculate()
    {
        foreach (NodeChunk chunk in chunks)
        {
            chunk.Calculate();
        }
    }

    void Draw()
    {
        foreach (NodeChunk chunk in chunks)
        {
            chunk.Render(texture);
        }
    }
}





