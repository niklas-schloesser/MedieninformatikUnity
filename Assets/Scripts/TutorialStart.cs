using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStart : MonoBehaviour
{
    private bool inDialogue;

    private PlayerMovement player;
    private FirstPersonCamera cameraScript;

    [SerializeField] Animator frameAnimator;
    // [SerializeField] Animator textAnimatorTop;
    // [SerializeField] Animator textAnimatorBottom;
    void Start()
    {
        inDialogue = true;
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();

        StartCoroutine(introCoroutine());
    }

    void Update()
    {
        if (inDialogue)
        {
            player.isGrounded = false;
            cameraScript.canMove = false;
        }
    }

    IEnumerator introCoroutine()
    {
        frameAnimator.SetTrigger("appearTop");
        // textAnimatorTop.SetTrigger("appearTop");
        yield return new WaitForSeconds(0.5f);
        frameAnimator.SetTrigger("appearBottom");
        // textAnimatorBottom.SetTrigger("appearBottom");
    }
}