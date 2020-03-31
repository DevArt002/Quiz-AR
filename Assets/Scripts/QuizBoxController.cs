using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizBoxController : MonoBehaviour
{
    #region Attributes
    /*
     *Serializable Attributes
     */
    // UI components
    public GameObject m_QACanvas;
    public GameObject m_OptionPanel;
    public GameObject m_QuestionTxt;

    /*
     *Unserializable Attributes
     */
    [System.NonSerialized]
    public List<GameObject> options = new List<GameObject>();
    [System.NonSerialized]
    public int quizNumber;
    [System.NonSerialized]
    public int hint;
    #endregion

    #region Methods
    // Listener for option click
    public void OnOptionClick(int optionId)
    {
        foreach (GameObject option in options)
        {
            int instanceId = option.GetInstanceID();
            Transform toggleTransform = option.transform.GetChild(0);

            // Toggle active/disable 
            if (instanceId == optionId)
            {
                toggleTransform.GetComponent<Toggle>().isOn = true;
                // Check if the option is the right answer and affect the score system
                if(option.GetComponent<OptionController>().optionNumber == (hint - 1))
                {
                    MainController.Instance.OnOptionClick(quizNumber, true);
                }
                else
                {
                    MainController.Instance.OnOptionClick(quizNumber, false);
                }
            }
            else
            {
                toggleTransform.GetComponent<Toggle>().isOn = false;
            }
        }
    }

    // Listener for hint click
    public void OnHintClick()
    {
        print(hint - 1);
        GameObject rightOption = options[hint - 1];

        ColorBlock colors = rightOption.GetComponent<Button>().colors;
        colors.normalColor = new Color32(248, 89, 34, 255);
        colors.highlightedColor = new Color32(255, 194, 132, 255);
        colors.pressedColor = new Color32(255, 41, 0, 255);
        colors.selectedColor = new Color32(255, 141, 35, 255);

        rightOption.GetComponent<Button>().colors = colors;
    }

    // Listener for quiz box click
    private void OnMouseUp()
    {
        m_QACanvas.SetActive(!m_QACanvas.activeInHierarchy);
    }
    #endregion
}
