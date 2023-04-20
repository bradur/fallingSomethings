using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBrushColor : MonoBehaviour
{

    [SerializeField]
    private Color borderColor;

    [SerializeField]
    private Image imgBorder;
    [SerializeField]
    private Image imgBackground;
    [SerializeField]
    private TextMeshProUGUI txtHotkey;
    [SerializeField]
    private TextMeshProUGUI txtBrushName;

    private bool isSelected = false;

    private Color originalBorderColor;

    private BrushConfig config;

    public NodeType Type { get { return config.Type; } }

    public void Initialize(BrushConfig brushConfig)
    {
        config = brushConfig;
        originalBorderColor = imgBorder.color;
        imgBackground.color = brushConfig.BrushColor();
        txtHotkey.text = KeyCodeToString(brushConfig.Hotkey);
        txtBrushName.text = $"{brushConfig.BrushName}";
        if (isSelected)
        {
            Select();
        }
    }

    private string KeyCodeToString(KeyCode key)
    {
        if (key == KeyCode.Alpha0)
        {
            return "0";
        }
        if (key == KeyCode.Alpha1)
        {
            return "1";
        }
        if (key == KeyCode.Alpha2)
        {
            return "2";
        }
        if (key == KeyCode.Alpha3)
        {
            return "3";
        }
        if (key == KeyCode.Alpha4)
        {
            return "4";
        }
        if (key == KeyCode.Alpha5)
        {
            return "5";
        }
        return $"{key}";
    }

    public void SelectOrDeselect(KeyCode key)
    {
        if (key == config.Hotkey)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    public void SelectOrDeselect(NodeType nodeType)
    {
        if (nodeType == config.Type)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    public void Click()
    {
        GameManager.main.SelectBrush(config);
    }

    public void Select()
    {
        isSelected = true;
        imgBorder.color = borderColor;
    }

    public void Deselect()
    {
        isSelected = false;
        imgBorder.color = originalBorderColor;
    }

}
