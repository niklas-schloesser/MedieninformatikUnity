using UnityEngine;
using System.Collections;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private Key.KeyType keyType;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    private bool isOpen = false;
    private AudioSource audioSource;
    public Timer timer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public Key.KeyType GetKeyType()
    {
        return keyType;
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        if (audioSource != null)
            audioSource.Play();

        StartCoroutine(OpenDoorCoroutine());

        if (timer != null)
         timer.StartTimer();

    }

    private IEnumerator OpenDoorCoroutine()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, openAngle, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }
    }
}
