using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Room1Puzzle : MonoBehaviour
{
    private Inventory inventoryScript;
    private Animator boardAnimator;

    [SerializeField] private ObjectInteraction boxInteractionScript;
    [SerializeField] private TextMeshProUGUI objectiveText;

    private bool hasKey;
    private bool playedAnimation;

    void Start()
    {
        inventoryScript = this.GetComponent<Inventory>();
        boardAnimator = GameObject.Find("Board").GetComponent<Animator>();
    }

    void Update()
    {
        if (inventoryScript.inventory.Contains("_Key") && !hasKey)
        {
            hasKey = true;
            boxInteractionScript.message += "<br><br>Du steckst den Schlüssel ein - die Tafel beginnt sich zu bewegen?!";
        }

        if (hasKey && !playedAnimation && boxInteractionScript.finishedText)
        {
            SolvePuzzle();
        }
    }

    void SolvePuzzle()
    {
        playedAnimation = true;
        boardAnimator.SetBool("boardMove", true);
        objectiveText.text = "Sammle den Abschluss ein!";
    }
}