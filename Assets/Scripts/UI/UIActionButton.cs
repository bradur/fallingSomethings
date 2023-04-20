using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIActionButton : MonoBehaviour
{
    [SerializeField]
    private KeyCode hotkey;

    [SerializeField]
    private string brushName;

    [SerializeField]
    private Color bgColor;

    [SerializeField]
    private Image imgBackground;
    [SerializeField]
    private TextMeshProUGUI txtHotkey;
    [SerializeField]
    private TextMeshProUGUI txtName;

    [SerializeField]
    private UIAction action;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        imgBackground.color = bgColor;
        txtHotkey.text = hotkey.ToString();
        txtName.text = $"{brushName}";
    }

    public void Click()
    {
        switch (action)
        {
            case UIAction.Options:
                GameManager.main.OpenOptions();
                break;
            case UIAction.Clear:
                GameManager.main.Reset();
                break;
            case UIAction.ClearAndFill:
                GameManager.main.InitializeAndFill();
                break;
            default:
                break;
        }
    }

}

public enum UIAction
{
    Clear,
    ClearAndFill,
    Options
}
