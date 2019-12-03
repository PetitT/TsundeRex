using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TheBiggestScript : MonoBehaviour
{
    public Text state_question_text;
    public List<Text> answers_text;
    public Text score_text;

    private List<string> questions = new List<string>();
    private List<List<Tuple<string, int>>> answers = new List<List<Tuple<string, int>>>();
    private List<string[]> answerResults = new List<string[]>();

    private int currentQuestion = 0;
    private int currentAnswer = 0;

    public int startScore;
    public int maxScore;
    private int currentScore;

    private bool isUpdatingText = true;
    private bool isInitializing = true;
    private bool isReadingInputs = true;
    private bool hasAnswered = false;
    private bool isUpdatingScore = true;

    public float timeToWaitAfterInput;

    private void Start()
    {
        StartCoroutine("Game");
    }

    private IEnumerator Game()
    {
        while (true)
        {
            #region Initialization

            if (isInitializing)
            {
                #region AddAnswers

                answers.Add(new List<Tuple<string, int>>());
                answers[0].Add(new Tuple<string, int>("42", 1));
                answers[0].Add(new Tuple<string, int>("Kek", 2));
                answers[0].Add(new Tuple<string, int>("Huitre", -1));
                answers[0].Add(new Tuple<string, int>("Mais est-ce que ça va fonctionner?", -2));

                #endregion

                #region AddAnswerResults

                answerResults.Add(new string[] { "Wow très original", "Kek à toi, frère", "Oui tout à fait cohérent", "Mais t'es un génie, bro" });

                #endregion

                #region AddQuestions

                questions.Add("What is the meaning of life");

                #endregion

                #region OtherStuff

                currentScore = startScore;
                score_text.text = currentScore.ToString();

                #endregion

                isInitializing = false;
            }
            #endregion

            #region TextUpdate

            if (isUpdatingText)
            {
                state_question_text.text = questions[currentQuestion];
                for (int i = 0; i < answers_text.Count; i++)
                {
                    answers_text[i].text = answers[currentQuestion][i].Item1;
                }
                isUpdatingText = false;
            }

            #endregion

            #region AnswerResults

            if (hasAnswered)
            {
                #region UpdateTextAndScore

                state_question_text.text = answerResults[currentQuestion][currentAnswer];
                foreach (var text in answers_text)
                {
                    text.text = "";
                }

                currentScore += answers[currentQuestion][currentAnswer].Item2;
                score_text.text = currentScore.ToString();

                #endregion

                #region CheckScore

                if (currentScore <= 0)
                {
                    foreach (var text in answers_text)
                    {
                        text.text = "";
                    }

                    state_question_text.text = "You Lost...";

                    yield return new WaitForSeconds(timeToWaitAfterInput);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                if (currentScore >= maxScore)
                {
                    foreach (var text in answers_text)
                    {
                        text.text = "";
                    }
                    state_question_text.text = "You Won!";

                    yield return new WaitForSeconds(timeToWaitAfterInput);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                #endregion

                #region ChangeQuestion

                if (currentQuestion < questions.Count - 1)
                {
                    currentQuestion++;
                }
                else
                {
                    foreach (var text in answers_text)
                    {
                        text.text = "";
                    }
                    state_question_text.text = "No more questions...";
                }

                #endregion

                #region ResetBooleans

                yield return new WaitForSeconds(timeToWaitAfterInput);
                isReadingInputs = true;
                isUpdatingText = true;
                isUpdatingScore = true;
                hasAnswered = false;

                #endregion
            }

            #endregion

            #region ReadInputs

            if (isReadingInputs)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentAnswer = 0;
                    hasAnswered = true;
                    isReadingInputs = false;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    currentAnswer = 1;
                    hasAnswered = true;
                    isReadingInputs = false;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    currentAnswer = 2;
                    hasAnswered = true;
                    isReadingInputs = false;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    currentAnswer = 3;
                    hasAnswered = true;
                    isReadingInputs = false;
                }
            }

            #endregion

            yield return null;
        }
    }
}
