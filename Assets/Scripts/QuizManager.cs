using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    List<string> questions = new List<string> {
        "Columna vertebralis", "Scapulae", "Tibia", "Ossa pedis", "Humerus", "Radius",
        "Cranium", "Pelvis", "Femur", "Vertebrae cervicales", "Sternum", "Ulna",
        "Ossa manus", "Clavicula", "Patella", "Fibula"
    };

    private bool answeringBlocked = false;
    public TMP_Text questionText;
    public GameObject feedbackpanel;
    public TMP_Text feedbackText;
    public TMP_InputField scoreText;
    public TMP_InputField rankText;
    public int counterForRankMid = 0;
    public int counterForRankPro = 0;
    private string currentBone;
    private int score = 0;
    private int correctAnswers = 0;

    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        feedbackpanel.SetActive(false);
        LoadScoreAndRank();
        NextQuestion();
    }

    void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame && !answeringBlocked)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CheckAnswer(hit.collider.gameObject.name);
            }
        }
    }

    void NextQuestion()
    {
        currentBone = questions[Random.Range(0, questions.Count)];
        questionText.text = "Finde: " + currentBone;
    }

    void CheckAnswer(string tappedObjectName)
    {
        if (answeringBlocked) return;
        answeringBlocked = true;

        string tappedNormalized = NormalizeName(tappedObjectName);
        string targetNormalized = NormalizeName(currentBone);

        bool isCorrect = tappedNormalized.Contains(targetNormalized);

        if (isCorrect)
        {
            score += 100;
            correctAnswers++;
            feedbackText.text = "Richtig!";
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            feedbackText.text = "Falsch!";
            audioSource.PlayOneShot(wrongSound);
        }

        string rank = GetRank(correctAnswers);
        scoreText.text = score.ToString();
        rankText.text = rank;

        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetString("Rank", rank);
        PlayerPrefs.Save();

        StartCoroutine(ShowFeedbackAndProceed());
    }

    IEnumerator ShowFeedbackAndProceed()
    {
        feedbackpanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        feedbackpanel.SetActive(false);

        if (correctAnswers == 3 && counterForRankMid == 0)
        {
            counterForRankMid = 1;
            PlayerPrefs.SetInt("counterForRankMid", counterForRankMid);
            PlayerPrefs.Save();
            SceneManager.LoadScene("NeuerRangMid"); 
        }
        else if (correctAnswers == 6 && counterForRankPro == 0)
        {
            counterForRankPro = 1;
            PlayerPrefs.SetInt("counterForRankPro", counterForRankPro);
            PlayerPrefs.Save();
            SceneManager.LoadScene("NeuerRangPro"); 
        }
        else
        {
            NextQuestion();
            answeringBlocked = false;
        }
    }

    string GetRank(int correctAnswers)
    {
        if (correctAnswers < 3) return "Noob";
        else if (correctAnswers < 6) return "Mid";
        else return "Pro";
    }

    string NormalizeName(string name)
    {
        return name.ToLower().Replace(" ", "").Replace("_", "").Replace("-", "");
    }

    void LoadScoreAndRank()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        string rank = PlayerPrefs.GetString("Rank", "Noob");
        correctAnswers = score / 100;

        counterForRankMid = PlayerPrefs.GetInt("counterForRankMid", 0);
        counterForRankPro = PlayerPrefs.GetInt("counterForRankPro", 0);

        scoreText.text = score.ToString();
        rankText.text = rank;
    }
}
