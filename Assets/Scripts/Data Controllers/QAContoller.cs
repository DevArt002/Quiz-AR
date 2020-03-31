using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAContoller : MonoBehaviour
{
    public static QAContoller Instance { get; set; }

    /*
     *Serializable Attributes
     */
    // Question & Answer & hint list
    public List<QAModel> m_Quizzes = new List<QAModel>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
}
