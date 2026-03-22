using UnityEngine;
using TMPro;

public class GlowPulse : MonoBehaviour
{
    private TMP_Text textMesh;
    private Material textMaterial;

    public float pulseSpeed = 2f;
    public float minGlow = 0.1f;
    public float maxGlow = 0.6f;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        // We create a unique instance of the material so we don't affect other text
        textMaterial = textMesh.fontSharedMaterial;

        // Ensure Glow is actually turned on in the shader
        textMesh.fontSharedMaterial.EnableKeyword("GLOW_ON");
    }

    void Update()
    {
        // Calculate a smooth ping-pong value between min and max
        float glowValue = Mathf.Lerp(minGlow, maxGlow, (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f);

        // "_GlowOuter" is the internal shader variable name for the Outer Glow setting
        textMesh.canvasRenderer.GetMaterial().SetFloat(ShaderUtilities.ID_GlowOuter, glowValue);
    }
}