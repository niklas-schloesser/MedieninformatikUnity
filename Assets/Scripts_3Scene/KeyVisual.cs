using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class KeyVisual : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float floatAmplitude = 0.25f; // how high it floats
    public float floatFrequency = 1f;    // speed of bobbing

    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;    // degrees per second

    [Header("Pulsing Settings")]
    public float pulseAmplitude = 0.1f;  // max scale change
    public float pulseFrequency = 3f;    // speed of pulsing

    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    public float glowIntensity = 1.5f;

    private Vector3 startPos;
    private Vector3 startScale;
    private Renderer rend;
    private Material matInstance;

    private void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;

        // Make a unique material instance for glow
        rend = GetComponent<Renderer>();
        matInstance = rend.material;
        matInstance.EnableKeyword("_EMISSION");
        matInstance.SetColor("_EmissionColor", glowColor * glowIntensity);
    }

    private void Update()
    {
        // Bobbing up and down
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Rotate around Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Pulse scale
        float scale = 1 + pulseAmplitude * Mathf.Sin(Time.time * pulseFrequency);
        transform.localScale = startScale * scale;
    }

    private void OnDestroy()
    {
        // Cleanup material instance to avoid memory leaks
        if (matInstance != null)
            Destroy(matInstance);
    }
}
