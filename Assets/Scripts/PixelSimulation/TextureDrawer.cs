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
    private List<NodeChunk> chunks = new List<NodeChunk>();

    private Texture2D texture;
    private float timer = 0f;
    private float interval = 1.0f;

    private int drawRadius = 1;

    private BrushConfig brushConfig;

    [SerializeField]
    private LayerMask drawLayer;

    private bool paused = false;

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
        NodeChunk chunk = new NodeChunk(width, height, brushConfig);
        chunk.Render(texture);
        chunks.Add(chunk);
    }

    public void Reset()
    {
        chunks = new List<NodeChunk>();
        NodeChunk chunk = new NodeChunk(width, height);
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

    public void ChangeType(BrushConfig config)
    {
        brushConfig = config;
    }


    private void DrawCircle(Node startNode, int radius, BrushConfig config = null)
    {
        if (radius == 1)
        {
            chunks[0].SetNode(startNode.X, startNode.Y, Node.NewNode(startNode.X, startNode.Y, config));
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
                if (config == null)
                {
                    if (node != null && !node.IsEmpty())
                    {
                        chunks[0].SetNode(xPos, yPos, Node.NewNode(xPos, yPos, config));
                    }
                }
                else
                {
                    if (node != null && node.IsEmpty())
                    {
                        chunks[0].SetNode(xPos, yPos, Node.NewNode(xPos, yPos, config));
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
        if (paused)
        {
            return;
        }
        timer += Time.deltaTime;
        interval = 1.0f / fps;
        if (timer > interval)
        {
            Calculate();
            Draw();
            timer = 0f;
        }
    }

    private void TogglePause()
    {
        paused = !paused;
    }

    private void ProcessHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TogglePause();
        }
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
        bool thirdMouseButtonWasClicked = Input.GetMouseButton(3);

        if (!leftMouseButtonWasClicked && !rightMouseButtonWasClicked && !thirdMouseButtonWasClicked)
        {
            return;
        }

        if (thirdMouseButtonWasClicked)
        {
            Vector2Int nodePos = GetNodeAtMousePosition();
            Node node = chunks[0].GetNode(nodePos.x, nodePos.y);
            Debug.Log(node);
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
                DrawCircle(node, drawRadius, brushConfig);
            }
        }
        if (rightMouseButtonWasClicked)
        {
            Vector2Int nodePos = GetNodeAtMousePosition();
            Node node = chunks[0].GetNode(nodePos.x, nodePos.y);
            if (node != null)
            {
                DrawCircle(node, drawRadius);
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





