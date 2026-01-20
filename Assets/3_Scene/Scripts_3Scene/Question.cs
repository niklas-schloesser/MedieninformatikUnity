public class Question
{
    public string question;
    public string answerA;
    public string answerB;
    public string answerC;
    public string correctAnswer;

    public Question(string q, string a, string b, string c, string correct)
    {
        question = q;
        answerA = a;
        answerB = b;
        answerC = c;
        correctAnswer = correct;
    }
}
