using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private TextMeshProUGUI fpsTextComponent; // Assign in inspector
    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;

    void Start()
    {
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            fpsTextComponent.text = $"FPS {fps:F2}"; // Update TextMeshPro text

            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}