using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework.Constraints;

public class TutorialStart : MonoBehaviour
{
    private bool inDialogue;
    private bool endDialogue;

    private PlayerMovement player;
    private Rigidbody playerRigidbody;
    private FirstPersonCamera cameraScript;

    [SerializeField] private Animator frameAnimator;
    [SerializeField] private TextMeshProUGUI topText;
    [SerializeField] private TextMeshProUGUI bottomText;
    [SerializeField] private GameObject continueText;

    [SerializeField] private string topDialogue;
    [SerializeField] private string bottomDialogue;

    void Start()
    {
        inDialogue = true;
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();

        StartCoroutine(IntroCoroutine());
    }

    void Update()
    {
        if (inDialogue)
        {
            player.allowedToJump = false;
            cameraScript.canMove = false;
            playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }

        if (endDialogue && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(EndCoroutine());
        }
    }

    IEnumerator IntroCoroutine()
    {
        frameAnimator.SetTrigger("appearTop");
        topText.text = topDialogue;
        yield return new WaitForSeconds(1f);
        frameAnimator.SetTrigger("appearBottom");
        bottomText.text = bottomDialogue;
        yield return new WaitForSeconds(1f);
        continueText.SetActive(true);
        endDialogue = true;
    }

    IEnumerator EndCoroutine()
    {
        continueText.SetActive(false);
        frameAnimator.SetTrigger("disappearBottom");
        yield return new WaitForSeconds(0.5f);
        frameAnimator.SetTrigger("disappearTop");
        inDialogue = false;
        player.allowedToJump = true;
        cameraScript.canMove = true;
        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}