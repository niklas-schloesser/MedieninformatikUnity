using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 0.25f; // tune in inspector
    [SerializeField] private float minPitch = -85f;
    [SerializeField] private float maxPitch = 85f;

    private float xRotation = 0f;
    private bool mouseActivated = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = transform.localEulerAngles.x;
        if (xRotation > 180f) xRotation -= 360f;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        if (!mouseActivated)
        {
            if (mouseDelta.sqrMagnitude < 0.01f) return;
            mouseActivated = true;
        }

        // IMPORTANT: no deltaTime here (mouse delta already per-frame)
        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
