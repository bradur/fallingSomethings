using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ValueSlider : MonoBehaviour
{
    [SerializeField]
    private string title;

    [SerializeField]
    private TextMeshProUGUI txtValue;
    [SerializeField]
    private int min = 1;
    [SerializeField]
    private int max = 50;

    [SerializeField]
    private UnityEngine.UI.Slider slider;

    private UnityAction<int> callback;

    public void Initialize(int value, UnityAction<int> menuCallback)
    {
        callback = menuCallback;
        slider.value = value;
        slider.minValue = min;
        slider.maxValue = max;
    }

    private void UpdateTxt(float value)
    {
        txtValue.text = $"{title}: {value}";
    }

    public void UpdateSlider(float value)
    {
        UpdateTxt(value);
        if (callback != null)
        {
            callback(Mathf.FloorToInt(value));
        }
    }
}
