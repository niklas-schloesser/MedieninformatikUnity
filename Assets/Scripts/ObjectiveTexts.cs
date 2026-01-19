
using System.Collections;
using UnityEngine;

public class ObjectiveTexts : MonoBehaviour
{
    [SerializeField] private GameObject locationText;
    [SerializeField] private GameObject objectiveText;

    private PlayerMovement playerScript;
    private bool hasPlayed;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && !hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(InfoText());
        }
    }

    IEnumerator InfoText()
    {
        playerScript.canMove = false;
        locationText.SetActive(true);
        yield return new WaitForSeconds(4f);
        playerScript.canMove = true;
        locationText.SetActive(false);
        objectiveText.SetActive(true);
    }
}