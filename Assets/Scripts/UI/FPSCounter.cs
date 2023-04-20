using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float expSmoothingFactor = 0.9f;
    [SerializeField]
    private float refreshFrequency = 0.4f;

    private float timeSinceUpdate = 0f;
    private float averageFps = 1f;

    [SerializeField]
    private TextMeshProUGUI txtFps;

    private void Update()
    {
        // Exponentially weighted moving average (EWMA)
        averageFps = expSmoothingFactor * averageFps + (1f - expSmoothingFactor) * 1f / Time.unscaledDeltaTime;

        if (timeSinceUpdate < refreshFrequency)
        {
            timeSinceUpdate += Time.deltaTime;
            return;
        }

        int fps = Mathf.RoundToInt(averageFps);
        txtFps.text = $"FPS: {fps}";

        timeSinceUpdate = 0f;
    }
}