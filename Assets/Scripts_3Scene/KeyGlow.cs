using UnityEngine;

public class KeyGlow : MonoBehaviour
{
    public Color glowColor = Color.yellow;
    public float glowIntensity = 2f;

    private Renderer rend;
    private Material matInstance;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            matInstance = rend.material; // create instance for this key
            matInstance.EnableKeyword("_EMISSION");
            matInstance.SetColor("_EmissionColor", glowColor * glowIntensity);
        }
    }
}

