using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private OptionsMenu menu;

    [SerializeField]
    private TextureDrawer textureDrawer;

    [SerializeField]
    private List<UIBrushColor> uIBrushColors = new List<UIBrushColor>();

    [SerializeField]
    private List<Color> sandColors = new List<Color>();

    [SerializeField]
    private List<Color> woodColors = new List<Color>();

    [SerializeField]
    private List<Color> waterColors = new List<Color>();

    [SerializeField]
    private List<Color> fireColors = new List<Color>();

    public Color RandomBrushColor(NodeType nodeType)
    {
        if (nodeType == NodeType.Sand)
        {
            return sandColors[UnityEngine.Random.Range(0, sandColors.Count)];
        }
        if (nodeType == NodeType.Water)
        {
            return waterColors[UnityEngine.Random.Range(0, waterColors.Count)];
        }
        if (nodeType == NodeType.Wood)
        {
            return woodColors[UnityEngine.Random.Range(0, woodColors.Count)];
        }
        if (nodeType == NodeType.Fire)
        {
            return fireColors[UnityEngine.Random.Range(0, fireColors.Count)];
        }
        return Color.clear;
    }

    public Color BrushColor(NodeType nodeType)
    {
        if (nodeType == NodeType.Sand)
        {
            return sandColors[0];
        }
        if (nodeType == NodeType.Water)
        {
            return waterColors[0];
        }
        if (nodeType == NodeType.Wood)
        {
            return woodColors[0];
        }
        if (nodeType == NodeType.Fire)
        {
            return fireColors[0];
        }
        return Color.clear;
    }

    private bool menuOpen = false;
    public bool MenuOpen { get { return menuOpen; } }

    public void OpenOptions()
    {
        menuOpen = true;
        menu.Show();
    }


    public void InitializeAndFill()
    {
        textureDrawer.InitializeAndFill();
    }
    public void Reset()
    {
        textureDrawer.Reset();
    }

    public int RandomChoice(int value1, int value2)
    {
        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
        {
            return value1;
        }
        return value2;
    }


    public void CloseOptions()
    {
        menuOpen = false;
        menu.Hide();
        UpdateOptions();
    }

    public void UpdateOptions()
    {
        textureDrawer.SetOptions(menu.Options);
    }

    public void SelectBrush(NodeType nodeType)
    {
        foreach (UIBrushColor brushColor in uIBrushColors)
        {
            if (brushColor.NodeType == nodeType)
            {
                brushColor.Select();
            }
            else
            {
                brushColor.Deselect();
            }
        }
        textureDrawer.ChangeType(nodeType);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (menuOpen)
            {
                CloseOptions();
            }
            else
            {
                OpenOptions();
            }
        }
        foreach (UIBrushColor uIBrushColor in uIBrushColors)
        {
            if (Input.GetKeyDown(uIBrushColor.Hotkey))
            {
                SelectBrush(uIBrushColor.NodeType);
                break;
            }
        }
    }
}

