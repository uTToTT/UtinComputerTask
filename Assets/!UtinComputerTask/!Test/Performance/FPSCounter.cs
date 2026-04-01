using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float smooth = 0.1f;

    private float _deltaTime;

    private void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * smooth;

        float fps = 1.0f / _deltaTime;
        text.text = $"FPS: {fps:0}";
    }
}