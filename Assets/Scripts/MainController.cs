using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Vuforia;

public class MainController : MonoBehaviour
{
    #region Attributes
    public static MainController Instance { get; set; }
    /*
     *Serializable Attributes
     */
    // UI components
    public GameObject m_StartUI;
    public GameObject m_GuideUI;
    public GameObject m_ScoreUI;
    public GameObject m_QuitBtn;
    // Prefabs
    public GameObject m_QuizBox;
    public GameObject m_OptionItem;
    // Quiz root
    public Transform m_Root;
    // Transition duration
    public float guideUIDuration = 10.0f;
    public float scoreUIDuration = 10.0f;

    /*
     *Unserializable Attributes
     */
    private List<QAModel> quizzes;
    private List<QuizBoxPosModel> quizBoxPoses;
    private Camera mainCamera;
    private Transform planeStage;
    private Transform planeFinder;
    private string[] orderLabel = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    private List<int> scores = new List<int>();

    #endregion

    #region Methods
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!m_StartUI.activeInHierarchy)
            m_StartUI.SetActive(true);

        quizzes = QAContoller.Instance.m_Quizzes;
        quizBoxPoses = QuizBoxPosController.Instance.m_QuizBoxPoses;
        mainCamera = Camera.main;
        planeFinder = GameObject.FindGameObjectWithTag("PlaneFinder").transform;
        planeStage = GameObject.FindGameObjectWithTag("PlaneStage").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Listener for start button
    public void OnStartBtnClick()
    {
        if (m_StartUI.activeInHierarchy)
            m_StartUI.SetActive(false);

        if (m_ScoreUI.activeInHierarchy)
            m_ScoreUI.SetActive(false);

        if (!m_GuideUI.activeInHierarchy)
            m_GuideUI.SetActive(true);

        if (!m_QuitBtn.activeInHierarchy)
            m_QuitBtn.SetActive(true);

        StartCoroutine(DisableGuideUI());
    }

    // Listener for confirm&back button
    public void OnConfirmBackBtnClick()
    {
        if (!m_ScoreUI.activeInHierarchy)
        {
            m_ScoreUI.SetActive(true);
            TextMeshProUGUI scoreTxt = m_ScoreUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            int totalScore = 0;
            foreach(int score in scores)
            {
                totalScore += score;
            }
            scoreTxt.text = "Your Score: \n" + totalScore.ToString();
        }

        if (m_GuideUI.activeInHierarchy)
            m_GuideUI.SetActive(false);

        m_QuitBtn.SetActive(false);

        StartCoroutine(GoToStart());
    }

    // Listener for placing AR object
    public void OnARObjectsPlaced()
    {
        if(planeStage.GetChild(0).GetComponent<MeshRenderer>().enabled)
            // Disable placing AR object by hit the screen
            planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = false;
    }

    // Listener for option click (Score calculation)
    public void OnOptionClick(int quizNumber, bool rightAnswer)
    {
        scores[quizNumber] = rightAnswer ? 1 : 0;
    }

    // Disable Guide(Instruction) UI after 10 seconds
    IEnumerator DisableGuideUI()
    {
        yield return new WaitForSeconds(guideUIDuration);

        if (m_GuideUI.activeInHierarchy)
            m_GuideUI.SetActive(false);

        AttachQuizzes();

        // Enable AR
        SetARAvailability(true);
    }

    // Go to start scene after 10 seconds
    IEnumerator GoToStart()
    {
        yield return new WaitForSeconds(scoreUIDuration);

        SceneManager.LoadScene("MainScene");
    }

    // Attach Quizzes & Quiz Boxes
    private void AttachQuizzes()
    {
        int index = 0;

        foreach(QuizBoxPosModel quizBoxPos in quizBoxPoses)
        {
            // Instantiate quiz box
            Vector3 pos = quizBoxPos.Position;
            Quaternion rot = Quaternion.Euler(quizBoxPos.Rotation);
            GameObject tempQuizBox = Instantiate(m_QuizBox, pos, rot, m_Root);

            // Attach quizzes
            QuizBoxController qbController = tempQuizBox.GetComponent<QuizBoxController>();
            QAModel quiz = quizzes[index];
            qbController.m_QuestionTxt.GetComponent<TextMeshProUGUI>().text = quiz.Question;
            qbController.quizNumber = index;
            scores.Add(0);
            for(int i = 0; i < quiz.Answers.Length; i++)
            {
                GameObject tempOptionItem = Instantiate(m_OptionItem, qbController.m_OptionPanel.transform);
                tempOptionItem.GetComponent<TextMeshProUGUI>().text = orderLabel[i] + ". " + quiz.Answers[i];
                tempOptionItem.GetComponent<OptionController>().optionNumber = i;
                qbController.options.Add(tempOptionItem);
            }
            qbController.hint = quiz.Hint;

            index++;
        }
    }

    // Enable/Disable AR
    private void SetARAvailability(bool enable)
    {
        mainCamera.transform.GetComponent<VuforiaBehaviour>().enabled = enable;
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = enable;
        planeFinder.GetComponent<PlaneFinderBehaviour>().enabled = enable;
        planeStage.GetComponent<DefaultTrackableEventHandler>().enabled = enable;
    }

    #endregion
}
