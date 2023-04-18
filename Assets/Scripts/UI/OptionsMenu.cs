using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField]
    private SimulationOptions options;

    public SimulationOptions Options { get { return options; } }

    [SerializeField]
    private ValueSlider FpsSlider;
    [SerializeField]
    private ValueSlider SizeSlider;
    [SerializeField]
    private ValueSlider BrushSlider;

    [SerializeField]
    private Transform container;

    void Start()
    {
        FpsSlider.Initialize(options.FPS, SetFPS);
        SizeSlider.Initialize(options.Size, SetSize);
        BrushSlider.Initialize(options.BrushSize, SetBrushSize);
        GameManager.main.UpdateOptions();
    }

    public void SetFPS(int fps)
    {
        options.FPS = fps;
    }
    public void SetBrushSize(int bSize)
    {
        options.BrushSize = bSize;
    }

    public void SetSize(int size)
    {
        options.Size = size;
    }

    public void Show()
    {
        container.gameObject.SetActive(true);
    }
    public void Hide()
    {
        container.gameObject.SetActive(false);
    }

    public void SaveButtonClicked()
    {
        GameManager.main.CloseOptions();
    }
}

[System.Serializable]
public class SimulationOptions
{
    public int FPS = 30;
    public int Size = 128;
    public int BrushSize = 3;
}