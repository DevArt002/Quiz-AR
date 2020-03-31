using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    #region Attributes
    /*
     *Serializable Attributes
     */
    // UI components
    private GameObject quizBox;
    // Option number
    [System.NonSerialized]
    public int optionNumber;

    #endregion

    #region Methods
    // Start is called before the first frame update
    private void Start()
    {
        quizBox = FindParentWithTag(gameObject, "QuizBox");
    }

    // Listener for click
    public void OnClick()
    {
        quizBox.GetComponent<QuizBoxController>().OnOptionClick(gameObject.GetInstanceID());
    }

    // Find parent with tag name
    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }
    #endregion
}
