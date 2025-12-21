using System.Collections.Generic;
using UnityEngine;



public class KeyHolder : MonoBehaviour
{
    private List<Key.KeyType> keyList;

    private void Awake()
    {
        keyList = new List<Key.KeyType>();
    }

    public void AddKey(Key.KeyType keyType)
    {
        Debug.Log("Picked up key: " + keyType);
        keyList.Add(keyType);
    }

    public bool HasKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
    }

    private void OnTriggerEnter(Collider other)
{
    // PICK UP KEY
    Key key = other.GetComponent<Key>();
    if (key != null)
    {
        Debug.Log("KEY PICKED UP!");
        AddKey(key.GetKeyType());

        // Play pickup sound if key has an AudioSource
        AudioSource audioSource = key.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Optional: play particle effect if key has one
        ParticleSystem particles = key.GetComponentInChildren<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }

        // Disable key visuals instead of destroying immediately
        Renderer rend = key.GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false;

        Collider col = key.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Destroy the key after a short delay to allow sound/particles to play
        Destroy(key.gameObject, 0.5f);

        return;
    }

    // OPEN DOOR
    KeyDoor keyDoor = other.GetComponent<KeyDoor>();
    if (keyDoor != null && HasKey(keyDoor.GetKeyType()))
    {
        keyDoor.OpenDoor();
        RemoveKey(keyDoor.GetKeyType());
    }
}

}
