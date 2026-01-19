using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Room1Puzzle : MonoBehaviour
{
    private Inventory inventoryScript;
    private Animator boardAnimator;

    [SerializeField] private ObjectInteraction boxInteractionScript;

    private bool puzzleSolved;
    private bool playedAnimation;

    void Start()
    {
        inventoryScript = this.GetComponent<Inventory>();
        boardAnimator = GameObject.Find("Board").GetComponent<Animator>();
    }

    void Update()
    {
        if (inventoryScript.inventory.Contains("_Key") && !puzzleSolved)
        {
            puzzleSolved = true;
            boxInteractionScript.message += "<br><br>Du steckst den Schlüssel ein - die Tafel beginnt sich zu bewegen?!";
        }

        if (puzzleSolved && !playedAnimation && Input.GetKeyDown(KeyCode.Escape))
        {
            playedAnimation = true;
            boardAnimator.SetTrigger("boardMove");
        }
    }
}
