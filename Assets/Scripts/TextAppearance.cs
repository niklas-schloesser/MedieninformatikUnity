using Unity.VisualScripting;
using UnityEngine;

public class TextAppearance : MonoBehaviour
{
    [SerializeField] private GameObject textObject;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            textObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        textObject.SetActive(false);
    }
}
