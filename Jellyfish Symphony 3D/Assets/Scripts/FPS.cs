using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public float fpsUpdateInterval;
    private float fpsTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fpsTimer += Time.unscaledDeltaTime;
        if (fpsTimer >= fpsUpdateInterval)
        {
            UpdateFPSText();
            fpsTimer = 0f;
        }
    }
    void UpdateFPSText()
    {
        float fps = 1f / Time.unscaledDeltaTime;
        fpsText.text = "FPS: " + Mathf.RoundToInt(fps).ToString();
    }
}
