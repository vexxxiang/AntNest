using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Kolorowanie
        string color;
        if (fps >= 50)
            color = "green";
        else if (fps >= 30)
            color = "yellow";
        else
            color = "red";

        fpsText.text = $"<color={color}>FPS: {Mathf.CeilToInt(fps)}</color>";
    }
}