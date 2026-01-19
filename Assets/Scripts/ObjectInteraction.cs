using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectInteraction : MonoBehaviour
{
    private bool isInTrigger;
    private bool canReadText;
    private bool isReadingText = false;

    [SerializeField] private bool hasItem;
    [SerializeField] private string itemName;
    [SerializeField] private GameObject item;

    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI textBoxContent;

    public string message;

    private PlayerMovement playerScript;
    private FirstPersonCamera cameraScript;
    private Inventory inventoryScript;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
        inventoryScript = GameObject.Find("Player").GetComponent<Inventory>();

        canReadText = true;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            isInTrigger = true;
            if (canReadText)
            {
                interactionIcon.SetActive(true);
            }
            else
            {
                interactionIcon.SetActive(false);
            }
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
        if (isInTrigger && canReadText && !isReadingText && Input.GetKeyDown(KeyCode.E))
        {
            ReadText();
        }

        if (isReadingText && Input.GetKeyDown(KeyCode.Escape))
        {
            isReadingText = false;
            playerScript.canMove = true;
            playerScript.allowedToJump = true;
            cameraScript.canMove = true;
            textBox.SetActive(false);
            if (hasItem)
            {
                PickupItem();
            }
        }
    }
    void ReadText()
    {
        isReadingText = true;
        playerScript.canMove = false;
        playerScript.allowedToJump = false;
        cameraScript.canMove = false;
        textBoxContent.text = message;
        textBox.SetActive(true);
    }

    void PickupItem()
    {
        item.SetActive(false);
        interactionIcon.SetActive(false);
        inventoryScript.inventory += "_" + itemName;
        canReadText = false;
    }
}