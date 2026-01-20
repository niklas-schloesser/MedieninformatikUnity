using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    public Quiz quiz;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            quiz.StartQuiz();
        }
    }
}
