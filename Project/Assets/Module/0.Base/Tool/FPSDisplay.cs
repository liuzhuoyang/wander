using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FPSDisplay : MonoBehaviour
{
    public float updateInterval = 0.5f;
    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;

    public TextMeshProUGUI fpsTextComponent; // Assign in inspector

    void Start()
    {
        if (fpsTextComponent == null)
        {
            Debug.LogWarning("FPSDisplay: Missing TextMeshProUGUI reference!");
            this.enabled = false;
            return;
        }

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