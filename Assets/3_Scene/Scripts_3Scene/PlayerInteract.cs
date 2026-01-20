using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Quiz quiz;
    public float interactRange = 2.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryInteract();
    }

    void TryInteract()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("QuizButton"))
            {
                quiz.StartQuiz();
            }
        }
    }
}
