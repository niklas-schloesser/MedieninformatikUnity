using UnityEngine;

public class Key : MonoBehaviour
{
    public enum KeyType
    {
        Blue,
        Silver,
        Gold
    }

    [SerializeField] private KeyType keyType;

    public KeyType GetKeyType()
    {
        return keyType;
    }
}
