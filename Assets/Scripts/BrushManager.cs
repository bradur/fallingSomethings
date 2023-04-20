using System.Collections.Generic;
using UnityEngine;

public class BrushManager : MonoBehaviour
{
    [SerializeField]
    private List<BrushConfig> brushes = new List<BrushConfig>();
    [SerializeField]
    private List<UIBrushColor> uiBrushes = new List<UIBrushColor>();

    [SerializeField]
    private UIBrushColor uiBrushPrefab;
    [SerializeField]
    private Transform uiBrushContainer;

    private TextureDrawer textureDrawer;

    public void Initialize(TextureDrawer drawer)
    {
        textureDrawer = drawer;
        int index = 0;
        foreach (BrushConfig config in brushes)
        {
            UIBrushColor brushColor = Instantiate(uiBrushPrefab, uiBrushContainer);
            brushColor.Initialize(config);
            uiBrushes.Add(brushColor);
            if (index == 0)
            {
                GameManager.main.SelectBrush(config);
            }
            index += 1;
        }
    }

    public void SelectBrush(BrushConfig brushConfig)
    {
        foreach (UIBrushColor uiBrush in uiBrushes)
        {
            uiBrush.SelectOrDeselect(brushConfig.Type);
        }
        textureDrawer.ChangeType(brushConfig);
    }

    public Color RandomBrushColor(NodeType nodeType)
    {
        foreach (BrushConfig config in brushes)
        {
            if (config.Type == nodeType)
            {
                return config.RandomBrushColor();
            }
        }
        return Color.clear;
    }

    public Color BrushColor(NodeType nodeType)
    {
        foreach (BrushConfig config in brushes)
        {
            if (config.Type == nodeType)
            {
                return config.BrushColor();
            }
        }
        return Color.clear;
    }

    void Update()
    {
        foreach (BrushConfig config in brushes)
        {
            if (Input.GetKeyDown(config.Hotkey))
            {
                SelectBrush(config);
                break;
            }
        }
    }
}
