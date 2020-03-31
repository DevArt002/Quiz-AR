using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizBoxPosController : MonoBehaviour
{
    public static QuizBoxPosController Instance { get; set; }

    /*
     *Serializable Attributes
     */
    // Quiz box position & rotation list
    public List<QuizBoxPosModel> m_QuizBoxPoses = new List<QuizBoxPosModel>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
}
