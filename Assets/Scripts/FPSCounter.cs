using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour {
    public TextMeshProUGUI fpsText;
    public float updateInterval = 0.5f;

    private float accum = 0;
    private int frames = 0;
    private float timeLeft;

    private void Start() {
        timeLeft = updateInterval;
    }

    private void Update() {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        // Update FPS counter every updateInterval seconds
        if (timeLeft <= 0.0) {
            float fps = accum / frames;
            fpsText.text = "FPS: " + Mathf.RoundToInt(fps);

            timeLeft = updateInterval;
            accum = 0;
            frames = 0;
        }
    }
}