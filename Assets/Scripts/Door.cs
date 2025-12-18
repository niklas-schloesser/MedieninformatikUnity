using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool inTrigger;
    [SerializeField] private Animator doorAnimator;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            inTrigger = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        inTrigger = false;
    }

    void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            doorAnimator.SetTrigger("openDoor");
        }
    }
}
