using UnityEngine;

public class Room1Goal : MonoBehaviour
{
    private bool isInTrigger;
    private bool isReadingText;

    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private GameObject goalTextbox;

    private PlayerMovement playerScript;
    private FirstPersonCamera cameraScript;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            isInTrigger = true;
            interactionIcon.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            isInTrigger = false;
            interactionIcon.SetActive(false);
        }
    }

    void Update()
    {
        if (isInTrigger && !isReadingText && Input.GetKeyDown(KeyCode.E))
        {
            ReadText();
        }
    }

    void ReadText()
    {
        isReadingText = true;
        playerScript.canMove = false;
        playerScript.allowedToJump = false;
        cameraScript.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        goalTextbox.SetActive(true);
    }

    public void ButtonA()
    {
        Debug.Log("Test");
    }

    public void ButtonB()
    {

    }
}
