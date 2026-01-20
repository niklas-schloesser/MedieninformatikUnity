using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Quiz : MonoBehaviour
{
    [Header("UI References")]
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI feedbackText;      // shows "Correct / Wrong"
    public GameObject[] quizButtons;          // size 4
    public TextMeshProUGUI[] buttonTexts;     // size 4

    [Header("Player Lock")]
    public PlayerController playerController;

    [Header("Boxes (BoxOpenByCode)")]
    public BoxOpenByCode gunBox;
    public BoxOpenByCode knifeBox;

    [Header("Optional")]
    public GameObject interactPrompt;

    [Header("Feedback Colors")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color normalColor = Color.white;

    private enum State { MAIN, UNI_LIST, WORK_LIST, QUESTION, CONFIRM_NOTHING }
    private State state;

    private string correctAnswer;
    private string lastSelectedOption;
    private int attemptsLeft;

    private enum RewardType { None, Gun, Knife }
    private RewardType currentReward;

    private bool isShowingFeedback;

    private HashSet<string> lockedMajors = new HashSet<string>();
    private HashSet<string> lockedJobs = new HashSet<string>();

    private readonly string[] allMajors = { "Engineering", "Medicine", "Arts", "Computer Science" };
    private readonly string[] allJobs = { "Construction", "Office", "Sales", "Technician" };

    private void Start()
    {
        if (quizPanel != null) quizPanel.SetActive(false);
        ClearFeedback();
    }

    // Called by PlayerInteract
    public void StartQuiz()
    {
        if (quizPanel == null)
        {
            Debug.LogError("Quiz: quizPanel not assigned!");
            return;
        }

        quizPanel.SetActive(true);

        if (playerController != null) playerController.CanMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (interactPrompt != null) interactPrompt.SetActive(false);

        ClearFeedback();
        ShowMainMenu();
    }

    // UI Buttons call Quiz.OnClickButton(index)
    public void OnClickButton(int index)
    {
        if (isShowingFeedback) return;
        if (index < 0 || index >= buttonTexts.Length) return;

        string choice = buttonTexts[index].text;
        ClearFeedback(); // every click resets feedback unless we show new feedback

        switch (state)
        {
            case State.MAIN:
                if (choice == "Continue University") ShowUniversityList();
                else if (choice == "Go to Work") ShowWorkList();
                else ShowDoNothingConfirm();
                break;

            case State.UNI_LIST:
                if (choice == "Go Back") ShowMainMenu();
                else StartUniversityQuestion(choice);
                break;

            case State.WORK_LIST:
                if (choice == "Go Back") ShowMainMenu();
                else StartWorkQuestion(choice);
                break;

            case State.QUESTION:
                CheckAnswer(choice);
                break;

            case State.CONFIRM_NOTHING:
                if (choice == "YES") CloseQuiz();
                else ShowMainMenu();
                break;
        }
    }

    // ---------- MENUS ----------
    private void ShowMainMenu()
    {
        state = State.MAIN;

        bool uniAvailable = lockedMajors.Count < allMajors.Length;
        bool workAvailable = lockedJobs.Count < allJobs.Length;

        var options = new List<string>();
        if (uniAvailable) options.Add("Continue University");
        if (workAvailable) options.Add("Go to Work");
        options.Add("Do Nothing");

        UpdateUI("What do you want to do after school?", options);
    }

    private void ShowUniversityList()
    {
        state = State.UNI_LIST;

        var options = new List<string>();
        foreach (var m in allMajors)
            if (!lockedMajors.Contains(m)) options.Add(m);

        options.Add("Go Back");
        UpdateUI("Choose a Major (1 attempt):", options);
    }

    private void ShowWorkList()
    {
        state = State.WORK_LIST;

        var options = new List<string>();
        foreach (var j in allJobs)
            if (!lockedJobs.Contains(j)) options.Add(j);

        options.Add("Go Back");
        UpdateUI("Pick a Job (2 attempts):", options);
    }

    private void ShowDoNothingConfirm()
    {
        state = State.CONFIRM_NOTHING;
        UpdateUI("Are you sure you want to do nothing?", new List<string> { "YES", "NO" });
    }

    // ---------- QUESTIONS ----------
    private void StartUniversityQuestion(string major)
    {
        state = State.QUESTION;
        currentReward = RewardType.Gun;
        attemptsLeft = 1;
        lastSelectedOption = major;

        if (major == "Engineering")
            SetQuestion("Engineering: Which force pulls objects to Earth?",
                new[] { "Gravity", "Magnetism", "Friction", "Electricity" }, "Gravity");

        else if (major == "Medicine")
            SetQuestion("Medicine: What is the largest organ in the human body?",
                new[] { "Heart", "Skin", "Liver", "Lungs" }, "Skin");

        else if (major == "Arts")
            SetQuestion("Arts: Who painted the Mona Lisa?",
                new[] { "Van Gogh", "Da Vinci", "Picasso", "Monet" }, "Da Vinci");

        else if (major == "Computer Science")
            SetQuestion("Computer Science: What does HTML stand for?",
                new[] { "HyperText Markup Language", "High Tech Machine Language", "Hyperlink Tool Markup List", "Home Tool Markup Language" },
                "HyperText Markup Language");
    }

    private void StartWorkQuestion(string job)
    {
        state = State.QUESTION;
        currentReward = RewardType.Knife;
        attemptsLeft = 2;
        lastSelectedOption = job;

        if (job == "Construction")
            SetQuestion("Work (Construction): Which tool checks if something is straight?",
                new[] { "Hammer", "Spirit Level", "Saw", "Drill" }, "Spirit Level");

        else if (job == "Office")
            SetQuestion("Work (Office): What does CC mean in an email?",
                new[] { "Carbon Copy", "Code Contact", "Closed Chat", "Copy Click" }, "Carbon Copy");

        else if (job == "Sales")
            SetQuestion("Work (Sales): What is the best first step with a new customer?",
                new[] { "Listen to needs", "Offer a discount instantly", "Ignore questions", "Talk only about price" }, "Listen to needs");

        else if (job == "Technician")
            SetQuestion("Work (Technician): What should you do BEFORE repairing a device?",
                new[] { "Disconnect power", "Pour water", "Hit it", "Remove screws randomly" }, "Disconnect power");
    }

    private void SetQuestion(string q, string[] answers, string correct)
    {
        correctAnswer = correct;

        if (questionText != null) questionText.text = q;

        for (int i = 0; i < quizButtons.Length; i++)
        {
            bool active = i < answers.Length;
            quizButtons[i].SetActive(active);
            if (active) buttonTexts[i].text = answers[i];
        }
    }

    // ---------- ANSWER CHECK ----------
    private void CheckAnswer(string choice)
    {
        if (choice == correctAnswer)
        {
            StartCoroutine(ShowCorrectThenReward());
            return;
        }

        attemptsLeft--;

        if (currentReward == RewardType.Gun)
        {
            lockedMajors.Add(lastSelectedOption);
            StartCoroutine(ShowWrongThenBack("Wrong Answer!\nThis major is locked."));
        }
        else // Knife
        {
            if (attemptsLeft <= 0)
            {
                lockedJobs.Add(lastSelectedOption);
                StartCoroutine(ShowWrongThenBack("Wrong Answer!\nThis job is locked."));
            }
            else
            {
                StartCoroutine(ShowWrongThenStay("Wrong Answer!\nOne attempt left."));
            }
        }
    }

    // ---------- FEEDBACK ----------
    private IEnumerator ShowCorrectThenReward()
    {
        isShowingFeedback = true;

        SetFeedback("Correct!\nClaim your reward.", correctColor);

        yield return new WaitForSecondsRealtime(0.8f);

        if (currentReward == RewardType.Gun)
        {
            if (gunBox != null) gunBox.OpenBox();
            else Debug.LogError("Quiz: gunBox NOT assigned!");
        }
        else if (currentReward == RewardType.Knife)
        {
            if (knifeBox != null) knifeBox.OpenBox();
            else Debug.LogError("Quiz: knifeBox NOT assigned!");
        }

        yield return new WaitForSecondsRealtime(0.4f);

        isShowingFeedback = false;
        CloseQuiz();
    }

    private IEnumerator ShowWrongThenBack(string msg)
    {
        isShowingFeedback = true;

        SetFeedback(msg, wrongColor);

        yield return new WaitForSecondsRealtime(0.9f);

        ClearFeedback();
        isShowingFeedback = false;

        ShowMainMenu();
    }

    private IEnumerator ShowWrongThenStay(string msg)
    {
        isShowingFeedback = true;

        SetFeedback(msg, wrongColor);

        yield return new WaitForSecondsRealtime(0.9f);

        ClearFeedback();
        isShowingFeedback = false;
        // stays on the same question so they can try again
    }

    private void SetFeedback(string msg, Color c)
    {
        if (feedbackText == null) return;
        feedbackText.text = msg;
        feedbackText.color = c;
    }

    private void ClearFeedback()
    {
        if (feedbackText == null) return;
        feedbackText.text = "";
        feedbackText.color = normalColor;
    }

    // ---------- CLOSE ----------
    private void CloseQuiz()
    {
        if (quizPanel != null) quizPanel.SetActive(false);

        if (playerController != null) playerController.CanMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (interactPrompt != null) interactPrompt.SetActive(true);

        ClearFeedback();
    }

    private void UpdateUI(string q, List<string> options)
    {
        if (questionText != null) questionText.text = q;

        for (int i = 0; i < quizButtons.Length; i++)
        {
            bool active = i < options.Count;
            quizButtons[i].SetActive(active);
            if (active) buttonTexts[i].text = options[i];
        }
    }
}
