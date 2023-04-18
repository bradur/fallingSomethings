using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBrushColor : MonoBehaviour
{
    [SerializeField]
    private KeyCode hotkey;
    public KeyCode Hotkey { get { return hotkey; } }
    [SerializeField]
    private string brushName;
    [SerializeField]
    private NodeType nodeType;
    public NodeType NodeType { get { return nodeType; } }

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

    [SerializeField]
    private bool isSelected = false;

    private Color originalBorderColor;
    private Color originalBgColor;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        originalBorderColor = imgBorder.color;
        originalBgColor = imgBackground.color;
        imgBackground.color = GameManager.main.BrushColor(nodeType);
        txtHotkey.text = KeyCodeToString(hotkey);
        txtBrushName.text = $"{brushName}";
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
        if (key == hotkey)
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
        GameManager.main.SelectBrush(nodeType);
    }

    public void Select()
    {
        isSelected = true;
        imgBorder.color = borderColor;
        //imgBackground.color = bgColor;
        //GameManager.main.SelectBrush(nodeType);
    }

    public void Deselect()
    {
        isSelected = false;
        imgBorder.color = originalBorderColor;
        //imgBackground.color = originalBgColor;
    }

}
