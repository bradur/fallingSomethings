using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BrushConfig", menuName = "Configs/Brush Config")]
public class BrushConfig : ScriptableObject
{

    [SerializeField]
    private List<Color> colors = new List<Color>();

    [SerializeField]
    private string brushName = "brush";
    public string BrushName { get { return brushName; } }

    [SerializeField]
    private NodeType nodeType;
    public NodeType Type { get { return nodeType; } }

    [SerializeField]
    private KeyCode hotkey;
    public KeyCode Hotkey { get { return hotkey; } }

    public Color RandomBrushColor()
    {
        if (colors.Count > 0)
        {
            return colors[UnityEngine.Random.Range(0, colors.Count)];
        }
        return Color.clear;
    }

    public Color BrushColor()
    {
        if (colors.Count > 0)
        {
            return colors[0];
        }
        return Color.clear;
    }
}
