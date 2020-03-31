using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QAModel
{
    public string Question;
    public string[] Answers;
    public int Hint;

    public QAModel (string question, string[] answers, int hint)
    {
        Question = question;
        Answers = answers;
        Hint = hint;
    }
}
