using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float amplitude = 0.5f; // How far up and down it moves
    [SerializeField] private float frequency = 1f;   // How fast it moves

    private Vector3 startPos;

    void Start()
    {
        // Save the original position so we can move relative to it
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Calculate the new Y position using a Sine wave
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;

        transform.localPosition = newPos;

        // Make the text always face the player camera so it is readable
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }
}